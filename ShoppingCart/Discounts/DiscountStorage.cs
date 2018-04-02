using System;
using System.Collections.Generic;

namespace ShoppingCart.Discounts
{
    /// <summary>
    /// A simple discount storage, discounts are stored in a list.
    /// </summary>
    public class DiscountStorage : IDiscountStorage
    {
        private IList<IDiscount> discounts;

        /// <summary>
        /// Constructor - initializes a new instance of the DiscountStorage class.
        /// </summary>
        /// <param name="">List of products that the storage should contain</param>
        public DiscountStorage(IList<IDiscount> discounts)
        {
            this.discounts = discounts ?? throw new ArgumentException("Discount list is null", "discounts");
        }

        /// <summary>
        /// Method which retreives all currently active discounts
        /// </summary>
        /// <returns>List of currently active discounts</returns>
        public IList<IDiscount> GetActiveDiscounts()
        {
            return discounts;
        }
    }
}
