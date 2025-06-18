using System.Data;
using Application.DTOs;
using Application.Interfaces;
using Application.Queries;
using Dapper;
using MediatR;

namespace Application.Handlers;

public class GetAllUsersQueryHandler(
    IDbConnection dbConnection,
    ILoggerManager logger,
    IRedisCache cache) : IRequestHandler<GetAllUsersQuery, List<UserDto>>
{
    private const string CacheKey = "Users:All";

    public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
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