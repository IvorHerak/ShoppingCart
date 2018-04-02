using ShoppingCart.Products;

namespace ShoppingCart
{
    /// <summary>
    /// Data structure representing a shopping cart item.
    /// Contains product, quantity and discount data.
    /// </summary>
    public class ShoppingCartItem : IShoppingCartItem
    {
        public IProduct Product { get; set; }

        public int Quantity { get; set; }

        public ShoppingCartItem Copy()
        {
            return new ShoppingCartItem
            {
                Product = new Product
                {

                    Name = Product.Name,
                    UnitPrice = Product.UnitPrice
                },
                Quantity = Quantity
            };
        }
    }
}