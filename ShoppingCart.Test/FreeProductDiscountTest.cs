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
    public class FreeProductDiscountTest
    {
        [TestMethod]
        public void FreeProductDiscountConstructorTest()
        {
            var testDiscount = new FreeProductDiscount("Test", 3);
            Assert.IsNotNull(testDiscount);

            try
            {
                testDiscount = new FreeProductDiscount("Test", -2);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ArgumentException));
            }

            try
            {
                testDiscount = new FreeProductDiscount("Test", 0);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ArgumentException));
            }
        }

        [TestMethod]
        public void FreeProductDiscountApplicableTest()
        {
            var testDiscount = new FreeProductDiscount("Test2", 3);

            var testItems = new List<ITotalCostCalculationItem>();
            var testItem1 = new TotalCostCalculationItem(new ShoppingCartItem { Product = new Product { Name = "Test1", UnitPrice = 1 }, Quantity = 4 });

            testItems.Add(testItem1);
            Assert.IsFalse(testDiscount.IsDiscountApplicable(testItems));

            var testItem2 = new TotalCostCalculationItem(new ShoppingCartItem { Product = new Product { Name = "Test2", UnitPrice = 1 }, Quantity = 2 });

            testItems.Add(testItem2);
            Assert.IsFalse(testDiscount.IsDiscountApplicable(testItems));

            testItems.Clear();

            var testItem3 = new TotalCostCalculationItem(new ShoppingCartItem { Product = new Product { Name = "Test2", UnitPrice = 5 }, Quantity = 5 });

            testItems.Add(testItem1);
            testItems.Add(testItem3);

            Assert.IsTrue(testDiscount.IsDiscountApplicable(testItems));
        }

        [TestMethod]
        public void FreeProductDiscountApplicationTestNonApplication()
        {
            var testDiscount = new FreeProductDiscount("Test2", 3);
            var testItems = new List<ITotalCostCalculationItem>();
            var testItem1 = new TotalCostCalculationItem(new ShoppingCartItem { Product = new Product { Name = "Test1", UnitPrice = 1 }, Quantity = 4 });

            testItems.Add(testItem1);
            var discountedItems = testDiscount.ApplyDiscount(testItems);

            Assert.IsNotNull(discountedItems);
            TestUtil.AssertEqualTotalCostCalculationItemLists(testItems, discountedItems);
            var testItem2 = new TotalCostCalculationItem(new ShoppingCartItem { Product = new Product { Name = "Test2", UnitPrice = 1 }, Quantity = 2 });

            testItems.Add(testItem2);
            discountedItems = testDiscount.ApplyDiscount(testItems);
            TestUtil.AssertEqualTotalCostCalculationItemLists(testItems, discountedItems);
        }

        [TestMethod]
        public void FreeProductDiscountApplicationTestBasicApplication()
        {
            var testDiscount = new FreeProductDiscount("Test2", 3);
            var testItems = new List<ITotalCostCalculationItem>();
            var testItem1 = new TotalCostCalculationItem(new ShoppingCartItem { Product = new Product { Name = "Test1", UnitPrice = 1 }, Quantity = 4 });
            var testItem2 = new TotalCostCalculationItem(new ShoppingCartItem { Product = new Product { Name = "Test2", UnitPrice = 3 }, Quantity = 4 });

            testItems.Add(testItem1);
            testItems.Add(testItem2);
            var discountedItems = testDiscount.ApplyDiscount(testItems);

            var discountedTest1 = discountedItems.FirstOrDefault(x => x.ProductName == "Test1");
            Assert.IsNotNull(discountedTest1);
            TestUtil.AssertEqualTotalCostCalculationItems(testItem1, discountedTest1);

            var discountedTest2 = discountedItems.FirstOrDefault(x => x.ProductName == "Test2");
            Assert.IsNotNull(discountedTest2);
            Assert.AreEqual(testItem2.ProductName, discountedTest2.ProductName);
            Assert.AreEqual(testItem2.ProductUnitPrice, discountedTest2.ProductUnitPrice);
            Assert.AreEqual(testItem2.ProductQuantity, discountedTest2.ProductQuantity);
            //one Test2 should be free, so the discount is one Test2 UnitPrice
            Assert.AreEqual(3M, discountedTest2.Discount);
        }

        [TestMethod]
        public void FreeProductDiscountApplicationTestMultipleApplication()
        {
            var testDiscount = new FreeProductDiscount("Test2", 3);
            var testItems = new List<ITotalCostCalculationItem>();
            var testItem1 = new TotalCostCalculationItem(new ShoppingCartItem { Product = new Product { Name = "Test1", UnitPrice = 1 }, Quantity = 4 });
            var testItem2 = new TotalCostCalculationItem(new ShoppingCartItem { Product = new Product { Name = "Test2", UnitPrice = 2 }, Quantity = 12 });

            testItems.Add(testItem1);
            testItems.Add(testItem2);
            var discountedItems = testDiscount.ApplyDiscount(testItems);

            var discountedTest1 = discountedItems.FirstOrDefault(x => x.ProductName == "Test1");
            Assert.IsNotNull(discountedTest1);
            TestUtil.AssertEqualTotalCostCalculationItems(testItem1, discountedTest1);

            var discountedTest2 = discountedItems.FirstOrDefault(x => x.ProductName == "Test2");
            Assert.IsNotNull(discountedTest2);
            Assert.AreEqual(testItem2.ProductName, discountedTest2.ProductName);
            Assert.AreEqual(testItem2.ProductUnitPrice, discountedTest2.ProductUnitPrice);
            Assert.AreEqual(testItem2.ProductQuantity, discountedTest2.ProductQuantity);
            //four Test2 should be free, so the discount is four times Test2 UnitPrice
            Assert.AreEqual(8M, discountedTest2.Discount);
        }
    }
}
