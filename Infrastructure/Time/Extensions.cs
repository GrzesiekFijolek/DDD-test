using Domain.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Time;

internal static class Extensions
{
    public static IServiceCollection AddTime(this IServiceCollection services)
    {
        services.AddSingleton<IClock, Clock>();

        return services;
    }
}
