namespace Store.Application.Services;

public class TaxCalculatorFactory : ITaxCalculatorFactory
{
    public ITaxCalculator GetCalculatorInstance(string countryCode)
        => countryCode switch
        {
            "AUS" => new AustraliaTaxCalculator(),
            "GBR" => new UKTaxCalculator(),
            _ => new NoTaxCalculator()
        };
}