using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using prjNovelReader.Models.ViewModels;

namespace prjNovelReader.Models.Service.Interface
{
    public interface INovelService
    {
        int GetNovelId(string novelName, string novelAuthor);
        void Create(NovelViewModel novel);
        void Update(int id,NovelViewModel novel);
        void Delete(int id);
        void CreateChapter(string tid, int numberOfPage,int novelId);
        void UpdateChapter(int id, ChapterViewModel chapter);
        void DeleteChapter(int id);
        NovelViewModel ShowNovel(int id);
        ChapterViewModel ShowChapter(int id);
    }
}