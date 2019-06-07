using System.Linq;
using System.Web.Mvc;
using Domain.Entities;
using Domain.Abstract;
using WebUI.Models;

namespace GameStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        public ViewResult Index(Cart cart,string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        private IGoodRepository repository;
        private IOrderProcessor orderRrocessor;
        public CartController(IGoodRepository repo,IOrderProcessor processor)
        {
            repository = repo;
            orderRrocessor = processor;
        }

        public RedirectToRouteResult AddToCart(Cart cart,int id, string returnUrl)
        {
            Good good= repository.Goods
                .FirstOrDefault(g => g.Id == id);

            if (good != null)
            {
                cart.AddItem(good,1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart,int id, string returnUrl)
        {
            Good good = repository.Goods
                .FirstOrDefault(g => g.Id == id);

            if (good != null)
            {
                cart.RemoveLine(good);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public PartialViewResult Summary (Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart,ShippingDetails shippingDetails)
        {

            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Ваша корзина пуста");
            }
            if (ModelState.IsValid)
            {
                orderRrocessor.ProcessorOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }

        }
    }
}