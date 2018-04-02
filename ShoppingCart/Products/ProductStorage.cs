using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart.Products
{
    /// <summary>
    /// Simple product storage implementation, products are stored in a list.
    /// </summary>
    public class ProductStorage : IProductStorage
    {
        private IList<IProduct> products;

        /// <summary>
        /// Constructor - initializes a new instance of the ProductStorage class.
        /// </summary>
        /// <param name="products">List of products that the storage should contain</param>
        public ProductStorage(IList<IProduct> products)
        {
            this.products = products ?? throw new ArgumentException("Product list is null", "products");
        }

        /// <summary>
        /// Method which fetches a single product by name
        /// </summary>
        /// <param name="productName">Product name</param>
        /// <returns>Product with the given name or null if no such product exists</returns>
        public IProduct GetProductByNameOrDefault(string productName)
        {
            return products.FirstOrDefault(x => x.Name == productName);
        }
    }
}
