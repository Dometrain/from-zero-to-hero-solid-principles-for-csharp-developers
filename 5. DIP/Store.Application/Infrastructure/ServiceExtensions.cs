using Microsoft.Extensions.DependencyInjection;
using Store.Infrastructure.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Store.Application.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddInfrastructureServices(configuration);
        return services;
    }
}
