using System.Data.Entity;
using Domain.Entities;

namespace Domain.Concrete
{
    public class EFDbContext: DbContext
    {
        public DbSet<Product> Products { get; set; }
    }
}
