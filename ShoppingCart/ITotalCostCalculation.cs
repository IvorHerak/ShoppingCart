using ShoppingCart.Discounts;
using System.Collections.Generic;

namespace ShoppingCart
{
    /// <summary>
    /// Interface representing one total cost caluclation, it can store calculation items,
    /// total calculated cost and the applied discounts.
    /// </summary>
    public interface ITotalCostCalculation
    {
        decimal TotalCost { get; set; }

        IList<ITotalCostCalculationItem> CalculationItems { get; set; }

        IList<IDiscount> AppliedDiscounts { get; set; }
    }
}
