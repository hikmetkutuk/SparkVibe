using System.Data;
using Application.Features.DTOs;
using Application.Features.Queries;
using Application.Interfaces;
using Dapper;
using MediatR;

namespace Application.Features.Handlers;

public class UserListHandler(
    IDbConnection dbConnection,
    ILoggerManager logger,
    IRedisCache cache) : IRequestHandler<UserListQuery, List<UserDto>>
{
    private const string CacheKey = "Users:All";

    public async Task<List<UserDto>> Handle(UserListQuery request, CancellationToken cancellationToken)
    {
        var cachedUsersJson = await cache.GetAsync<string>(CacheKey);
        if (!string.IsNullOrEmpty(cachedUsersJson))
        {
            logger.LogInfo("All users loaded from cache");
            var cachedUsers = System.Text.Json.JsonSerializer.Deserialize<List<UserDto>>(cachedUsersJson);
            if (cachedUsers != null) return cachedUsers;
        }

        logger.LogInfo("All users not found in cache. Querying database...");

        const string sql = """
                               SELECT "UserName",
                                      "Id",
                                      "Email",
                                      "FirstName",
                                      "LastName",
                                      "Gender"
                               FROM "AspNetUsers"
                           """;

        var users = (await dbConnection.QueryAsync<UserDto>(sql)).ToList();

        if (users.Count == 0)
        {
            logger.LogWarn("No users found in the database.");
            return [];
        }

        var usersJson = System.Text.Json.JsonSerializer.Serialize(users);
        await cache.SetAsync(CacheKey, usersJson, TimeSpan.FromMinutes(30));

        return users;
    }
}