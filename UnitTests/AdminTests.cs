using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebUI.Controllers;

namespace UnitTests
{

    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Products()
        {
            // arrange
            Mock<IProductRepository> mock = new();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product {ProductId = 1, Name = "Товар №1"},
                new Product {ProductId = 2, Name = "Товар №2"},
                new Product {ProductId = 3, Name = "Товар №3"},
                new Product {ProductId = 4, Name = "Товар №4"},
                new Product {ProductId = 5, Name = "Товар №5"},
            });
            AdminController controller = new(mock.Object);

            // act
            List<Product> result = ((IEnumerable<Product>)controller.Index().ViewData.Model).ToList();

            // assert
            Assert.AreEqual(result.Count(), 5);
            Assert.AreEqual(result[3].Name, "Товар №4");
            Assert.AreEqual(result[1].Name, "Товар №2");
            Assert.AreEqual(result[4].Name, "Товар №5");
        }

        [TestMethod]
        public void Can_Edit_Product()
        {
            // arrange
            Mock<IProductRepository> mock = new();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product {ProductId = 1, Name = "Товар №1"},
                new Product {ProductId = 2, Name = "Товар №2"},
                new Product {ProductId = 3, Name = "Товар №3"},
                new Product {ProductId = 4, Name = "Товар №4"},
                new Product {ProductId = 5, Name = "Товар №5"},
            });
            AdminController controller = new(mock.Object);

            // act
            Product product1 = controller.Edit(1).ViewData.Model as Product;
            Product product2 = controller.Edit(2).ViewData.Model as Product;
            Product product3 = controller.Edit(3).ViewData.Model as Product;

            // assert
            Assert.AreEqual(product1.ProductId, 1);
            Assert.AreEqual(product2.ProductId, 2);
            Assert.AreEqual(product3.ProductId, 3);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Product()
        {
            // arrange
            Mock<IProductRepository> mock = new();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product {ProductId = 1, Name = "Товар №1"},
                new Product {ProductId = 2, Name = "Товар №2"},
                new Product {ProductId = 3, Name = "Товар №3"},
                new Product {ProductId = 4, Name = "Товар №4"},
                new Product {ProductId = 5, Name = "Товар №5"},
            });
            AdminController controller = new(mock.Object);

            // act
            Product product = controller.Edit(6).ViewData.Model as Product;

            // assert
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            // arrange
            Mock<IProductRepository> mock = new();
            AdminController controller = new(mock.Object);
            Product product = new() { Name = "Test"};

            // act
            ActionResult result = controller.Edit(product);

            // assert
            mock.Verify(m => m.SaveProduct(product));
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            // arrange
            Mock<IProductRepository> mock = new();
            AdminController controller = new(mock.Object);
            Product product = new() { Name = "Test" };
            controller.ModelState.AddModelError("error", "error");

            // act
            ActionResult result = controller.Edit(product);

            // assert
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
