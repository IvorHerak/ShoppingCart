using ShoppingCart.Discounts;
using ShoppingCart.Products;
using System;
using System.Collections.Generic;

namespace ShoppingCart.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            // example run of the Shopping Cart using the exact data from the task
            var products = new List<IProduct>()
            {
                new Product { Name = "butter", UnitPrice = 0.80M },
                new Product { Name = "milk", UnitPrice = 1.15M },
                new Product { Name = "bread", UnitPrice = 1M}
            };

            var discounts = new List<IDiscount>()
            {
                new CombinationDiscount("butter", 2, "bread", 0.50M),
                new FreeProductDiscount("milk", 4)
            };

            var shoppingCart = new ShoppingCart(new ProductStorage(products), new DiscountStorage(discounts));

            Console.WriteLine();
            Console.WriteLine("Scenario 1:");
            shoppingCart.AddProduct("bread", 1);
            shoppingCart.AddProduct("butter", 1);
            shoppingCart.AddProduct("milk", 1);
            shoppingCart.CalculateTotal();

            shoppingCart = new ShoppingCart(new ProductStorage(products), new DiscountStorage(discounts));

            Console.WriteLine();
            Console.WriteLine("Scenario 2:");
            shoppingCart.AddProduct("bread", 2);
            shoppingCart.AddProduct("butter", 2);
            shoppingCart.CalculateTotal();

            shoppingCart = new ShoppingCart(new ProductStorage(products), new DiscountStorage(discounts));

            Console.WriteLine();
            Console.WriteLine("Scenario 3:");
            shoppingCart.AddProduct("milk", 4);
            shoppingCart.CalculateTotal();

            shoppingCart = new ShoppingCart(new ProductStorage(products), new DiscountStorage(discounts));

            Console.WriteLine();
            Console.WriteLine("Scenario 4:");
            shoppingCart.AddProduct("bread", 1);
            shoppingCart.AddProduct("butter", 2);
            shoppingCart.AddProduct("milk", 8);
            shoppingCart.CalculateTotal();

            Console.ReadLine();
        }
    }
}
