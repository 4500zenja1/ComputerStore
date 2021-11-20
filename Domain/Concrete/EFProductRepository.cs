using System.Collections.Generic;
using Domain.Abstract;
using Domain.Entities;

namespace Domain.Concrete
{
    public class EFProductRepository: IProductRepository
    {
        readonly EFDbContext context = new();

        public IEnumerable<Product> Products
        {
            get { return context.Products; }
        }
    }
}
