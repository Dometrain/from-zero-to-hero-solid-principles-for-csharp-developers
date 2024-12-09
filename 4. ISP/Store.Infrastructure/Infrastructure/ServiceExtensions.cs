using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Store.Infrastructure.Data;
using Store.Infrastructure.Models;
namespace Store.Infrastructure.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("Postgres");
        services.AddNpgsqlDataSource(connectionString, builder => { builder.EnableDynamicJson(); });
        services.AddTransient<IReadProducts, ProductRepository>();
        services.AddTransient<IReadCart, CartRepository>();
        services.AddTransient<IWriteCart, CartRepository>();
        services.AddTransient<IReadOrders, OrderRepository>();
        services.AddTransient<IWriteOrder, OrderRepository>();
        services.AddTransient<IUserRepository, UserRepository>();

        SqlMapper.AddTypeHandler(new JsonListTypeHandler<ProductRecord>());
        SqlMapper.AddTypeHandler(new JsonListTypeHandler<OrderRecord>());
        SqlMapper.AddTypeHandler(new JsonListTypeHandler<ItemRecord>());
        return services;
    }
}
