using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace prjNovelReader.Models.ViewModels
{
    public class ChapterViewModel
    {
        [DisplayName("章節編號"), Required]
        public string ChapterNumber { get; set; }
        [DisplayName("書名"), Required]
        public string NovelName { get; set; }
        [DisplayName("內容"), Required]
        public string Text { get; set; }
    }
}