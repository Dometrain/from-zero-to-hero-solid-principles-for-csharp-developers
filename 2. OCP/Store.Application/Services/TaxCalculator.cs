using Store.Application.Models;

namespace Store.Application.Services;

public class TaxCalculator
{
    public decimal CalculateTax(Order order, User user)
    {
        var orderTotal = order.Items.Sum(x => x.Quantity * x.Product.Price);
        var totalToTax = orderTotal + order.DeliveryCost;

        return totalToTax * 0.20m;
    }
}
