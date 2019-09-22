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

namespace prjNovelReader.Models.Service
{
    public class NovelService : INovelService
    {
        private IRepository<tNovel> novelRepository = new GenericRepository<tNovel>();
        private IRepository<tCategory> categoryRepository = new GenericRepository<tCategory>();
        private IRepository<tAuthor> authorRepository = new GenericRepository<tAuthor>();
        private IRepository<tNovelTextC> textCRepository = new GenericRepository<tNovelTextC>();
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
            List<tNovelTextC> chapterList = textCRepository.GetSome(m => m.NovelId == id).ToList();
            foreach(var item in chapterList)
            {
                DeleteChapter(item.NovelTextId);
            }
            novelRepository.Delete(novelRepository.Get(m => m.NovelId == id));
        }

        public void DeleteChapter(int id)
        {
            textCRepository.Delete(textCRepository.Get(m => m.NovelTextId == id));
        }

        public void Update(int id,NovelViewModel novel)
        {
            tNovel = novelRepository.Get(m => m.NovelId == id);
            tNovel.Name = novel.Name;
            CheckAuthor(novel.Author);
            CheckCategory(novel.Category);
            novelRepository.Update(tNovel);


        }

        public void UpdateChapter(int id, ChapterViewModel chapter)
        {
            var text = textCRepository.Get(m => m.NovelTextId == id);
            text.ChapterNum = int.Parse(chapter.ChapterNumber);
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
                Author = novel.tAuthor.Name
            };
            return novelData;
        }

        public ChapterViewModel ShowChapter(int id)
        {
            var chapter = textCRepository.Get(m => m.NovelTextId == id);

            var chapterData = new ChapterViewModel()
            {
                ChapterNumber = chapter.ChapterNum.ToString(),
                NovelName = chapter.tNovel.Name,
                Text = chapter.Text
            };
            return chapterData;
        }
    }
}