using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            // arrange
            Product product1 = new() { ProductId = 1, Name = "Продукт №1" };
            Product product2 = new() { ProductId = 2, Name = "Продукт №2" };
            Cart cart = new();

            // act
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);
            List<CartLine> result = cart.Lines.ToList();

            // assert
            Assert.AreEqual(result.Count(), 2);
            Assert.AreEqual(result[0].Product, product1);
            Assert.AreEqual(result[1].Product, product2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // arrange
            Product product1 = new() { ProductId = 1, Name = "Продукт №1" };
            Product product2 = new() { ProductId = 2, Name = "Продукт №2" };
            Cart cart = new();

            // act
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);
            cart.AddItem(product1, 5);
            List<CartLine> result = cart.Lines.OrderBy(x => x.Product.ProductId).ToList();

            // assert
            Assert.AreEqual(result.Count(), 2);
            Assert.AreEqual(result[0].Quantity, 6);
            Assert.AreEqual(result[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Lines()
        {
            // arrange
            Product product1 = new() { ProductId = 1, Name = "Продукт №1" };
            Product product2 = new() { ProductId = 2, Name = "Продукт №2" };
            Product product3 = new() { ProductId = 3, Name = "Продукт №3" };
            Cart cart = new();

            // act
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 4);
            cart.AddItem(product3, 2);
            cart.AddItem(product2, 1);
            cart.RemoveLine(product2);

            // assert
            Assert.AreEqual(cart.Lines.Where(x => x.Product == product2).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            // arrange
            Product product1 = new() { ProductId = 1, Name = "Продукт №1", Price = 100 };
            Product product2 = new() { ProductId = 2, Name = "Продукт №2", Price = 55 };
            Cart cart = new();

            // act
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);
            cart.AddItem(product1, 5);
            decimal result = cart.ComputeTotalValue();

            // assert
            Assert.AreEqual(result, 655);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            // arrange
            Product product1 = new() { ProductId = 1, Name = "Продукт №1", Price = 100 };
            Product product2 = new() { ProductId = 2, Name = "Продукт №2", Price = 55 };
            Cart cart = new();

            // act
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);
            cart.AddItem(product1, 5);
            cart.Clear();

            // assert
            Assert.AreEqual(cart.Lines.Count(), 0);
        }
    }
}
