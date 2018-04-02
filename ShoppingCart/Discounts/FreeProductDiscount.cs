using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Discounts
{
    /// <summary>
    /// Discount representing "buy X-1 products get (X)th free" types of discounts 
    /// </summary>
    public class FreeProductDiscount : IDiscount
    {
        private readonly string productName;
        private readonly int thresholdQuantity;

        /// <summary>
        /// Constructor - initializes a new instance of the FreeProductDiscount class.
        /// </summary>
        /// <param name="productName">Name of the product for which the discount is applied</param>
        /// <param name="thresholdQuantity">Quantity of the product needed to trigger the discount</param>
        public FreeProductDiscount(string productName, int thresholdQuantity)
        {
            this.productName = productName;

            if (thresholdQuantity <= 0)
            {
                throw new ArgumentException("Threshold quantity is negative or zero", "thresholdQuantity");
            }
            this.thresholdQuantity = thresholdQuantity;
        }

        /// <summary>
        /// Method which applies this discount on the given cost calculation items.
        /// For each threshold quantity of the product present give one product for free.
        /// </summary>
        /// <param name="items">cost calculation items</param>
        /// <returns>Items modified in accordance with the discount logic</returns>
        public IList<ITotalCostCalculationItem> ApplyDiscount(IList<ITotalCostCalculationItem> items)
        {
            //fetch the target item
            var discountTarget = items.FirstOrDefault(x => x.ProductName == productName);

            //if the target item doesn't exist, return unmodified items
            if (discountTarget == null)
            {
                return items;
            }

            //the discount is stacking meaning if n*thresholdQuantity quantity of the target item is present, n of them are free
            var numberOfFreeProducts = discountTarget.ProductQuantity / thresholdQuantity;

            //to make items free, we give them a discount in amount of their full unit price
            discountTarget.Discount = numberOfFreeProducts * discountTarget.ProductUnitPrice;
            return items;
        }

        /// <summary>
        /// Method which determines wheter this discount is applicable on the given cost calculation items.
        /// This discount applies if there is enough quantity of the target product in the given items.
        /// </summary>
        /// <param name="items">Cost calculation items</param>
        /// <returns>Returns true if there is enough quantity of the target product in the given items, otherwise false</returns>
        public bool IsDiscountApplicable(IList<ITotalCostCalculationItem> items)
        {
            return items.Any(x => x.ProductName == productName && x.ProductQuantity >= thresholdQuantity);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Buy ")
                .Append(thresholdQuantity - 1)
                .Append(" ")
                .Append(((thresholdQuantity - 1) > 1) ? productName.ToString() + "s" : productName.ToString())
                .Append(" and get the next ")
                .Append(productName)
                .Append(" for free.");

            return sb.ToString();
        }
    }
}
