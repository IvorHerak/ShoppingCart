using ShoppingCart.Discounts;
using ShoppingCart.Loggers;
using ShoppingCart.Products;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart
{
    /// <summary>
    /// Class that represents a shopping cart, you can add products to it and calculate the items total price
    /// </summary>
    public class ShoppingCart
    {
        private readonly IProductStorage productStorage;
        private readonly IDiscountStorage discountStorage;
        private readonly IShoppingCartLogger logger;
        private readonly IList<ShoppingCartItem> items = new List<ShoppingCartItem>();

        /// <summary>
        /// Constructor - initializes a new instance of the ShoppingCart class.
        /// </summary>
        /// <param name="productStorage">Storage used to validate products and fetch their unit prices</param>
        /// <param name="discountStorage">Storage used to fetch all active discounts</param>
        /// <param name="logger">Logger used to log total cost calculations</param>
        public ShoppingCart(IProductStorage productStorage, IDiscountStorage discountStorage, IShoppingCartLogger logger = null)
        {
            //validate the storages
            this.productStorage = productStorage ?? throw new ArgumentException("Product storage is null", "productStorage");
            this.discountStorage = discountStorage ?? throw new ArgumentException("Discount storage is null", "discountStorage");

            this.logger = logger ?? new ConsoleShoppingCartLogger();
        }

        /// <summary>
        /// Gets the copy of the current list of the shopping cart current items
        /// </summary>
        public IList<IShoppingCartItem> GetShoppingCartItems()
        {
            return new List<IShoppingCartItem>(items.Select(x => x.Copy()));
        }

        /// <summary>
        /// Method that adds a product to the shopping cart.
        /// </summary>
        /// <param name="product">Product to be added</param>
        /// <param name="quantity">Quantity of the product to be added</param>
        public void AddProduct(string productName, int quantity)
        {
            //validate the parameters
            if (string.IsNullOrEmpty(productName))
            {
                throw new ArgumentException("Product name is null or empty", "productName");
            }

            var product = productStorage.GetProductByNameOrDefault(productName);
            if (product == null)
            {
                throw new ArgumentException("Unknown product", "productName");
            }

            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity is negative or zero", "quantity");
            }

            // we group the same products in the item list
            var itemWithSameProduct = items.FirstOrDefault(x => x.Product.Name == product.Name);
            if (itemWithSameProduct == null)
            {
                items.Add(new ShoppingCartItem { Product = product, Quantity = quantity });
            }
            else
            {
                itemWithSameProduct.Quantity += quantity;
            }
        }

        /// <summary>
        /// Method that performs a total cost calculation for the items in the shopping cart.
        /// </summary>
        /// <returns>Object containing the items used for the total cost calculation, discounts applied and the total cost</returns>
        public ITotalCostCalculation CalculateTotal()
        {
            //get a copy of the items, original stays untouched
            IList<ITotalCostCalculationItem> calculationItems = new List<ITotalCostCalculationItem>(
                items.Select(x => new TotalCostCalculationItem(x))
            );

            //fetch active discounts and apply them if they are applicable (also remember applied discounts)
            var activeDiscounts = discountStorage.GetActiveDiscounts();
            var appliedDiscounts = new List<IDiscount>();
            foreach (var discount in activeDiscounts)
            {
                if (discount.IsDiscountApplicable(calculationItems))
                {
                    appliedDiscounts.Add(discount);
                    calculationItems = (discount.ApplyDiscount(calculationItems));
                }
            }

            //prepare the result object
            var calculation = new TotalCostCalculation
            {
                TotalCost = calculationItems.Sum(x => (x.TotalPrice)),
                CalculationItems = calculationItems,
                AppliedDiscounts = appliedDiscounts
            };

            //log the calculation and return
            logger.Log(calculation);
            return calculation;
        }
    }
}
