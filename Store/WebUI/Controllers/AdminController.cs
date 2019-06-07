using Domain.Abstract;
using Domain.Entities;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
       private IGoodRepository repository;
        
        public AdminController (IGoodRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index()
        {
            return View(repository.Goods);
        }

        //public ViewResult Edit(int goodId)
        //{
        //    Good good = repository.Goods.FirstOrDefault(g => g.Id == goodId);
        //    return View(good);
        //}

 public ActionResult Edit(int Id)
        {
            Good good = repository.Goods
                .FirstOrDefault(g => g.Id == Id);
            return View(good);
        }

        [HttpPost]
        public ActionResult Edit(Good good, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    good.ImageMimeType = image.ContentType;
                    good.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(good.ImageData, 0, image.ContentLength);
                }
                repository.SaveGood(good);
                TempData["message"] = string.Format("Изменения в игре \"{0}\" были сохранены", good.Name);
                return RedirectToAction("Index");
            }
            else
            {
                
                return View(good);
            }
        }
        ///////////////
        public ActionResult Create()
        {
            return View("Edit", new Good());
        }
        /// <summary>
        /// /////////////////
        /// </summary>
        /// <param name="goodId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int goodId)
        {
            Good deletedGood = repository.DeleteGood(goodId);
            if (deletedGood != null)
            {
                TempData["message"] = string.Format("Игра \"{0}\" была удалена", deletedGood.Name);
            }
            return RedirectToAction("Index");
        }
        
       
    }
}