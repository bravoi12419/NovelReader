using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using prjNovelReader.Models.ViewModels;
using prjNovelReader.Models.Service;
using prjNovelReader.Models.Service.Interface;

namespace prjNovelReader.Controllers
{
    public class NovelController : Controller
    {
        private INovelService novelService;
        public NovelController()
        {
            novelService = new NovelService();
        }
        // GET: AddNovel
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddNewNovel()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddNewNovel(NovelViewModel novel)
        {
            novelService.Create(novel);
            return RedirectToAction("AddChapter", "Novel", new { newNovelName=novel.Name, newNovelAuthor=novel.Author });
        }
        public ActionResult AddChapter(string newNovelName, string newNovelAuthor)
        {
            ViewBag.newNovelName = newNovelName;
            ViewBag.newNovelAuthor = newNovelAuthor;
            TempData[newNovelName] = newNovelName;
            TempData[newNovelAuthor] = newNovelAuthor;
            return View();
        }
        public ActionResult AddChapterChina(string newNovelName, string newNovelAuthor)
        {
            ViewBag.newNovelName = newNovelName;
            ViewBag.newNovelAuthor = newNovelAuthor;
            return View();
        }
        [HttpPost]
        public ActionResult AddChapterChina(string tid,int numberOfPage, string novelName, string novelAuthor)
        {
            var novelId=novelService.GetNovelId(novelName, novelAuthor);
            novelService.CreateChapter(tid, numberOfPage, novelId);
            return RedirectToAction("ReadNovel", "Home", new { novelId });
        }

        public ActionResult AddChapterJapan(string newNovelName, string newNovelAuthor)
        {
            ViewBag.newNovelName = newNovelName;
            ViewBag.newNovelAuthor = newNovelAuthor;
            return View();
        }
        [HttpPost]
        public ActionResult AddChapterJapan(string url, string novelName, string novelAuthor)
        {
            var novelId = novelService.GetNovelId(novelName, novelAuthor);
            novelService.CreateChapterWenKu(url, novelId);
            return RedirectToAction("ReadNovel", "Home", new { novelId });
        }
        public ActionResult Edit(int id)
        {
            ViewBag.NovelId = id;
            return View(novelService.ShowNovel(id));
        }
        [HttpPost]
        public ActionResult Edit(int id, NovelViewModel novel)
        {
            novelService.Update(id, novel);
            return RedirectToAction("ReadNovel", "Home", new { novelId = id });
        }
        public ActionResult EditChapter(int id)
        {
            ViewBag.ChapterId = id;
            string type = "日"; 
            return View(novelService.ShowChapter(id,type));
        }
        [HttpPost]
        public ActionResult EditChapter(int id,ChapterViewModel chapter)
        {
            novelService.UpdateChapter(id, chapter);
            return RedirectToAction("ReadChapter", "Home", new { id });
        }
        public ActionResult Delete(int id)
        {
            novelService.Delete(id);
            return RedirectToAction("index", "Home");
        }
        public ActionResult DeleteChapter(int id,int novelId,string novelType)
        {
            novelService.DeleteChapter(id, novelType);
            return RedirectToAction("ReadNovel", "Home",new { novelId = novelId });
        }

    }
}