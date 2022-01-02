using System;
using System.Collections.Generic;
using WebUI.Models;

namespace WebUI.Infrastructure.Abstract
{
    public interface IRepository
    {
        IEnumerable<Order> Orders { get;  }
        void SaveOrder(Cart cart, ShippingDetails details);
        Order DeleteOrder(int orderId);

        IEnumerable<Product> Products { get; }
        void SaveProduct(Product product);
        Product DeleteProduct(int productId);
    }
}
