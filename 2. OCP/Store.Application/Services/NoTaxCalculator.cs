using Store.Application.Models;

namespace Store.Application.Services;

public class NoTaxCalculator : ITaxCalculator
{
    public decimal CalculateTax(Order order, User user) => 0;
    public bool CanHandle(string countryCode) => false;
}