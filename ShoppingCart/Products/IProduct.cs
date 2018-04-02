namespace ShoppingCart.Products
{
    /// <summary>
    /// Interface that represents a product, it contains properties for the unit price and the product name
    /// </summary>
    public interface IProduct
    {
        decimal UnitPrice { get; set; }

        string Name { get; set; } 
    }
}
