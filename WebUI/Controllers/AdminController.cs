using System.Linq;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entities;

namespace WebUI.Controllers
{
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
                TempData["message"] = string.Format("Изменения в товаре \"{0}\" были сохранены :D", product.Name);
                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
        }
    }
}