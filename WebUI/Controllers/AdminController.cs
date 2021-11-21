using System.Linq;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entities;

namespace WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        readonly IProductRepository repository;

        public AdminController(IProductRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index()
        {
            return View(repository.Products);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View("Edit", new Product());
        }

        public ViewResult Edit(int productId)
        {
            Product product = repository.Products
                .Where(x => x.ProductId == productId)
                .FirstOrDefault();
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                repository.SaveProduct(product);
                if (product.ProductId == repository.Products.ToList()[repository.Products.Count()-1].ProductId)
                {
                    TempData["message"] = string.Format("Товар \"{0}\" успешно добавлен!", product.Name);
                }
                else
                {
                    TempData["message"] = string.Format("Изменения в товаре \"{0}\" были сохранены!", product.Name);
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
        }

        [HttpPost]
        public ActionResult Delete(int productId)
        {
            Product deletedProduct = repository.DeleteProduct(productId);
            if (deletedProduct != null)
            {
                TempData["message"] = string.Format("Товар \"{0}\" успешно удалён из базы данных",
                    deletedProduct.Name);
            }
            return RedirectToAction("Index");
        }
    }
}