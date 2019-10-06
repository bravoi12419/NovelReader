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
        IPagedList<IndexViewModel> ShowNovel(string searchString, int? categoryId, int page = 1);
        ShowChapterViewModel ShowChapterC(int novelId);
        ReadChapterViewModel ReadChapter (int chapterId,string type);
        string GetCategoryDList(int? categoryId);
        string GetDropdownList(string name, IDictionary<string, string> optionData, object htmlAttributes, string defaultSelectValue, bool appendOptionLabel, string optionLabel);

    }
}