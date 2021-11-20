using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebUI.Controllers;
using WebUI.HtmlHelpers;
using WebUI.Models;

namespace UnitTests
{
    [TestClass]
    public class AppTests
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // arrange
            Mock<IProductRepository> mock = new();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product {ProductId = 1, Name = "Продукт №1"},
                new Product {ProductId = 2, Name = "Продукт №2"},
                new Product {ProductId = 3, Name = "Продукт №3"},
                new Product {ProductId = 4, Name = "Продукт №4"},
                new Product {ProductId = 5, Name = "Продукт №5"}
            });
            ProductController controller = new(mock.Object)
            {
                pageSize = 3
            };

            // act
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

            // assert
            List<Product> products = result.Products.ToList();
            Assert.IsTrue(products.Count == 2);
            Assert.AreEqual(products[0].Name, "Продукт №4");
            Assert.AreEqual(products[1].Name, "Продукт №5");

        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            // arrange
            HtmlHelper myHelper = null;
            PagingInfo pagingInfo = new()
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            static string pageUrlDelegate(int i) => "Page" + i;

            // act
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // assert
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // arrange
            Mock<IProductRepository> mock = new();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product {ProductId = 1, Name = "Продукт №1"},
                new Product {ProductId = 2, Name = "Продукт №2"},
                new Product {ProductId = 3, Name = "Продукт №3"},
                new Product {ProductId = 4, Name = "Продукт №4"},
                new Product {ProductId = 5, Name = "Продукт №5"}
            });
            ProductController controller = new(mock.Object)
            {
                pageSize = 3
            };

            // act
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

            // assert
            // проверка на верные параметры pageInfo
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Games()
        {
            // arrange
            Mock<IProductRepository> mock = new();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product {ProductId = 1, Name = "Продукт №1", Category = "Cat1"},
                new Product {ProductId = 2, Name = "Продукт №2", Category = "Cat2"},
                new Product {ProductId = 3, Name = "Продукт №3", Category = "Cat1"},
                new Product {ProductId = 4, Name = "Продукт №4", Category = "Cat2"},
                new Product {ProductId = 5, Name = "Продукт №5", Category = "Cat3"}
            });
            ProductController controller = new(mock.Object)
            {
                pageSize = 3
            };

            // act
            List<Product> result = ((ProductsListViewModel)controller.List("Cat2", 1).Model)
                .Products.ToList();

            // assert
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Продукт №2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "Продукт №4" && result[1].Category == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            // arrange
            Mock<IProductRepository> mock = new();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product {ProductId = 1, Name = "Продукт №1", Category = "Ноутбук"},
                new Product {ProductId = 2, Name = "Продукт №2", Category = "Видеокарта"},
                new Product {ProductId = 3, Name = "Продукт №3", Category = "Ноутбук"},
                new Product {ProductId = 4, Name = "Продукт №4", Category = "USB-хаб"}
            });
            NavController target = new(mock.Object);

            // act
            List<string> results = ((IEnumerable<string>)target.Menu().Model).ToList();

            // assert
            Assert.AreEqual(results.Count(), 3);
            Assert.AreEqual(results[0], "USB-хаб");
            Assert.AreEqual(results[1], "Видеокарта");
            Assert.AreEqual(results[2], "Ноутбук");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            // arrange
            Mock<IProductRepository> mock = new();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product {ProductId = 1, Name = "Продукт №1", Category = "ОЗУ"},
                new Product {ProductId = 2, Name = "Продукт №2", Category = "Процессоры"}
            });
            NavController target = new(mock.Object);
            string categoryToSelect = "Процессоры";

            // act
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            // assert
            Assert.AreEqual(categoryToSelect, result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Game_Count()
        {
            // arrange
            Mock<IProductRepository> mock = new();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product {ProductId = 1, Name = "Продукт №1", Category = "Cat1"},
                new Product {ProductId = 2, Name = "Продукт №2", Category = "Cat2"},
                new Product {ProductId = 3, Name = "Продукт №3", Category = "Cat1"},
                new Product {ProductId = 4, Name = "Продукт №4", Category = "Cat2"},
                new Product {ProductId = 5, Name = "Продукт №5", Category = "Cat3"}
            });
            ProductController controller = new(mock.Object)
            {
                pageSize = 3
            };

            // act
            int res1 = ((ProductsListViewModel)controller.List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((ProductsListViewModel)controller.List("Cat2").Model).PagingInfo.TotalItems;
            int res3 = ((ProductsListViewModel)controller.List("Cat3").Model).PagingInfo.TotalItems;
            int resAll = ((ProductsListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            // assert
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    }
}
