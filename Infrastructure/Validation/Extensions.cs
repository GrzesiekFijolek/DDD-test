using Application.Common.CQRS;
using FluentValidation;
using Infrastructure.Validation.Decorators;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Validation;

internal static class Extensions
{
    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(Extensions).Assembly);

        services.TryDecorate(typeof(ICommandHandler<>), typeof(ValidationCommandHandlerDecorator<>));
        services.TryDecorate(typeof(ICommandHandler<,>), typeof(ValidationCommandHandlerDecorator<,>));
        services.TryDecorate(typeof(IQueryHandler<,>), typeof(ValidationQueryHandlerDecorator<,>));

        return services;
    }
}
