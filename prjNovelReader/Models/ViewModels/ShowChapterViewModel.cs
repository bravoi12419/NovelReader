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
        [DisplayName("章節列表")]
        public List<tNovelTextC> ChapterList { get; set; }
    }
}