using System;
using System.Text;

namespace ShoppingCart.Loggers
{
    /// <summary>
    /// Logger which is used to log the total cost calculation to the console.
    /// </summary>
    public class ConsoleShoppingCartLogger : IShoppingCartLogger
    {
        /// <summary>
        /// Method which logs the total cost calculation to the console
        /// </summary>
        /// <param name="calculation">Total cost calculation</param>
        public void Log(ITotalCostCalculation calculation)
        {
            var sb = new StringBuilder();
            sb.Append("Total cost calculation analytics:");
            sb.Append(Environment.NewLine).Append(Environment.NewLine);
            sb.Append("Items:");
            sb.Append(Environment.NewLine);

            if (calculation.CalculationItems.Count == 0)
            {
                sb.Append("No items!").Append(Environment.NewLine);
            }

            foreach (var item in calculation.CalculationItems)
            {
                sb.Append("Product name: ").Append(item.ProductName).Append('|');
                sb.Append("Unit price: ").Append(item.ProductUnitPrice).Append('|');
                sb.Append("Quantity: ").Append(item.ProductQuantity).Append('|');
                sb.Append("Discount: ").Append(item.Discount).Append('|');
                sb.Append("Total: ").Append(item.TotalPrice).Append('|');
                sb.Append(Environment.NewLine);
            }

            sb.Append(Environment.NewLine).Append(Environment.NewLine);
            sb.Append("Discounts:");
            sb.Append(Environment.NewLine);
            if (calculation.AppliedDiscounts.Count == 0)
            {
                sb.Append("No discounts!").Append(Environment.NewLine);
            }
            foreach (var discount in calculation.AppliedDiscounts)
            {
                sb.Append(discount.ToString());
                sb.Append(Environment.NewLine);
            }
            sb.Append(Environment.NewLine).Append(Environment.NewLine);
            sb.Append("Total basket cost:");
            sb.Append(Environment.NewLine);
            sb.Append(calculation.TotalCost);

            Console.WriteLine(sb.ToString());
        }
    }
}
