var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var currencyConverterController = new CurrencyConverterController();

app.MapGet("/", (context) => context.Response.WriteAsync("Welcom back"));

//https://localhost:7022/currencies
app.Map("/currencies", () => currencyConverterController.GetCurrencies());

//https://localhost:7022/exchangeRate/USD/EUR
app.Map("/exchangeRate/{sourceCurrency}/{targetCurrency}", (string sourceCurrency, string targetCurrency) =>
currencyConverterController.GetExchangeRate(sourceCurrency, targetCurrency));

//https://localhost:7022/convertCurrency/USD/EUR/100
app.Map("/convertCurrency/{sourceCurrency}/{targetCurrency}/{amount}", (string sourceCurrency, string targetCurrency, decimal amount) =>
currencyConverterController.ConvertCurrency(sourceCurrency, targetCurrency, amount));

app.Run();

class Currency
{
    public string Code { get; set; }
    public string Name { get; set; }
}

class ExchangeRate
{
    public string SourceCurrency { get; set; }
    public string TargetCurrency { get; set; }
    public decimal Rate { get; set; }    
}

class CurrencyConverterController
{
    private List<Currency> _currencies;
    private List<ExchangeRate> _exchangeRates;

    public CurrencyConverterController()
    {
        _currencies = new List<Currency>
        {
            new Currency {Code = "USD", Name = " US Dollar"},
            new Currency {Code = "EUR", Name = " US Euro"},
            new Currency {Code = "GBP", Name = " British Money"},
        };

        _exchangeRates = new List<ExchangeRate>
        {
            new ExchangeRate {SourceCurrency = "USD", TargetCurrency = "EUR", Rate = 0.85m},
            new ExchangeRate {SourceCurrency = "USD", TargetCurrency = "GBP", Rate = 0.75m},
            new ExchangeRate {SourceCurrency = "EUR", TargetCurrency = "USD", Rate = 1.18m},
            new ExchangeRate {SourceCurrency = "EUR", TargetCurrency = "GBP", Rate = 0.90m},
            new ExchangeRate {SourceCurrency = "GBP", TargetCurrency = "USD", Rate = 1.33m},
            new ExchangeRate {SourceCurrency = "GBP", TargetCurrency = "EUR", Rate = 1.11m},
        };
    }
    public List<Currency> GetCurrencies()
    {
        return _currencies;
    }

    public decimal GetExchangeRate(string sourceCurrency, string targetCurrency)
    {
        var exchangeRate = _exchangeRates.FirstOrDefault(r => r.SourceCurrency == sourceCurrency  && r.TargetCurrency == targetCurrency);

        if (exchangeRate == null)
        {
            throw new Exception("Exchange rate not found");
        }
        return exchangeRate.Rate;
    }

    public decimal ConvertCurrency(string sourceCurrency, string targetCurrency, decimal amount)
    {
        var exchangeRate = GetExchangeRate(sourceCurrency, targetCurrency);

        return amount * exchangeRate;
    }
}