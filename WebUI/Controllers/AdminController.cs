using System.Web.Mvc;
using Domain.Abstract;

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
    }
}