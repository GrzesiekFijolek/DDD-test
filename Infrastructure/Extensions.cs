using System.Text;
using Application.Common.CQRS;
using Application.Security;
using Domain.Common;
using Domain.Users.Entities;
using Infrastructure.Auth;
using Infrastructure.Database;
using Infrastructure.Database.Decorators;
using Infrastructure.Exceptions;
using Infrastructure.Logging.Decorators;
using Infrastructure.Security;
using Infrastructure.Time;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Extensions;

namespace Infrastructure;

public static class Extensions
{
    public static WebApplicationBuilder UseSerilogLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration
                .WriteTo.Console();
                // .WriteTo.Seq("http://localhost:5341");
        });

        return builder;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var infrastructureAssembly = typeof(AppDbContext).Assembly;

        services.Scan(s => s.FromAssemblies(infrastructureAssembly)
            .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        services.AddOptions<DatabaseOptions>()
            .Bind(configuration.GetSection(DatabaseOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<AuthOptions>()
            .Bind(configuration.GetSection(AuthOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<ExceptionMiddleware>();
        services.AddScoped<IUnitOfWork, AppDatabaseUnitOfWork>();
        
        services.AddPostgres(configuration);

        services.AddAuth(configuration);

        services.AddSingleton<IClock, Clock>();
        services.AddSingleton<IAuthenticator, JwtAuthenticator>();

        services.AddSingleton<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();
        services.AddScoped<IPasswordManager, PasswordManager>();

        services.TryDecorate(typeof(ICommandHandler<>), typeof(UnitOfWorkCommandHandlerDecorator<>));
        services.TryDecorate(typeof(ICommandHandler<,>), typeof(UnitOfWorkCommandHandlerDecorator<,>));

        services.TryDecorate(typeof(ICommandHandler<>), typeof(LoggingComandHandlerDecorator<>));
        services.TryDecorate(typeof(ICommandHandler<,>), typeof(LoggingComandHandlerDecorator<,>));
        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication application)
    {
        application.UseMiddleware<ExceptionMiddleware>();
        application.UseAuthentication();

        // application.MapControllers();

        return application;
    }


    private static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>((sp, x) =>
        {
            var options = configuration.GetOptions<DatabaseOptions>(DatabaseOptions.SectionName);
            x.UseNpgsql(options.ConnectionString,
                npgsql => npgsql.MigrationsHistoryTable(
                    options.MigrationsHistoryTable,
                    options.MigrationsHistorySchema));
        });
        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetOptions<AuthOptions>(AuthOptions.SectionName);

        services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(jwt =>
            {
                jwt.Audience = options.Audience;
                jwt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = options.Issuer,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SigningKey))
                };
            });

        return services;
    }
    
    internal static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : class, new()
    {
        var options = new T();
        var section = configuration.GetRequiredSection(sectionName);
        section.Bind(options);

        return options;
    }
}