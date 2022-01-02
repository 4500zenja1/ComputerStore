using System.Web.Mvc;
using WebUI.Infrastructure.Abstract;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class OrderController : Controller
    {
        readonly IRepository repository;

        public OrderController(IRepository repo)
        {
            repository = repo;
        }
        public ActionResult Index()
        {
            return View(repository.Orders);
        }

        [HttpPost]
        public ActionResult Delete(int orderId)
        {
            Order deletedOrder= repository.DeleteOrder(orderId);
            if (deletedOrder != null)
            {
                TempData["message"] = "Заказ успешно удалён из базы данных";
            }
            return RedirectToAction("Index");
        }
    }
}