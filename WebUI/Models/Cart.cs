using System.Collections.Generic;
using System.Linq;

namespace WebUI.Models
{
    public class Cart
    {
        private readonly List<CartLine> lineCollection = new();

        public void AddItem(Product product, int quantity)
        {
            CartLine line = lineCollection
                .Where(x => x.Product.ProductId == product.ProductId)
                .FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            } else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Product product)
        {
            lineCollection.RemoveAll(x => x.Product.ProductId == product.ProductId);
        }

        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(x => x.Product.Price * x.Quantity);
        }

        public void Clear()
        {
            lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }

    public class CartLine
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
