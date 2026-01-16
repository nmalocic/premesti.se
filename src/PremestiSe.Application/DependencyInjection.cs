using Microsoft.Extensions.DependencyInjection;

namespace PremestiSe.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register application services here
        return services;
    }
}
