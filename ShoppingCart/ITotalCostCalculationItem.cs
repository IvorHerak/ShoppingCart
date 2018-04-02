namespace ShoppingCart
{
    /// <summary>
    /// Interface representing one item in the total cost calculation,
    /// it contains the product name and unit price and also the quantity of the product, 
    /// calculated total price and calculated discount
    /// </summary>
    public interface ITotalCostCalculationItem
    {
        string ProductName { get; }

        decimal ProductUnitPrice { get; }

        int ProductQuantity { get; }

        decimal Discount { get; set; }

        decimal TotalPrice { get; }
    }
}
