using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.EntityFramework.Context;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Options;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection") ??
                              throw new InvalidOperationException()));

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService>(provider => new TokenService(configuration));
        services.AddScoped<IUserRepository, UserRepository>();
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        return services;
    }
}