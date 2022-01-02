using System.Web.Mvc;
using WebUI.Models;

namespace WebUI.Infrastructure.Binders
{
    public class CartModelBinder : IModelBinder
    {
        private const string sessionKey = "Cart";

        public object BindModel(ControllerContext controllercontext,
            ModelBindingContext bindingContext)
        {
            Cart cart = null;
            if (controllercontext.HttpContext.Session != null)
            {
                cart = (Cart)controllercontext.HttpContext.Session[sessionKey];
            }

            if (cart == null)
            {
                cart = new Cart();
                if (controllercontext.HttpContext.Session != null)
                {
                    controllercontext.HttpContext.Session[sessionKey] = cart;
                }
            }
            return cart;
        }
    }
}