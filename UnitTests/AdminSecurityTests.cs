using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebUI.Controllers;
using WebUI.Infrastructure.Abstract;
using WebUI.Models;

namespace UnitTests
{
    [TestClass]
    public class AdminSecurityTests
    {
        [TestMethod]
        public void Can_Login_With_Valid_Credentials()
        {
            // arrange
            Mock<IAuthProvider> mock = new();
            mock.Setup(m => m.Authenticate("admin", "12345")).Returns(true);
            LoginViewModel model = new()
            {
                UserName = "admin",
                Password = "12345"
            };
            AccountController target = new(mock.Object);

            // act
            ActionResult result = target.Login(model, "/MyURL");

            // assert
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual("/MyURL", ((RedirectResult)result).Url);
        }

        [TestMethod]
        public void Cannot_Login_With_Invalid_Credentials()
        {
            // arrange
            Mock<IAuthProvider> mock = new();
            mock.Setup(m => m.Authenticate("badUser", "badPass")).Returns(false);
            LoginViewModel model = new()
            {
                UserName = "badUser",
                Password = "badPass"
            };
            AccountController target = new(mock.Object);

            // act
            ActionResult result = target.Login(model, "/MyURL");

            // assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }
    }
}
