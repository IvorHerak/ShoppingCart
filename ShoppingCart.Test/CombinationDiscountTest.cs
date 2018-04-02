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
    public class CombinationDiscountTest
    {
        [TestMethod]
        public void CombinationDiscountConstructorTest()
        {
            var testDiscount = new CombinationDiscount("Test", 3, "Test2", 0.5M);
            Assert.IsNotNull(testDiscount);

            try
            {
                testDiscount = new CombinationDiscount("Test", -33, "Test2", 0.5M);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ArgumentException));
            }

            try
            {
                testDiscount = new CombinationDiscount("Test", 0, "Test2", 0.5M);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ArgumentException));
            }

            try
            {
                testDiscount = new CombinationDiscount("Test", 5, "Test2", 0M);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ArgumentException));
            }

            try
            {
                testDiscount = new CombinationDiscount("Test", 5, "Test2", -0.5M);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ArgumentException));
            }
        }

        [TestMethod]
        public void CombinationDiscountApplicableTest()
        {
            var testDiscount = new CombinationDiscount("Test2", 3, "Test1", 0.5M);

            var testItems = new List<ITotalCostCalculationItem>();
            var testItem1 = new TotalCostCalculationItem(new ShoppingCartItem { Product = new Product { Name = "Test1", UnitPrice = 5 }, Quantity = 4 });
       
            testItems.Add(testItem1);
            Assert.IsFalse(testDiscount.IsDiscountApplicable(testItems));

            var testItem2 = new TotalCostCalculationItem(new ShoppingCartItem { Product = new Product { Name = "Test2", UnitPrice = 1 }, Quantity = 2 });

            testItems.Add(testItem2);
            Assert.IsFalse(testDiscount.IsDiscountApplicable(testItems));

            testItems.Remove(testItem2);
            testItem2 = new TotalCostCalculationItem(new ShoppingCartItem { Product = new Product { Name = "Test2", UnitPrice = 1 }, Quantity = 5 });

            testItems.Add(testItem2);
            Assert.IsTrue(testDiscount.IsDiscountApplicable(testItems));
        }

        [TestMethod]
        public void CombinationDiscountApplicationTestNonApplication()
        {
            var testDiscount = new CombinationDiscount("Test2", 3, "Test1", 0.5M);
            var testItems = new List<ITotalCostCalculationItem>();
            var testItem1 = new TotalCostCalculationItem(new ShoppingCartItem { Product = new Product { Name = "Test1", UnitPrice = 5 }, Quantity = 4 });

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
        public void CombinationDiscountApplicationTestBasicApplication()
        {
            var testDiscount = new CombinationDiscount("Test2", 3, "Test1", 0.5M);
            var testItems = new List<ITotalCostCalculationItem>();
            var testItem1 = new TotalCostCalculationItem(new ShoppingCartItem { Product = new Product { Name = "Test1", UnitPrice = 5 }, Quantity = 4 });

            var testItem2 = new TotalCostCalculationItem(new ShoppingCartItem { Product = new Product { Name = "Test2", UnitPrice = 3 }, Quantity = 4 });

            testItems.Add(testItem1);
            testItems.Add(testItem2);
            var discountedItems = testDiscount.ApplyDiscount(testItems);

            var discountedTest2 = discountedItems.FirstOrDefault(x => x.ProductName == "Test2");
            Assert.IsNotNull(discountedTest2);
            TestUtil.AssertEqualTotalCostCalculationItems(testItem2, discountedTest2);

            var discountedTest1 = discountedItems.FirstOrDefault(x => x.ProductName == "Test1");
            Assert.IsNotNull(discountedTest1);
            Assert.AreEqual(testItem1.ProductName, discountedTest1.ProductName);
            Assert.AreEqual(testItem1.ProductUnitPrice, discountedTest1.ProductUnitPrice);
            Assert.AreEqual(testItem1.ProductQuantity, discountedTest1.ProductQuantity);

            //50% off for one Test1
            Assert.AreEqual(2.5M, discountedTest1.Discount);
        }

        [TestMethod]
        public void CombinationDiscountApplicationTestMultipleNotEnoughQuantity()
        {
            var testDiscount = new CombinationDiscount("Test2", 3, "Test1", 0.5M);
            var testItems = new List<ITotalCostCalculationItem>();
            var testItem1 = new TotalCostCalculationItem(new ShoppingCartItem { Product = new Product { Name = "Test1", UnitPrice = 5 }, Quantity = 2 });

            var testItem2 = new TotalCostCalculationItem(new ShoppingCartItem { Product = new Product { Name = "Test2", UnitPrice = 3 }, Quantity = 12 });

            testItems.Add(testItem1);
            testItems.Add(testItem2);
            var discountedItems = testDiscount.ApplyDiscount(testItems);

            var discountedTest2 = discountedItems.FirstOrDefault(x => x.ProductName == "Test2");
            Assert.IsNotNull(discountedTest2);
            TestUtil.AssertEqualTotalCostCalculationItems(testItem2, discountedTest2);

            var discountedTest1 = discountedItems.FirstOrDefault(x => x.ProductName == "Test1");
            Assert.IsNotNull(discountedTest1);
            Assert.AreEqual(testItem1.ProductName, discountedTest1.ProductName);
            Assert.AreEqual(testItem1.ProductUnitPrice, discountedTest1.ProductUnitPrice);
            Assert.AreEqual(testItem1.ProductQuantity, discountedTest1.ProductQuantity);

            // the threshold is met 4 times, but we only have 2 items to discount
            // so 2*(Test1UnitPrice)*0.5
            Assert.AreEqual(5M, discountedTest1.Discount);
        }

        [TestMethod]
        public void CombinationDiscountApplicationTestMultipleEnoughQuantity()
        {
            var testDiscount = new CombinationDiscount("Test2", 3, "Test1", 0.5M);
            var testItems = new List<ITotalCostCalculationItem>();
            var testItem1 = new TotalCostCalculationItem(new ShoppingCartItem { Product = new Product { Name = "Test1", UnitPrice = 5 }, Quantity = 8 });

            var testItem2 = new TotalCostCalculationItem(new ShoppingCartItem { Product = new Product { Name = "Test2", UnitPrice = 3 }, Quantity = 12 });

            testItems.Add(testItem1);
            testItems.Add(testItem2);
            var discountedItems = testDiscount.ApplyDiscount(testItems);

            var discountedTest2 = discountedItems.FirstOrDefault(x => x.ProductName == "Test2");
            Assert.IsNotNull(discountedTest2);
            TestUtil.AssertEqualTotalCostCalculationItems(testItem2, discountedTest2);

            var discountedTest1 = discountedItems.FirstOrDefault(x => x.ProductName == "Test1");
            Assert.IsNotNull(discountedTest1);
            Assert.AreEqual(testItem1.ProductName, discountedTest1.ProductName);
            Assert.AreEqual(testItem1.ProductUnitPrice, discountedTest1.ProductUnitPrice);
            Assert.AreEqual(testItem1.ProductQuantity, discountedTest1.ProductQuantity);

            // the threshold is met 4 times and we have 8 items of which we can discount 4
            // so 4*(Test1UnitPrice)*0.5
            Assert.AreEqual(10M, discountedTest1.Discount);
        }
    }
}
