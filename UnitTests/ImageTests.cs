using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebUI.Controllers;

namespace UnitTests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            // arrange
            Product product = new()
            {
                ProductId = 2,
                Name = "Товар №2",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };
            Mock<IProductRepository> mock = new();
            mock.Setup(m => m.Products).Returns(new List<Product> {
                new Product {ProductId = 1, Name = "Товар №1"},
                product,
                new Product {ProductId = 3, Name = "Товар №3"}
            }.AsQueryable());
            ProductController controller = new(mock.Object);

            // act
            ActionResult result = controller.GetImage(2);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(product.ImageMimeType, ((FileResult)result).ContentType);
        }

        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            // arrange
            Mock<IProductRepository> mock = new();
            mock.Setup(m => m.Products).Returns(new List<Product> {
                new Product {ProductId = 1, Name = "Товар №1"},
                new Product {ProductId = 2, Name = "Товар №2"}
            }.AsQueryable());
            ProductController controller = new(mock.Object);

            // act
            ActionResult result = controller.GetImage(10);

            // assert
            Assert.IsNull(result);
        }
    }
}
