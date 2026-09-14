using Application.Common.CQRS;
using Infrastructure.Auth;
using Infrastructure.Database;
using Infrastructure.Exceptions;
using Infrastructure.Logging;
using Infrastructure.Security;
using Infrastructure.Time;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class Extensions
{
    public static WebApplicationBuilder UseSerilogLogging(this WebApplicationBuilder builder)
    {
        builder.AddSerilog();

        return builder;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var infrastructureAssembly = typeof(AppDbContext).Assembly;

        services.Scan(s => s.FromAssemblies(infrastructureAssembly)
            .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(s => s.FromAssemblies(infrastructureAssembly)
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(s => s.FromAssemblies(infrastructureAssembly)
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddDatabase(configuration);
        services.AddAuth(configuration);
        services.AddSecurity();
        services.AddTime();
        services.AddExceptions();
        services.AddCommandLogging();

        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication application)
    {
        application.UseDatabase();
        application.UseMiddleware<ExceptionMiddleware>();
        application.UseAuthentication();

        // application.MapControllers();

        return application;
    }

    internal static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : class, new()
    {
        var options = new T();
        var section = configuration.GetRequiredSection(sectionName);
        section.Bind(options);

        return options;
    }
}
