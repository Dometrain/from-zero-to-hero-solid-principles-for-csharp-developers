using Store.Common.Helpers;

namespace Store.Application.Services;

public class AdvancedTaxCalculatorFactory : ITaxCalculatorFactory
{
    private readonly IEnumerable<ITaxCalculator> _calculators;
    public AdvancedTaxCalculatorFactory(IEnumerable<ITaxCalculator> calculators)
    {
        _calculators = calculators.NotNull();
    }

    public ITaxCalculator GetCalculatorInstance(string countryCode)
    {
        var calculator = _calculators.FirstOrDefault(x => x.CanHandle(countryCode));
        if (calculator == null)
            return new NoTaxCalculator();

        return calculator;
    }
}