using Microsoft.Extensions.DependencyInjection;
using Store.Application.Services;
using Store.Infrastructure.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Store.Application.Models;

namespace Store.Application.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddInfrastructureServices(configuration);
        services.AddTransient<IProductService, ProductService>();
        services.AddTransient<IOrderService, OrderService>();
        services.AddTransient<ICartService, CartService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IPasswordService, PasswordService>();
        services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddTransient<IReportService, ReportService>();
        services.AddTransient<ITaxCalculator, UKTaxCalculator>();
        services.AddTransient<ITaxCalculator, AustraliaTaxCalculator>();
        services.AddTransient<ITaxCalculatorFactory, AdvancedTaxCalculatorFactory>();
        return services;
    }
}
