using System.Linq;
using System.Web.Mvc;
using WebUI.Infrastructure.Abstract;
using WebUI.Infrastructure.Concrete;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly IRepository repository;
        private readonly EFRepository orderRep;
        public CartController(IRepository repo, EFRepository rep)
        {
            repository = repo;
            orderRep = rep;
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Для оформления заказа необходимо наличие как минимум" +
                    " одного товара в корзине!");
            }

            if (ModelState.IsValid)
            {
                orderRep.SaveOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }

        public ViewResult Index(Cart cart)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart
            });
        }

        public RedirectToRouteResult AddToCart(Cart cart, int productId)
        {
            Product product = repository.Products
                .FirstOrDefault(x => x.ProductId == productId);

            if (product != null)
            {
                cart.AddItem(product, 1);
                TempData["message"] = string.Format("Товар \"{0}\" успешно добавлен в корзину!",
                    product.Name);
            }
            return RedirectToAction("List", "Product");
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int productId)
        {
            Product product = repository.Products
                .FirstOrDefault(x => x.ProductId == productId);

            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index");
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }
    }
}