namespace MateMachine.CurrencyConverter.Models
{
    public record ConvertModel(string? FromCurrency, string? ToCurrency, double Amount);
}
