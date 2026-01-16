using Microsoft.Extensions.DependencyInjection;

namespace PremestiSe.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Register infrastructure services here
        return services;
    }
}
