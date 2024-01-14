using MateMachine.CurrencyConverter.Helpers;
using MateMachine.CurrencyConverter.Services;

var converterService = new CurrencyConverter();

// Default values
converterService.UpdateConfiguration(new List<Tuple<string, string, double>>
 {
    Tuple.Create("USD", "CAD", 1.34),
    Tuple.Create("CAD", "GBP", 0.58),
    Tuple.Create("USD", "EUR", 0.86)
 });

// Welcome texts
PromptHandler.Wellcome();

var inputKey = "-";
while (inputKey != "0")
{
    // Show menu
    inputKey = PromptHandler.Menu();

    try
    {
        switch (inputKey)
        {
            // Convert
            case "1":
                var convertModel = PromptHandler.ConvertInput();
                var ConvertResult = converterService.Convert(convertModel.FromCurrency!, convertModel.ToCurrency!, convertModel.Amount);
                PromptHandler.ConvertOutput(convertModel, ConvertResult);
                break;

            // Display rates
            case "2":
                PromptHandler.DisplayRates(converterService.Rates());
                break;

            // Update Configuration
            case "3":
                var updatedRates = PromptHandler.UpdateConfigurationInput();
                converterService.UpdateConfiguration(updatedRates.Select(x => new Tuple<string, string, double>(x.FromCurrency!, x.ToCurrency!, x.Amount)));
                PromptHandler.UpdateConfigurationOutput(updatedRates);
                break;

            // Clear Configuration
            case "4":
                var clearingResult = PromptHandler.ClearConfigurationInput();
                if (clearingResult)
                {
                    converterService.ClearConfiguration();
                    PromptHandler.ClearConfigurationOutput();
                }
                break;

            // Clear screen
            case "5":
                PromptHandler.ClearScreen();
                break;

            default:
                if (inputKey != "0") PromptHandler.InvalidCommand();
                break;
        }
    }

    catch (Exception exp)
    {
        PromptHandler.Error(exp);
    }
}