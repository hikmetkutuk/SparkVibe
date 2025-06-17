using System.Data;
using Application.DTOs;
using Application.Queries;
using Dapper;
using MediatR;

namespace Application.Handlers;

public class GetUserByIdQueryHandler(IDbConnection dbConnection) : IRequestHandler<GetUserByIdQuery, UserDto>
{
    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var sql = @"
            SELECT ""UserName"",
                   ""Id"",
                   ""Email"",
                   ""FirstName"",
                   ""LastName"",
                   ""Gender""
            FROM ""AspNetUsers""
            WHERE ""Id"" = @UserId";

        var user = await dbConnection.QueryFirstOrDefaultAsync<UserDto>(sql, new { request.UserId });

        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {request.UserId} not found.");
        }

        return user;
    }
}