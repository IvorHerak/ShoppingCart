using ShoppingCart.Loggers;

namespace ShoppingCart.Test.Infrastructure
{
    /// <summary>
    /// Test logger, it saves the last logged calculation in memory
    /// and exposes a getter to fetch the last calculation.
    /// </summary>
    public class TestShoppingCartLogger : IShoppingCartLogger
    {
        public void Log(ITotalCostCalculation calculation)
        {
            LastCalculation = calculation;
        }

        public ITotalCostCalculation LastCalculation { get; private set; }
    }
}
