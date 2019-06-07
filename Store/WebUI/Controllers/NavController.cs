using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class NavController : Controller
    {
        private IGoodRepository repository;



        public NavController(IGoodRepository repo)
        {
            repository = repo;
        }

        //public PartialViewResult Menu(string category = null)
        //{
        //    ViewBag.SelectedCategory = category;

        //    IEnumerable<string> categories = repository.Goods
        //        .Select(good => good.Category)
        //        .Distinct()
        //        .OrderBy(x => x);
        //    return PartialView(categories);
        //}

        public PartialViewResult Menu(string category = null, bool horizontalNav = false)
        {
            ViewBag.SelectedCategory = category;
            IEnumerable<string> categories = repository.Goods
                .Select(g => g.Category)
                .Distinct()
                .OrderBy(x => x);
            string viewName = horizontalNav ? "MenuHorizontal" : "Menu";

            return PartialView("FlexMenu", categories);
        }

    }
}