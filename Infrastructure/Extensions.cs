using Infrastructure.Database;
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

        services.AddPostgres();
        return services;
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