using Application.Common.CQRS;
using Infrastructure.Logging.Decorators;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Infrastructure.Logging;

internal static class Extensions
{
    public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration
                .WriteTo.Console();
                // .WriteTo.Seq("http://localhost:5341");
        });

        return builder;
    }

    public static IServiceCollection AddCommandLogging(this IServiceCollection services)
    {
        services.TryDecorate(typeof(ICommandHandler<>), typeof(LoggingComandHandlerDecorator<>));
        services.TryDecorate(typeof(ICommandHandler<,>), typeof(LoggingComandHandlerDecorator<,>));

        return services;
    }
}
