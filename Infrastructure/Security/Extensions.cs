using Application.Security;
using Domain.Users.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Security;

internal static class Extensions
{
    public static IServiceCollection AddSecurity(this IServiceCollection services)
    {
        services.AddSingleton<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();
        services.AddScoped<IPasswordManager, PasswordManager>();

        return services;
    }
}
