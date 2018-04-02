namespace ShoppingCart
{
    /// <summary>
    /// Class that represents one item in the total cost calculation, 
    /// it contains the product name and unit price and also the quantity of the product, 
    /// calculated total price and calculated discount
    /// </summary>
    public class TotalCostCalculationItem : ITotalCostCalculationItem
    {
        public TotalCostCalculationItem(IShoppingCartItem originalItem)
        {
            ProductName = originalItem.Product.Name;
            ProductUnitPrice = originalItem.Product.UnitPrice;
            ProductQuantity = originalItem.Quantity;
        }

        public string ProductName { get; private set; }

        public decimal ProductUnitPrice { get; private set; }

        public int ProductQuantity { get; private set; }

        public decimal Discount { get; set; }

        public decimal TotalPrice
        {
            get
            {
                return ProductUnitPrice * ProductQuantity - Discount;
            }
        }

        
    }
}
