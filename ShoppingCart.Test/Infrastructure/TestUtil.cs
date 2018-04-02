using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ShoppingCart.Test.Infrastructure
{
    internal class TestUtil
    {
        
        internal static void AssertEqualTotalCostCalculationItemLists(IList<ITotalCostCalculationItem> firstList, IList<ITotalCostCalculationItem> secondList)
        {
            Assert.AreEqual(firstList.Count, secondList.Count);
            for (var i = 0; i < firstList.Count; i++)
            {
                AssertEqualTotalCostCalculationItems(firstList[i], secondList[i]);
            }
        }

        internal static void AssertEqualTotalCostCalculationItems(ITotalCostCalculationItem firstItem, ITotalCostCalculationItem secondItem)
        {
            Assert.AreEqual(firstItem.ProductName, secondItem.ProductName);
            Assert.AreEqual(firstItem.ProductUnitPrice, secondItem.ProductUnitPrice);
            Assert.AreEqual(firstItem.ProductQuantity, secondItem.ProductQuantity);
            Assert.AreEqual(firstItem.Discount, secondItem.Discount);
        }

        internal static void AssertEqualShoppingCartItems(IShoppingCartItem firstItem, IShoppingCartItem secondItem)
        {
            Assert.AreEqual(firstItem.Quantity, secondItem.Quantity);
            Assert.AreEqual(firstItem.Product.Name, secondItem.Product.Name);
            Assert.AreEqual(firstItem.Product.UnitPrice, secondItem.Product.UnitPrice);
        }
    }
}
