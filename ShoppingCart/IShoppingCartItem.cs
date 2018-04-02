using ShoppingCart.Products;

namespace ShoppingCart
{
    /// <summary>
    /// Interface representing a shopping cart item, it can store product name and unit price,
    /// quantity of the product bought, calculated discount and the total price.
    /// </summary>
    public interface IShoppingCartItem
    {
        IProduct Product { get; set; }

        int Quantity { get; set; }
    }
}
