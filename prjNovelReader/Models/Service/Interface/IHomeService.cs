using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using prjNovelReader.Models.ViewModels;
namespace prjNovelReader.Models.Service.Interface
{
    public interface IHomeService
    {
        IPagedList<IndexViewModel> ShowNovel(string searchString, int? cate, int page = 1);
        ShowChapterViewModel ShowChapterC(int novelId);
        ReadChapterViewModel ReadChapter (int chapterId);
        string GetCategoryDList(int? cate);
        string GetDropdownList(string name, IDictionary<string, string> optionData, object htmlAttributes, string defaultSelectValue, bool appendOptionLabel, string optionLabel);

    }
}