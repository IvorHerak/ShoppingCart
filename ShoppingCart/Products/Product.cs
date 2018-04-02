namespace ShoppingCart.Products
{
    /// <summary>
    /// Class which represents a product.
    /// I am assuming the name of the product is its unique identifier for simplicity sake.
    /// </summary>
    public class Product : IProduct
    {
        /// <summary>
        /// Name of the product
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Unit price of the product
        /// </summary>
        public decimal UnitPrice { get; set; }
    }
}