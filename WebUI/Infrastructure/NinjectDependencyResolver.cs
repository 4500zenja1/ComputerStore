using Domain.Abstract;
using Domain.Concrete;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using WebUI.Infrastructure.Abstract;
using WebUI.Infrastructure.Concrete;

namespace WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        public void AddBindings()
        {
            /* 
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product {Name = "Ноутбук HP350", Price = 1097.00M},
                new Product {Name = "Мышь компьютерная Logitech M90", Price = 12.97M},
                new Product {Name = "Процессор Intel Core i7", Price = 302.63M},
                new Product {Name = "Видеокарта Gigabyte GeForce RTX3060", Price = 3299.00M},
                new Product {Name = "Оперативная память DDR4 HyperX", Price = 191.10M}
            });
            kernel.Bind<IProductRepository>().ToConstant(mock.Object);
            */
            kernel.Bind<IProductRepository>().To<EFProductRepository>();

            EmailSettings emailSettings = new()
            {
                WriteAsFile = bool.Parse(ConfigurationManager
                .AppSettings["Email.WriteAsFile"] ?? "false")
            };

            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>()
                .WithConstructorArgument("settings", emailSettings);
            kernel.Bind<IAuthProvider>().To<FormAuthProvider>();
        }
    }
}