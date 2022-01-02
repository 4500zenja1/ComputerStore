using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using WebUI.Infrastructure.Abstract;
using WebUI.Models;
using System.Web;
using System.Text;

namespace WebUI.Infrastructure.Concrete
{
    public class EFRepository: IRepository
    {
        readonly EFDbContext context = new();

        public IEnumerable<Order> Orders
        {
            get { return context.Orders; }
        }

        public void SaveOrder(Cart cart, ShippingDetails shippingInfo)
        {
            string UserId = HttpContext.Current.User.Identity.GetUserId();
            StringBuilder Details = new StringBuilder()
                .AppendLine("Товары:<br/>");

            foreach (var line in cart.Lines)
            {
                var subtotal = line.Product.Price * line.Quantity;
                Details.AppendFormat("{0} x {1} (итого: {2} Br.)<br/>",
                    line.Quantity, line.Product.Name, subtotal);
            }

            Details.AppendFormat("Общая стоимость: {0} Br.<br/><br/>", cart.ComputeTotalValue())
                .AppendLine("Доставка:<br/>")
                .AppendLine("Имя: " + shippingInfo.Name + "<br/>")
                .AppendLine("Адрес № 1: " + shippingInfo.Line1 + "<br/>")
                .AppendLine("Адрес № 2: " + (shippingInfo.Line2 ?? "не указан") + "<br/>")
                .AppendLine("Адрес № 3: " + (shippingInfo.Line3 ?? "не указан") + "<br/>")
                .AppendLine("Город: " + shippingInfo.City + "<br/>")
                .AppendLine("Страна: " + shippingInfo.Country + "<br/><br/>")
                .AppendFormat("Подарочная упаковка: {0}",
                    shippingInfo.GiftWrap ? "Да" : "Нет");
            Order order = new()
            {
                UserId = UserId,
                Details = Details.ToString()
            };
            context.Orders.Add(order);
            context.SaveChanges();
        }

        public Order DeleteOrder(int orderId)
        {
            Order dbEntry = context.Orders.Find(orderId);
            if (dbEntry != null)
            {
                context.Orders.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        public IEnumerable<Product> Products
        {
            get { return context.Products; }
        }

        public void SaveProduct(Product product)
        {
            if (product.ProductId == 0)
            {
                context.Products.Add(product);
            }
            else
            {
                Product dbEntry = context.Products.Find(product.ProductId);
                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Category = product.Category;
                    dbEntry.Price = product.Price;
                    dbEntry.ImageData = product.ImageData;
                    dbEntry.ImageMimeType = product.ImageMimeType;
                }
            }
            context.SaveChanges();
        }

        public Product DeleteProduct(int productId)
        {
            Product dbEntry = context.Products.Find(productId);
            if (dbEntry != null)
            {
                context.Products.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}