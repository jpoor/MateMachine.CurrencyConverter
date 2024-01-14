namespace MateMachine.CurrencyConverter.Services
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private readonly Dictionary<string, Dictionary<string, double>> _conversionRates = new Dictionary<string, Dictionary<string, double>>();
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        public Dictionary<string, Dictionary<string, double>> Rates() => _conversionRates;

        public void ClearConfiguration()
        {
            _lock.EnterWriteLock();
            try
            {
                _conversionRates.Clear();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRates)
        {
            _lock.EnterWriteLock();
            try
            {
                // check to update or insert new
                foreach (var rate in conversionRates)
                {
                    var fromCurrency = rate.Item1.ToUpper();
                    var toCurrency = rate.Item2.ToUpper();
                    var rateAmount = rate.Item3;

                    // insert if not exist
                    if (!_conversionRates.ContainsKey(fromCurrency))
                        _conversionRates[fromCurrency] = new Dictionary<string, double>();

                    // update rate
                    _conversionRates[fromCurrency][toCurrency] = rateAmount;

                    // insert reverse currency rate if not exist
                    if (!_conversionRates.ContainsKey(toCurrency))
                        _conversionRates[toCurrency] = new Dictionary<string, double>();

                    // update reverse rate
                    _conversionRates[toCurrency][fromCurrency] = 1 / rateAmount;
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public double Convert(string fromCurrency, string toCurrency, double amount)
        {
            // Detect case sensitive
            fromCurrency = fromCurrency.ToUpper();
            toCurrency = toCurrency.ToUpper();

            // Same currency
            if (fromCurrency == toCurrency)
                return amount;

            // Not Exist currencies
            if (!_conversionRates.ContainsKey(fromCurrency) || !_conversionRates.ContainsKey(toCurrency))
                throw new ArgumentException($"Cannot convert from {fromCurrency} to {toCurrency}");

            _lock.EnterReadLock();
            try
            {
                // Generate a graph by breadth-first search algorithm
                var visitedNodes = new HashSet<string>();
                var queue = new Queue<Tuple<string, double>>();
                queue.Enqueue(new Tuple<string, double>(fromCurrency, amount));

                while (queue.Count > 0)
                {
                    var current = queue.Dequeue();
                    var currency = current.Item1;
                    var value = current.Item2;

                    if (currency == toCurrency)
                        return value;

                    visitedNodes.Add(currency);

                    foreach (var pair in _conversionRates[currency])
                    {
                        var nextCurrency = pair.Key;
                        var rate = pair.Value;

                        if (!visitedNodes.Contains(nextCurrency))
                            queue.Enqueue(new Tuple<string, double>(nextCurrency, value * rate));
                    }
                }

                throw new ArgumentException($"Cannot convert from {fromCurrency} to {toCurrency}");
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }
}
