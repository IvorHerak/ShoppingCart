namespace ShoppingCart.Products
{
    /// <summary>
    /// Interface representing a product storage
    /// </summary>
    public interface IProductStorage
    {
        /// <summary>
        /// Method which fetches a single product by name
        /// </summary>
        /// <param name="productName">Product name</param>
        /// <returns>Product with the given name or null if no such product exists</returns>
        IProduct GetProductByNameOrDefault(string productName);
    }
}
