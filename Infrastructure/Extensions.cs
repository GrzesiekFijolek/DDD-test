using Infrastructure.Database;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<DatabaseOptions>()
            .Bind(configuration.GetSection(DatabaseOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<ExceptionMiddleware>();
        
        services.AddPostgres();
        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication application)
    {
        application.UseMiddleware<ExceptionMiddleware>();

        return application;
    }


    private static IServiceCollection AddPostgres(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>((sp, x) =>
        {
            var options = sp.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            x.UseNpgsql(options.ConnectionString,
                npgsql => npgsql.MigrationsHistoryTable(
                    options.MigrationsHistoryTable,
                    options.MigrationsHistorySchema));
        });
        return services;
    }
}