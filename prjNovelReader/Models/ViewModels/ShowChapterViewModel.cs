using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace prjNovelReader.Models.ViewModels
{
    public class ShowChapterViewModel
    {
        public int NovelId { get; set; }
        [DisplayName("小說名稱")]
        public string NovelName { get; set; }
        [DisplayName("種類")]
        public string Category { get; set; }
        [DisplayName("作者")]
        public string Author { get; set; }
        [DisplayName("來源")]
        public string Type { get; set; }
        [DisplayName("章節名稱")]
        public List<string> ChapterNameList { get; set; }
        public List<int> ChapterIdList { get; set; }

    }
}