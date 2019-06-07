using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class GoodController : Controller
    {
        private IGoodRepository repository;
        public int pageSize = 4;   //show how many goods will be on one page
        public GoodController(IGoodRepository repo)
        {
            repository = repo;
        }


        public ViewResult List(string category, int page = 1)
        {
            GoodsListViewModel model = new GoodsListViewModel
            {
                Goods = repository.Goods
                .Where(p => category == null || p.Category == category)
                .OrderBy(goods => goods.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),

                PageInfo = new PageInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ?
                    repository.Goods.Count() : repository.Goods.Where(good=>good.Category==category).Count()
                },
                CurrentCategory=category
                
            };
            return View(model);
        }

        public FileContentResult GetImage(int goodId)
        {
            Good good = repository.Goods.FirstOrDefault(g => g.Id == goodId);

            if (good != null)
            {
                return File(good.ImageData, good.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
    }
}