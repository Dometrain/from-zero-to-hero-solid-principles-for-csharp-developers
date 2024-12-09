namespace Store.Application.Services;

public interface ITaxCalculatorFactory
{
    ITaxCalculator GetCalculatorInstance(string countryCode);
}