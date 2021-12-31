using System.Collections.Generic;
using Domain.Entities;

namespace WebUI.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public AppUser Customer { get; set; }
        public List<Product> Products { get; set; }
        public ShippingDetails Details { get; set; }
    }
}
