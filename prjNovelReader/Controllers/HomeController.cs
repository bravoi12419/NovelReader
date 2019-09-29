using System.Collections.Generic;
using System.Web.Mvc;
using prjNovelReader.Models.Service;
using prjNovelReader.Models.Service.Interface;



namespace prjNovelReader.Controllers
{
    public class HomeController : Controller
    {
        private IHomeService homeService;
        public HomeController()
        {
            homeService = new HomeService();
        }

        // GET: Home
        public ActionResult Index(string searchString,int categoryId=0, int page=1)
        {
            ViewBag.categoryDList = homeService.GetCategoryDList(categoryId);
            ViewBag.categoryId = categoryId;
            ViewBag.searchString = searchString;
            ViewBag.page = page;

            return View();
        }
        public ActionResult Bookshelf(string searchString, int? categoryId, int page = 1)
        {
            if (categoryId == 0) categoryId = null;
            return PartialView(homeService.ShowNovel(searchString, categoryId, page));
        }

        public ActionResult ReadNovel(int novelId)
        {

            return View(homeService.ShowChapterC(novelId));
        }

        public ActionResult ReadChapter(int id,int glyph=0, int size=0)
        {
            ViewData["Id"] = id;
            ViewData["PreviousId"] = (id-1);
            ViewData["NextId"] = (id+1);
            ViewData["currentGlyph"] = glyph;
            ViewData["currentSize"] = size;
            return View(homeService.ReadChapter(id));
        }



    }
}