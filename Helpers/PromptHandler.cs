using MateMachine.CurrencyConverter.Models;

namespace MateMachine.CurrencyConverter.Helpers
{
    internal class PromptHandler
    {
        internal static void Wellcome()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine();
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("--- Wellcom to MateMachine - Currency converter ---");
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static string? Menu()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine("1: Convert");
            Console.WriteLine("2: Display rates");
            Console.WriteLine("3: Update configuration");
            Console.WriteLine("4: Clear configuration");
            Console.WriteLine("5: Clear screen");
            Console.WriteLine("---------------");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("0: Exit");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Enter your command: ");
            return Console.ReadLine();
        }

        internal static void ClearScreen()
        {
            Console.Clear();
            Wellcome();
        }

        internal static void InvalidCommand()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine();
            Console.WriteLine("-------------------------");
            Console.WriteLine("Invalid command!");
            Console.WriteLine("-------------------------");
            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void Error(Exception exp)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine(" *** ERROR ***");
            Console.WriteLine(exp.Message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void AnyKeyToCountinue()
        {
            Console.WriteLine("");
            Console.WriteLine("Press any key to continue ...");
            Console.ReadLine();
        }


        internal static ConvertModel ConvertInput()
        {
            Console.WriteLine();
            Console.Write("Convert from currency: ");
            var fromCurrency = Console.ReadLine();

            Console.Write("Convert to currency: ");
            var toCurrency = Console.ReadLine();

            Console.Write("Convert amount: ");
            var amount = double.Parse(Console.ReadLine() ?? "0");

            return new ConvertModel(fromCurrency, toCurrency, amount);
        }

        internal static void ConvertOutput(ConvertModel input, double output)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("-------------------------");
            Console.WriteLine("{0:#,0.##} {1} = {2:#,0.##} {3}", input.Amount, input.FromCurrency?.ToUpper(), output, input.ToCurrency?.ToUpper());
            Console.WriteLine("-------------------------");
            Console.ForegroundColor = ConsoleColor.White;
            AnyKeyToCountinue();
        }


        internal static IEnumerable<ConvertModel> UpdateConfigurationInput()
        {
            var rates = new List<ConvertModel>();

            Console.WriteLine();
            var inputKey = "1";

            while (inputKey != "0")
            {

                if (inputKey == "1")
                {
                    Console.Write("From currency: ");
                    var fromCurrency = Console.ReadLine();

                    Console.Write("To currency: ");
                    var toCurrency = Console.ReadLine();

                    Console.Write("Rate: ");
                    var rate = double.Parse(Console.ReadLine() ?? "0");

                    rates.Add(new ConvertModel(fromCurrency, toCurrency, rate));
                }

                else
                {
                    InvalidCommand();
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine();
                Console.WriteLine("--------------");
                Console.WriteLine("1: Next");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("0: Finish");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;

                Console.Write("Enter your command: ");
                inputKey = Console.ReadLine();
            }

            return rates;
        }

        internal static void UpdateConfigurationOutput(IEnumerable<ConvertModel> rates)
        {
            Console.WriteLine();
            Console.WriteLine("Updated rates:");

            Console.ForegroundColor = ConsoleColor.Green;

            foreach (var rate in rates)
                Console.WriteLine(" + ({0} => {1}) {2:n4}",
                    rate.FromCurrency?.ToUpper(),
                    rate.ToCurrency?.ToUpper(),
                    rate.Amount);

            Console.ForegroundColor = ConsoleColor.White;
            AnyKeyToCountinue();
        }


        internal static void DisplayRates(Dictionary<string, Dictionary<string, double>> rates)
        {
            Console.WriteLine();
            Console.WriteLine("Current rates:");

            if (rates.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine(" -- Rates have not yet been determined! --");
            }

            else
            {
                // From rates
                foreach (var fromRate in rates.Select((fromCurrency, fromIndx) => new { fromCurrency, fromIndx }))
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(" #{0}: {1}", fromRate.fromIndx + 1, fromRate.fromCurrency.Key);

                    // To rates list
                    foreach (var toRate in fromRate.fromCurrency.Value)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine("      => {0} = {1:n4}", toRate.Key, toRate.Value);
                    }

                    // Seprator
                    Console.WriteLine();
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
            AnyKeyToCountinue();
        }


        internal static bool ClearConfigurationInput()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(" ** Are you sure to clear all configurations?");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("    1: No, Never mind ...");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("    0: Yes, clear all configurations!");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("Enter your command: ");
            return Console.ReadLine() == "0";
        }

        internal static void ClearConfigurationOutput()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine();
            Console.WriteLine("----------------------------------");
            Console.WriteLine(" ** All configurations cleared. **");
            Console.WriteLine("----------------------------------");
            Console.ForegroundColor = ConsoleColor.White;
            AnyKeyToCountinue();
        }
    }
}
