using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Discounts
{
    /// <summary>
    /// Discount representing "buy X of one product and get the other product Y percent off" types of discounts 
    /// </summary>
    public class CombinationDiscount : IDiscount
    {
        private readonly string thresholdProductName;
        private readonly int thresholdQuantity;
        private readonly string targetProductName;
        private readonly decimal targetDiscountPercentage;

        /// <summary>
        /// Constructor - initializes a new instance of the CombinationDiscount class.
        /// </summary>
        /// <param name="thresholdProductName">Name of the threshold product</param>
        /// <param name="thresholdQuantity">Quantity of the threshold product needed to trigger the discount</param>
        /// <param name="targetProductName">Name of the target product that will be discounted</param>
        /// <param name="targetDiscountPercentage">Precentage by which the target product will be discounted - should be a number between 0 and 1</param>
        public CombinationDiscount(string thresholdProductName, int thresholdQuantity, string targetProductName, decimal targetDiscountPercentage)
        {
            this.thresholdProductName = thresholdProductName;
            this.targetProductName = targetProductName;

            if (thresholdQuantity <= 0)
            {
                throw new ArgumentException("Threshold quantity is negative or zero", "thresholdQuantity");
            }
            this.thresholdQuantity = thresholdQuantity;

            if (targetDiscountPercentage <= 0 || targetDiscountPercentage > 1)
            {
                throw new ArgumentException("Precentage is below or equal to zero or greater than one", "targetDiscountPercentage");
            }
            this.targetDiscountPercentage = targetDiscountPercentage;
        }

        /// <summary>
        /// Method which applies this discount on the given cost calculation items.
        /// For each threshold quantity of the threshold product present discount one target product by target discount percentage.
        /// </summary>
        /// <param name="items">Cost calculation items</param>
        /// <returns>Items modified in accordance with the discount logic</returns>
        public IList<ITotalCostCalculationItem> ApplyDiscount(IList<ITotalCostCalculationItem> items)
        {
            //fetch the target item
            var discountTarget = items.FirstOrDefault(x => x.ProductName == targetProductName);

            //if the target item doesn't exist, return unmodified items
            if (discountTarget == null)
            {
                return items;
            }

            //fetch the threshold item
            var thresholdItem = items.FirstOrDefault(x => x.ProductName == thresholdProductName);

            //if the threshold item doesn't exist, return unmodified items
            if (thresholdItem == null)
            {
                return items;
            }

            //the discount is stacking meaning if is n*thresholdQuantity quantity of the threshold item is present,
            //n of the target products are discouted
            var numberOfDiscountedProducts = Math.Min(discountTarget.ProductQuantity, thresholdItem.ProductQuantity / thresholdQuantity);
            discountTarget.Discount = (discountTarget.ProductUnitPrice * targetDiscountPercentage) * numberOfDiscountedProducts;
            return items;
        }

        /// <summary>
        /// Method which determines wheter this discount is applicable on the given cost calculation items items.
        /// This discount applies if there is enough quantity of the threshold product in the given items and 
        /// there is an item of the target product.
        /// </summary>
        /// <param name="items">Cost calculation items</param>
        /// <returns>
        /// Returns true if there is enough quantity of the threshold product in the given items and
        /// there is an item of the target product, otherwise false
        /// </returns>
        public bool IsDiscountApplicable(IList<ITotalCostCalculationItem> items)
        {
            return items.Any(x => x.ProductName == targetProductName) &&
                items.Any(x => x.ProductName == thresholdProductName && x.ProductQuantity >= thresholdQuantity);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Buy ")
                .Append(thresholdQuantity)
                .Append(" ")
                .Append(((thresholdQuantity - 1) > 1) ? thresholdProductName.ToString() + "s" : thresholdProductName.ToString())
                .Append(" and get one ")
                .Append(targetProductName)
                .Append(" at ")
                .Append(targetDiscountPercentage * 100)
                .Append("% off.");

            return sb.ToString();
        }
    }
}
