using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using prjNovelReader.Models.Service.Interface;
using prjNovelReader.Models.Service;
using WebpageCapture;
using System.Text.RegularExpressions;

namespace prjNovelReader.Controllers
{
    public class TestController : Controller
    {
        private IHomeService homeService = new HomeService();
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CssTest()
        {
            return View();
        }
        public ActionResult AjaxPageListTest(string searchString, int categoryId=0,int page=1)
        {
            ViewBag.categoryDList = homeService.GetCategoryDList(categoryId);
            ViewBag.categoryId = categoryId;
            ViewBag.searchString = searchString ;
            ViewBag.page = page;
            return View();
        }
        public ActionResult PagedPartial(string searchString, int? categoryId, int page = 1)
        {
            if (categoryId == 0) categoryId = null;
            return PartialView(homeService.ShowNovel(searchString, categoryId, page));
        }

        public ActionResult CssAnimationTest()
        {

            return View();
        }
        public ActionResult Test()
        {
            //var url = "https://www.wenku8.net/novel/2/2428/index.htm";

            //ViewBag.data = pageCapture.NovelText;
            return View();
        }
    }
}