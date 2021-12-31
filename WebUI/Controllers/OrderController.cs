using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Infrastructure.Abstract;

namespace WebUI.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository repository;
        public OrderController(IOrderRepository repo)
        {
            repository = repo;
        }

        public ViewResult List()
        {
            return View(repository.Orders);
        }
    }
}