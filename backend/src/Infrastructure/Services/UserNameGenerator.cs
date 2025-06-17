using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Helpers;

namespace Infrastructure.Services;

public class UserNameGenerator(UserManager<ApplicationUser> userManager) : IUserNameGenerator
{
    public async Task<string> GenerateUserNameAsync(string firstName, string lastName)
    {
        var cleanFirstName = firstName.ToAsciiOnly();
        var cleanLastName = lastName.ToAsciiOnly();

        var baseUserName = $"{cleanFirstName}{cleanLastName}".ToLowerInvariant();

        var finalUserName = baseUserName;
        var count = 1;

        while (await userManager.Users.AnyAsync(u => u.UserName == finalUserName))
        {
            finalUserName = $"{baseUserName}{count}";
            count++;
        }

        return finalUserName;
    }
}