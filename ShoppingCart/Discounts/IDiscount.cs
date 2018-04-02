using System.Collections.Generic;

namespace ShoppingCart.Discounts
{
    /// <summary>
    /// Interface that represents a discount which can be applied to a list of cost calculation items.
    /// </summary>
    public interface IDiscount
    {
        /// <summary>
        /// Method which determines wheter this discount is applicable on the given cost calculation items
        /// </summary>
        /// <param name="items">Total cost calculation items</param>
        /// <returns>True if the discount is applicable, otherwise false</returns>
        bool IsDiscountApplicable(IList<ITotalCostCalculationItem> items);

        /// <summary>
        /// Method which applies this discount on the given cost calculation items.
        /// The idea is to modify the items in accordance with the discount logic and return the modified items as a result.
        /// Items should be grouped by product.
        /// </summary>
        /// <param name="items">Total cost calculation items</param>
        /// <returns>Items modified in accordance with the discount logic</returns>
        IList<ITotalCostCalculationItem> ApplyDiscount(IList<ITotalCostCalculationItem> items);
    }
}