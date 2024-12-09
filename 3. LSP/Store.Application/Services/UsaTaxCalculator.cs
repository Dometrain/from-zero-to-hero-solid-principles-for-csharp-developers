using System.Text.RegularExpressions;
using Store.Application.Models;

namespace Store.Application.Services;

public class UsaTaxCalculator : ITaxCalculator
{
    public decimal CalculateTax(Order order, User user)
    {
        var orderTotal = order.Items.Sum(x => x.Quantity * x.Product.Price);
        var totalToTax = orderTotal + order.DeliveryCost;

        var getState = GetState(user.DeliveryAddress);
        return totalToTax * GetTaxForState(getState);
    }

    public bool CanHandle(string countryCode) => countryCode == "USA";

    private string GetState(string address)
    {
        string pattern = @"\s([A-Z]{2})[\s$]?";

        var match = Regex.Match(address, pattern);
        if (!match.Success)
            return null;

        return match.Groups[1].Value;
    }

    private decimal GetTaxForState(string stateCode)
        => stateCode?.ToUpper() switch
        {
            "AL" => 0.04m,
            "AK" => 0m,
            "AZ" => 0.056m,
            "AR" => 0.065m,
            "CA" => 0.0725m,
            "CO" => 0.029m,
            "CT" => 0.0635m,
            "DE" => 0m,
            "DC" => 0.06m,
            "FL" => 0.06m,
            "GA" => 0.04m,
            "GU" => 0.04m,
            "HI" => 0.04166m,
            "ID" => 0.06m,
            "IL" => 0.0625m,
            "IN" => 0.07m,
            "IA" => 0.06m,
            "KS" => 0.065m,
            "KY" => 0.06m,
            "LA" => 0.0445m,
            "ME" => 0.055m,
            "MD" => 0.06m,
            "MA" => 0.0625m,
            "MI" => 0.06m,
            "MN" => 0.06875m,
            "MS" => 0.07m,
            "MO" => 0.04225m,
            "MT" => 0m,
            "NE" => 0.055m,
            "NV" => 0.0685m,
            "NH" => 0m,
            "NJ" => 0.06625m,
            "NM" => 0.05125m,
            "NY" => 0.04m,
            "NC" => 0.0475m,
            "ND" => 0.05m,
            "OH" => 0.0575m,
            "OK" => 0.045m,
            "OR" => 0m,
            "PA" => 0.06m,
            "PR" => 0.105m,
            "RI" => 0.07m,
            "SC" => 0.06m,
            "SD" => 0.04m,
            "TN" => 0.07m,
            "TX" => 0.0625m,
            "UT" => 0.061m,
            "VT" => 0.06m,
            "VA" => 0.053m,
            "WA" => 0.065m,
            "WV" => 0.06m,
            "WI" => 0.05m,
            "WY" => 0.04m,
            _ => throw new ArgumentException("Invalid state code")
        };
}
