using Store.Application.Models;

namespace Store.Application.Services;

public interface ITaxCalculator
{
    decimal CalculateTax(Order order, User user);
    bool CanHandle(string countryCode);
}