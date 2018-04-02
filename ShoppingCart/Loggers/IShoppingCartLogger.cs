namespace ShoppingCart.Loggers
{
    /// <summary>
    /// Interface representig a logger which can be used to log a total cost calculation
    /// </summary>
    public interface IShoppingCartLogger
    {
        /// <summary>
        /// Method which should log the total cost calculation
        /// </summary>
        /// <param name="calculation">Total cost calculation</param>
        void Log(ITotalCostCalculation calculation);
    }
}
