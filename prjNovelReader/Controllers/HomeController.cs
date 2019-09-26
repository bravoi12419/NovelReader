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

        public ActionResult ReadChapter(int id,int? glyph, int? size)
        {
            if (glyph == null)
            {
                glyph = 0;
            }
            if (size == null)
            {
                size = 0;
            }
            ViewData["Id"] = id;
            ViewData["PreviousId"] = (id-1);
            ViewData["NextId"] = (id+1);
            ViewData["currentGlyph"] = glyph;
            ViewData["currentSize"] = size;
            string[] Size = { "小", "中", "大" };
            string[] Glyph = {"標楷體","微軟正黑體" };
            var dict = new Dictionary<string, string>();
            for(var i = 0; i < 3; i++)
            {
                dict.Add(Size[i],i.ToString());
            }
            ViewData["sizeDList"] = homeService.GetDropdownList(
                "size",
                dict,
                new { @class = "form-control", id = "size" },
                size.ToString(),
                false,
                "");

            dict.Clear();
            for(var i = 0; i < 2; i++)
            {
                dict.Add(Glyph[i], i.ToString());
            }
            ViewData["glyphDList"] = homeService.GetDropdownList(
                "glyph",
                dict,
                new { @class = "form-control", id = "glyph" },
                glyph.ToString(),
                false,
                ""
                );
            switch (size)
            {
                case 0:
                    ViewBag.size = "16px;";
                    break;
                case 1:
                    ViewBag.size = "24px;";
                    break;
                case 2:
                    ViewBag.size = "32px;";
                    break;
            }
            switch (glyph)
            {
                case 0:
                    ViewBag.glyph = "DFKai-sb;";
                    break;
                case 1:
                    ViewBag.glyph = "Microsoft JhengHei;";
                    break;
            }
            //ViewData["chapterText"]=homeService.ReadChapter(id);

            return View(homeService.ReadChapter(id));
        }



    }
}