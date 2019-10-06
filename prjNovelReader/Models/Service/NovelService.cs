using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using prjNovelReader.Models;
using prjNovelReader.Models.Service.Interface;
using prjNovelReader.Models.ViewModels;
using prjNovelReader.Models.Repository;
using prjNovelReader.Models.Repository.Interface;
using prjNovelReader.Models.Service;
using WebpageCapture;
using System.Text.RegularExpressions;

namespace prjNovelReader.Models.Service
{
    public class NovelService : INovelService
    {
        private IRepository<tNovel> novelRepository = new GenericRepository<tNovel>();
        private IRepository<tCategory> categoryRepository = new GenericRepository<tCategory>();
        private IRepository<tAuthor> authorRepository = new GenericRepository<tAuthor>();
        private IRepository<tNovelTextC> textCRepository = new GenericRepository<tNovelTextC>();
        private IRepository<tNovelTextJ> textJRepository = new GenericRepository<tNovelTextJ>();
        private tNovel tNovel = new tNovel();
        public int GetNovelId(string novelName, string novelAuthor)
        {
            return novelRepository.Get(m => m.Name == novelName && m.tAuthor.Name == novelAuthor).NovelId;
        }

        private void CheckCategory(string category)
        {
            var tCategory = new tCategory();
            tCategory = categoryRepository.Get(m => m.Name == category);
            if (tCategory != null)
            {
                tNovel.CategoryId = tCategory.CategoryId;
            }
            else
            {
                var newCategory = new tCategory
                {
                    Name = category
                };
                categoryRepository.Create(newCategory);
                tNovel.CategoryId = categoryRepository.Get(m => m.Name == category).CategoryId;
            }
        }
        private void CheckAuthor(string author)
        {
            var tAuthor = new tAuthor();
            tAuthor = authorRepository.Get(m => m.Name == author);
            if (tAuthor != null)
            {
                tNovel.AuthorId = tAuthor.AuthorId;
            }
            else
            {
                var newAuthor = new tAuthor
                {
                    Name = author
                };
                authorRepository.Create(newAuthor);
                tNovel.AuthorId = authorRepository.Get(m => m.Name == author).AuthorId;
            }
        }
        public void Create(NovelViewModel novel)
        {
            tNovel.Name = novel.Name;
            tNovel.Type = novel.Type.Replace(" ","");
            CheckAuthor(novel.Author);
            var tNovelCheck = novelRepository.Get(m=>m.Name == novel.Name && m.AuthorId == tNovel.AuthorId);
            if (tNovelCheck  == null)
            {
                CheckCategory(novel.Category);
                novelRepository.Create(tNovel);
            }
        }

        public void CreateChapter(string tid, int numberOfPage, int novelId)
        {
            CaTiNoWebpageCapture caTiNoWebpageCapture = new CaTiNoWebpageCapture();        
            var url = "https://ck101.com/forum.php?mod=viewthread&tid="+tid+"&page=";
            var count = 1;
            for(var i = 1; i <= numberOfPage; i++)
            {
                var urlPage = url + i;
                caTiNoWebpageCapture.GetHtml(urlPage);
                caTiNoWebpageCapture.HtmlCapture(caTiNoWebpageCapture.HtmlText);
            }
            var tNovelTextC = new tNovelTextC();
            tNovelTextC.NovelId = novelId;
            foreach (var item in caTiNoWebpageCapture.NovelText)
            {
                tNovelTextC.ChapterNum = count;
                tNovelTextC.Text = item;
                textCRepository.Create(tNovelTextC);
                count++;
            }

        }

        public void Delete(int id)
        {
            tNovel novel = novelRepository.Get(m => m.NovelId == id);
            if (novel.Type.Replace(" ","") == "日輕")
            {
                List<tNovelTextJ> chapterList = textJRepository.GetSome(m => m.NovelId == id).ToList();
                foreach (var item in chapterList)
                {
                    DeleteChapter(item.NovelTextId, novel.Type);
                }
            }
            else
            {
                List<tNovelTextC> chapterList = textCRepository.GetSome(m => m.NovelId == id).ToList();
                foreach (var item in chapterList)
                {
                    DeleteChapter(item.NovelTextId, novel.Type);
                }
            }
            novelRepository.Delete(novel);
        }

        public void DeleteChapter(int id,string type)
        {
            if (type.Replace(" ", "") == "日輕")
            {
                textJRepository.Delete(textJRepository.Get(m => m.NovelTextId == id));
            }
            else
            {
                textCRepository.Delete(textCRepository.Get(m => m.NovelTextId == id));
            }
        }

        public void Update(int id,NovelViewModel novel)
        {
            tNovel = novelRepository.Get(m => m.NovelId == id);
            tNovel.Name = novel.Name;
            tNovel.Type = novel.Type;
            CheckAuthor(novel.Author);
            CheckCategory(novel.Category);
            novelRepository.Update(tNovel);


        }

        public void UpdateChapter(int id, ChapterViewModel chapter)
        {
            var text = textCRepository.Get(m => m.NovelTextId == id);
            text.ChapterNum = int.Parse(chapter.ChapterName);
            text.Text = chapter.Text;
            textCRepository.Update(text);
        }

        public NovelViewModel ShowNovel(int id)
        {
            var novel = novelRepository.Get(m => m.NovelId == id);
            var novelData = new NovelViewModel()
            {
                Name = novel.Name,
                Category = novel.tCategory.Name,
                Author = novel.tAuthor.Name,
                Type = novel.Type
            };
            return novelData;
        }

        public ChapterViewModel ShowChapter(int id,string Type)
        {
            var chapter = textCRepository.Get(m => m.NovelTextId == id);

            var chapterData = new ChapterViewModel()
            {
                ChapterName = chapter.ChapterNum.ToString(),
                NovelName = chapter.tNovel.Name,
                Text = chapter.Text
            };
            return chapterData;
        }

        public void CreateChapterWenKu(string url,int novelId)
        {
            Wenku8WebpageCapture pageCapture = new Wenku8WebpageCapture();
            int reelCount = 1;
            int chapterCount = 1;
            int captureCount = 0;
            var tNovelTextJ = new tNovelTextJ();
            tNovelTextJ.NovelId = novelId;
            pageCapture.GetHtml(url);
            string str = pageCapture.HtmlText;
            var links = new List<string>();
            url = url.Replace("index.htm", "");
            str = Regex.Replace(str, "(<!D).+?(<table)", "");
            str = Regex.Replace(str, "(/table).+?(/html>)", "");
            MatchCollection reels = Regex.Matches(str, "(colspan).+?(后记)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            foreach (Match reel in reels)
            {
                str = reel.Value;
                MatchCollection chapters = Regex.Matches(str, "(href=\").+?(htm)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                foreach (Match chapter in chapters)
                {
                    pageCapture.GetHtml(url + chapter.Value.Replace("href=\"", ""));
                    pageCapture.HtmlCapture(pageCapture.HtmlText);
                    tNovelTextJ.ReelNum = reelCount;
                    tNovelTextJ.ChapterNum = chapterCount;
                    tNovelTextJ.Text = pageCapture.NovelText[captureCount];
                    textJRepository.Create(tNovelTextJ);
                    chapterCount++;
                    captureCount++;
                }
                reelCount++;
                chapterCount = 1;
            }


        }
    }
}