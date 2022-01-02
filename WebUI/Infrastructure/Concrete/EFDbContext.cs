using System.Data.Entity;
using WebUI.Models;

namespace WebUI.Infrastructure.Concrete
{
    public class EFDbContext: DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
