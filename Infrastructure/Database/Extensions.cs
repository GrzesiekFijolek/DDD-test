using Application.Common.CQRS;
using Infrastructure.Database.Decorators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Database;

internal static class Extensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<DatabaseOptions>()
            .Bind(configuration.GetSection(DatabaseOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddDbContext<AppDbContext>((sp, x) =>
        {
            var options = configuration.GetOptions<DatabaseOptions>(DatabaseOptions.SectionName);
            x.UseNpgsql(options.ConnectionString,
                npgsql => npgsql.MigrationsHistoryTable(
                    options.MigrationsHistoryTable,
                    options.MigrationsHistorySchema));
        });

        services.AddScoped<IUnitOfWork, AppDatabaseUnitOfWork>();

        services.TryDecorate(typeof(ICommandHandler<>), typeof(UnitOfWorkCommandHandlerDecorator<>));
        services.TryDecorate(typeof(ICommandHandler<,>), typeof(UnitOfWorkCommandHandlerDecorator<,>));

        return services;
    }
}
