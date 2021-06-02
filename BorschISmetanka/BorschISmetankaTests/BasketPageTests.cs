using Microsoft.VisualStudio.TestTools.UnitTesting;
using BorschISmetanka.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BorschISmetanka.Views.Tests
{
    [TestClass]
    public class BasketPageTests
    {
        [TestMethod()]
        [DataRow(0,0,0)]
        [DataRow(0, 80, 0)]
        [DataRow(1, 0, 0)]
        [DataRow(1, 80, 80)]
        public void CalculateDishPrice_MultipleDishes_ReturnPriceOfDishes(int count, int price, int expected)
        {
            //arrange
            
            //actual
            int actual = BasketPage.CalculateDishPrice(count, price);

            //assert
            Assert.AreEqual(expected, actual);
        }
    }
}