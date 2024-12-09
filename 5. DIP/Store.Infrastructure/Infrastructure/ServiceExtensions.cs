using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Store.Infrastructure.Models;
namespace Store.Infrastructure.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        SqlMapper.AddTypeHandler(new JsonListTypeHandler<ProductRecord>());
        SqlMapper.AddTypeHandler(new JsonListTypeHandler<OrderRecord>());
        SqlMapper.AddTypeHandler(new JsonListTypeHandler<ItemRecord>());
        return services;
    }
}
