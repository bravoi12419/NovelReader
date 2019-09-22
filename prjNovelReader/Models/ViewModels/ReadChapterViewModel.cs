using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjNovelReader.Models.ViewModels
{
    public class ReadChapterViewModel
    {
        public int Id { get; set; }
        public int? NovelId { get; set; }
        public int? ChapterId { get; set; }
        public string ChapterText { get; set; }
        public string NovelName { get; set; }
    }
}