using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebUI.Infrastructure.Abstract;

namespace WebUI.Controllers
{
    public class NavController : Controller
    {
        private readonly IRepository repository;

        public NavController (IRepository repo)
        {
            repository = repo;
        }
        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;

            IEnumerable<string> categories = repository.Products
                .Select(product => product.Category)
                .Distinct()
                .OrderBy(x => x);
            return PartialView(categories);
        }
    }
}