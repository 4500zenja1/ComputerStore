using System.Collections.Generic;
using System.Linq;
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
    }
}
