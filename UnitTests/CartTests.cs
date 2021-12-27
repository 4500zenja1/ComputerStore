using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebUI.Controllers;
using WebUI.Models;

namespace UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            // arrange
            Product product1 = new() { ProductId = 1, Name = "Товар №1" };
            Product product2 = new() { ProductId = 2, Name = "Товар №2" };
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
            Product product1 = new() { ProductId = 1, Name = "Товар №1" };
            Product product2 = new() { ProductId = 2, Name = "Товар №2" };
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
            Product product1 = new() { ProductId = 1, Name = "Товар №1" };
            Product product2 = new() { ProductId = 2, Name = "Товар №2" };
            Product product3 = new() { ProductId = 3, Name = "Товар №3" };
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
            Product product1 = new() { ProductId = 1, Name = "Товар №1", Price = 100 };
            Product product2 = new() { ProductId = 2, Name = "Товар №2", Price = 55 };
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
            Product product1 = new() { ProductId = 1, Name = "Товар №1", Price = 100 };
            Product product2 = new() { ProductId = 2, Name = "Товар №2", Price = 55 };
            Cart cart = new();

            // act
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);
            cart.AddItem(product1, 5);
            cart.Clear();

            // assert
            Assert.AreEqual(cart.Lines.Count(), 0);
        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            // arrange
            Mock<IProductRepository> mock = new();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product {ProductId = 1, Name = "Товар №1", Category = "Cat1"}
            }.AsQueryable());
            Cart cart = new();
            CartController controller = new(mock.Object, null);

            // act
            controller.AddToCart(cart, 1);

            // assert
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToList()[0].Product.ProductId, 1);
        }

        [TestMethod]
        public void Adding_Game_To_Cart_Goes_To_Cart_Screen()
        {
            // arrange
            Mock<IProductRepository> mock = new();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product {ProductId = 1, Name = "Товар №1", Category = "Cat1"}
            }.AsQueryable());
            Cart cart = new();
            CartController controller = new(mock.Object, null);

            // act
            RedirectToRouteResult result = controller.AddToCart(cart, 2);

            // assert
            Assert.AreEqual(result.RouteValues["action"], "List");
        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            // arrange
            Cart cart = new();
            CartController target = new(null, null);

            // act
            CartIndexViewModel result = (CartIndexViewModel)target.Index(cart).ViewData.Model;

            // assert
            Assert.AreSame(result.Cart, cart);
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            // arrange
            Mock<IOrderProcessor> mock = new();
            Cart cart = new();
            ShippingDetails shippingDetails = new();
            CartController controller = new(null, mock.Object);

            // act
            ViewResult result = controller.Checkout(cart, shippingDetails);

            // assert
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_Shipping_Details()
        {
            // arrange
            Mock<IOrderProcessor> mock = new();
            Cart cart = new();
            cart.AddItem(new Product(), 1);
            CartController controller = new(null, mock.Object);
            controller.ModelState.AddModelError("error", "error");

            // act
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // assert
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            // arrange
            Mock<IOrderProcessor> mock = new();
            Cart cart = new();
            cart.AddItem(new Product(), 1);
            CartController controller = new(null, mock.Object);

            // act
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // assert
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Once());
            Assert.AreEqual("Completed", result.ViewName);
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
}
