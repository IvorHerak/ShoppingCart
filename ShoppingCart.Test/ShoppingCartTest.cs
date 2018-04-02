using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingCart.Discounts;
using ShoppingCart.Products;
using ShoppingCart.Test.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ShoppingCart.Test
{
    [TestClass]
    public class ShoppingCartTest
    {
        [TestMethod]
        public void ShoppingCartConstructorTest()
        {
            ShoppingCart testShoppingCart = null;

            try
            {
                testShoppingCart = new ShoppingCart(null, null);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ArgumentException));
            }

            var discountStorage = new DiscountStorage(new List<IDiscount>());
            try
            {
                testShoppingCart = new ShoppingCart(null, discountStorage);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ArgumentException));
            }

            var productStorage = new ProductStorage(new List<IProduct>());
            try
            {
                testShoppingCart = new ShoppingCart(productStorage, null);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ArgumentException));
            }

            testShoppingCart = new ShoppingCart(productStorage, discountStorage);
            Assert.IsNotNull(testShoppingCart);
        }

        [TestMethod]
        public void ShoppingCartItemsGetterTest()
        {
            var products = new List<IProduct>();
            products.Add(new Product { Name = "Test", UnitPrice = 1 });
            var productStorage = new ProductStorage(products);
            var discountStorage = new DiscountStorage(new List<IDiscount>());

            var shoppingCart = new ShoppingCart(productStorage, discountStorage);
            var testItems = shoppingCart.GetShoppingCartItems();
            Assert.IsNotNull(testItems);
            Assert.AreEqual(0, testItems.Count);

            shoppingCart.AddProduct("Test", 1);
            testItems = shoppingCart.GetShoppingCartItems();
            Assert.IsNotNull(testItems);
            Assert.AreEqual(1, testItems.Count);

            // add neew item and change existing item
            testItems.Add(new ShoppingCartItem { Product = new Product { Name = "Test2", UnitPrice = 2 }, Quantity = 1 });
            var testItem = testItems.FirstOrDefault(x => x.Product.Name == "Test");
            Assert.IsNotNull(testItem);
            testItem.Quantity = 500;

            // check that the items in the shopping cart are intact
            var newTestItems = shoppingCart.GetShoppingCartItems();
            Assert.IsNotNull(newTestItems);
            Assert.AreEqual(1, newTestItems.Count);
            var newTestItem = newTestItems.FirstOrDefault(x => x.Product.Name == "Test");
            Assert.IsNotNull(newTestItem);
            Assert.AreEqual(1, newTestItem.Quantity);
        }

        [TestMethod]
        public void ShoppingCartAddProductValidationTest()
        {
            var products = new List<IProduct>();
            products.Add(new Product { Name = "Test", UnitPrice = 1 });
            products.Add(new Product { Name = "Test2", UnitPrice = 2 });


            var productStorage = new ProductStorage(products);
            var discountStorage = new DiscountStorage(new List<IDiscount>());

            var shoppingCart = new ShoppingCart(productStorage, discountStorage);

            //add unknown product
            try
            {
                shoppingCart.AddProduct("Test3", 1);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ArgumentException));
            }

            //add zero quantity
            try
            {
                shoppingCart.AddProduct("Test", 0);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ArgumentException));
            }

            //add negative quantity
            try
            {
                shoppingCart.AddProduct("Test2", -3);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ArgumentException));
            }
        }

        [TestMethod]
        public void ShoppingCartAddProductTest()
        {
            var products = new List<IProduct>();
            products.Add(new Product { Name = "Test", UnitPrice = 1 });
            products.Add(new Product { Name = "Test2", UnitPrice = 2 });


            var productStorage = new ProductStorage(products);
            var discountStorage = new DiscountStorage(new List<IDiscount>());

            //checking that by default the cart is empty
            var shoppingCart = new ShoppingCart(productStorage, discountStorage);
            var testItems = shoppingCart.GetShoppingCartItems();
            Assert.IsNotNull(testItems);
            Assert.AreEqual(0, testItems.Count);

            //basic add functionality test
            shoppingCart.AddProduct("Test", 1);
            shoppingCart.AddProduct("Test2", 1);

            testItems = shoppingCart.GetShoppingCartItems();
            Assert.IsNotNull(testItems);
            var testItem = testItems.FirstOrDefault(x => x.Product.Name == "Test");
            TestUtil.AssertEqualShoppingCartItems(testItem, new ShoppingCartItem { Product = new Product { Name = "Test", UnitPrice = 1 }, Quantity = 1 });
            var testItem2 = testItems.FirstOrDefault(x => x.Product.Name == "Test2");
            TestUtil.AssertEqualShoppingCartItems(testItem2, new ShoppingCartItem { Product = new Product { Name = "Test2", UnitPrice = 2 }, Quantity = 1 });

            //items grouping test
            shoppingCart.AddProduct("Test", 5);
            testItems = shoppingCart.GetShoppingCartItems();
            testItem = testItems.FirstOrDefault(x => x.Product.Name == "Test");
            TestUtil.AssertEqualShoppingCartItems(testItem, new ShoppingCartItem { Product = new Product { Name = "Test", UnitPrice = 1 }, Quantity = 6 });
            testItem2 = testItems.FirstOrDefault(x => x.Product.Name == "Test2");
            TestUtil.AssertEqualShoppingCartItems(testItem2, new ShoppingCartItem { Product = new Product { Name = "Test2", UnitPrice = 2 }, Quantity = 1 });
        }

        [TestMethod]
        public void ShoppingCartCalculateTotalTest()
        {
            var products = new List<IProduct>();
            products.Add(new Product { Name = "Test", UnitPrice = 1 });
            products.Add(new Product { Name = "Test2", UnitPrice = 2 });
            products.Add(new Product { Name = "Test3", UnitPrice = 3 });

            var productStorage = new ProductStorage(products);

            var discounts = new List<IDiscount>();
            discounts.Add(new FreeProductDiscount("Test", 3));
            discounts.Add(new CombinationDiscount("Test2", 3, "Test3", 0.5M));

            var discountStorage = new DiscountStorage(discounts);

            var testLogger = new TestShoppingCartLogger();

            var shoppingCartTest = new ShoppingCart(productStorage, discountStorage, testLogger);

            //trigger the FreeProductDiscount
            shoppingCartTest.AddProduct("Test", 4);

            //satisfy the trigger for the CombinationDiscount - but not enable usage
            shoppingCartTest.AddProduct("Test2", 6);

            var calculation = shoppingCartTest.CalculateTotal();

            //we check if the calculation was sent to the logger
            Assert.IsNotNull(testLogger.LastCalculation);

            //we check the total 
            Assert.AreEqual(15M, calculation.TotalCost);

            //check the applied discounts
            Assert.IsNotNull(calculation.AppliedDiscounts);
            Assert.AreEqual(1, calculation.AppliedDiscounts.Count);
            Assert.IsInstanceOfType(calculation.AppliedDiscounts[0], typeof(FreeProductDiscount));

            //check the calculation items
            Assert.IsNotNull(calculation.CalculationItems);
            Assert.AreEqual(2, calculation.CalculationItems.Count);
            var testItem = calculation.CalculationItems.FirstOrDefault(x => x.ProductName == "Test");
            Assert.IsNotNull(testItem);
            Assert.AreEqual(1M, testItem.ProductUnitPrice);
            Assert.AreEqual(4, testItem.ProductQuantity);
            Assert.AreEqual(1M, testItem.Discount);

            var testItem2 = calculation.CalculationItems.FirstOrDefault(x => x.ProductName == "Test2");
            Assert.IsNotNull(testItem2);
            Assert.AreEqual(2M, testItem2.ProductUnitPrice);
            Assert.AreEqual(6, testItem2.ProductQuantity);
            Assert.AreEqual(0M, testItem2.Discount);
        }
    }
}
