using ShoppingCart.Discounts;
using System.Collections.Generic;

namespace ShoppingCart
{
    /// <summary>
    /// Class representing one total cost calculation.
    /// Contains the items used, calculated cost and applied discounts.
    /// </summary>
    internal class TotalCostCalculation : ITotalCostCalculation
    {
        public decimal TotalCost { get; set; }

        public IList<ITotalCostCalculationItem> CalculationItems { get; set; }

        public IList<IDiscount> AppliedDiscounts { get; set; }
    }
}