using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using PagedList;
using prjNovelReader.Models.Service.Interface;
using prjNovelReader.Models.ViewModels;
using prjNovelReader.Models.Repository;
using prjNovelReader.Models.Repository.Interface;

namespace prjNovelReader.Models.Service
{
    public class HomeService : IHomeService
    {
        private IRepository<tCategory> categoryRepository = new GenericRepository<tCategory>();
        private IRepository<tNovel> novelRepository = new GenericRepository<tNovel>();
        private IRepository<tNovelTextC> textCRepository = new GenericRepository<tNovelTextC>();
        private IRepository<tNovelTextJ> textJRepository = new GenericRepository<tNovelTextJ>();
        private int pageSize = 5;
        public string GetDropdownList(string name, IDictionary<string, string> optionData, object htmlAttributes, string defaultSelectValue, bool appendOptionLabel, string optionLabel)
        {
            //name=tag內名稱  optionData=選單選項 htmlAttributes=額外增加的html defaultSelectValue=默認選擇選項(selected) appendOptionLabel=是否額外加上選項(全部) optionLabel=額外選項字串
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "產生DropDownList時 tag Name 不得為空");
            }
            TagBuilder select = new TagBuilder("select");
            select.Attributes.Add("name", name);
            StringBuilder renderHtmlTag = new StringBuilder();
            IDictionary<string, string> newOptionData = new Dictionary<string, string>();
            if (appendOptionLabel)
            {
                newOptionData.Add(new KeyValuePair<string, string>(optionLabel ?? "請選擇", ""));
            }
            foreach (var item in optionData)
            {
                newOptionData.Add(item);
            }
            foreach (var option in newOptionData)
            {
                TagBuilder optionTag = new TagBuilder("option");
                optionTag.Attributes.Add("value", option.Value);
                if (!string.IsNullOrEmpty(defaultSelectValue) && defaultSelectValue.Equals(option.Value))  //若已選擇，在加入已選擇像時額外加上selected
                {
                    optionTag.Attributes.Add("selected", "selected");
                }
                optionTag.SetInnerText(option.Key);
                renderHtmlTag.AppendLine(optionTag.ToString(TagRenderMode.Normal)); //將<option>放入stringBuilder中
            }
            select.MergeAttributes(new RouteValueDictionary(htmlAttributes)); //select加入其餘html屬性
            select.InnerHtml = renderHtmlTag.ToString(); //插入select內部
            return select.ToString();
        }
        public string GetCategoryDList(int? categoryId)
        {
            var dict = new Dictionary<string, string>();
            var categories = categoryRepository.GetAll().ToList();
            foreach (var item in categories)
            {
                dict.Add(item.Name, item.CategoryId.ToString());
            }
            return GetDropdownList
            (
            "categoryId",
            dict,
            new { @class = "form-control", id = "categoryId" },
            categoryId.ToString(),
            true,
            "全部"
            );
        }
        public IPagedList<IndexViewModel> ShowNovel(string searchString, int? categoryId, int page)
        {
            var indexVMs = new List<IndexViewModel>();
            var novels = new List<tNovel>();

            if (categoryId != null)
            {
                novels = novelRepository.GetSome(m => m.CategoryId == categoryId).ToList();
            }
            else
            {
                novels = novelRepository.GetAll().ToList();
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                novels = novels.Where(m => m.Name.Contains(searchString)).ToList();
            }

            foreach (var item in novels)
            {
                var IndexVM = new IndexViewModel
                {
                    Id = item.NovelId,
                    NovelName = item.Name,
                    Author = item.tAuthor.Name,
                    Category = item.tCategory.Name,
                    Type = item.Type
                };
                indexVMs.Add(IndexVM);
            }
            int currentPage = pageSize < 1 ? 1 : page;
            return indexVMs.ToPagedList(currentPage, pageSize);
        }

        public ShowChapterViewModel ShowChapterC(int novelId)
        {
            var novel = novelRepository.Get(m => m.NovelId == novelId);
            var showChapterViewModel = new ShowChapterViewModel() {
                NovelId = novelId,
                NovelName = novel.Name,
                Category = novel.tCategory.Name,
                Author = novel.tAuthor.Name,
                Type = novel.Type,
                ChapterIdList = new List<int>(),
                ChapterNameList = new List<string>()
            };
            if( novel.Type.Replace(" ","") == "日輕")
            {
                var list = textJRepository.GetSome(m => m.NovelId == novelId).ToList();
                foreach (var item in list)
                {
                    showChapterViewModel.ChapterIdList.Add(item.NovelTextId);
                    showChapterViewModel.ChapterNameList.Add($"第{item.ReelNum}卷-第{item.ChapterNum}章");
                }
            }
            else
            {
                var list = textCRepository.GetSome(m => m.NovelId == novelId).ToList();
                foreach (var item in list)
                {
                    showChapterViewModel.ChapterIdList.Add(item.NovelTextId);
                    showChapterViewModel.ChapterNameList.Add($"第{item.ChapterNum}章");
                }
            }
            return showChapterViewModel;
        }

        public ReadChapterViewModel ReadChapter(int chapterId,string type)
        {
            if (type.Replace(" ","") == "日輕")
            {
                var chapter = textJRepository.Get(m => m.NovelTextId == chapterId);
                var readChapterVM = new ReadChapterViewModel()
                {
                    Id = chapter.NovelTextId,
                    NovelId = chapter.NovelId,
                    ChapterId = chapter.ChapterNum,
                    ChapterText = chapter.Text,
                    NovelName = chapter.tNovel.Name
                };
                return readChapterVM;
            }
            else
            {
                var chapter = textCRepository.Get(m => m.NovelTextId == chapterId);
                var readChapterVM = new ReadChapterViewModel()
                {
                    Id = chapter.NovelTextId,
                    NovelId = chapter.NovelId,
                    ChapterId = chapter.ChapterNum,
                    ChapterText = chapter.Text,
                    NovelName = chapter.tNovel.Name
                };
                return readChapterVM;
            }
        }
    }
}