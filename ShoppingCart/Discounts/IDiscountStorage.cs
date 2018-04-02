using System.Collections.Generic;

namespace ShoppingCart.Discounts
{
    /// <summary>
    /// Interface specifying a discount storage
    /// </summary>
    public interface IDiscountStorage
    {
        /// <summary>
        /// Method which should retreive all currently active discounts
        /// </summary>
        /// <returns>List of currently active discounts</returns>
        IList<IDiscount> GetActiveDiscounts();
    }
}
