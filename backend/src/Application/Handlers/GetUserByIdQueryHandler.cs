using System.Data;
using Application.DTOs;
using Application.Interfaces;
using Application.Queries;
using Dapper;
using MediatR;

namespace Application.Handlers;

public class GetUserByIdQueryHandler(
    IDbConnection dbConnection,
    ILoggerManager logger,
    IRedisCache cache) : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private const string CachePrefix = "User:";

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = CachePrefix + request.UserId.ToString();
        var cachedUserJson = await cache.GetAsync<string>(cacheKey);
        if (!string.IsNullOrEmpty(cachedUserJson))
        {
            logger.LogInfo($"User {request.UserId} loaded from cache");
            var cachedUser = System.Text.Json.JsonSerializer.Deserialize<UserDto>(cachedUserJson);
            if (cachedUser != null) return cachedUser;
        }

        logger.LogInfo($"User not found in cache. Querying database for user {request.UserId}");

        const string sql = """

                                       SELECT "UserName",
                                              "Id",
                                              "Email",
                                              "FirstName",
                                              "LastName",
                                              "Gender"
                                       FROM "AspNetUsers"
                                       WHERE "Id" = @UserId
                           """;

        var user = await dbConnection.QueryFirstOrDefaultAsync<UserDto>(sql, new { request.UserId });

        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {request.UserId} not found.");
        }

        var userJson = System.Text.Json.JsonSerializer.Serialize(user);
        await cache.SetAsync(cacheKey, userJson, TimeSpan.FromMinutes(30));

        return user;
    }
}