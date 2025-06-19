using System.Data;
using Application.Features.DTOs;
using Application.Features.Queries;
using Application.Interfaces;
using Dapper;
using MediatR;

namespace Application.Features.Handlers;

public class RoleListHandler(
    IDbConnection dbConnection,
    ILoggerManager logger,
    IRedisCache cache) : IRequestHandler<RoleListQuery, List<RoleDto>>
{
    private const string CacheKey = "Roles:All";

    public async Task<List<RoleDto>> Handle(RoleListQuery request, CancellationToken cancellationToken)
    {
        var cachedRolesJson = await cache.GetAsync<string>(CacheKey);
        if (!string.IsNullOrEmpty(cachedRolesJson))
        {
            logger.LogInfo("All roles loaded from cache");
            var cachedRoles = System.Text.Json.JsonSerializer.Deserialize<List<RoleDto>>(cachedRolesJson);
            if (cachedRoles != null) return cachedRoles;
        }

        logger.LogInfo("All roles not found in cache. Querying database...");

        const string sql = """
                               SELECT "Name",
                                      "Id"
                               FROM "AspNetRoles"
                           """;

        var roles = (await dbConnection.QueryAsync<RoleDto>(sql)).ToList();

        if (roles.Count == 0)
        {
            logger.LogWarn("No roles found in the database.");
            return [];
        }

        var json = System.Text.Json.JsonSerializer.Serialize(roles);
        await cache.SetAsync(CacheKey, json, TimeSpan.FromMinutes(30));

        logger.LogInfo("All roles loaded from database and cached successfully.");
        return roles;
    }
}