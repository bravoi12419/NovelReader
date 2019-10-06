using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace prjNovelReader.Models.ViewModels
{
    public class NovelViewModel
    {
        [DisplayName("小說名稱"),Required]
        public string Name { get; set; }
        [DisplayName("種類"), Required]
        public string Category { get; set; }
        [DisplayName("作者"), Required]
        public string Author { get; set; }
        [DisplayName("來源"),Required]
        public string Type { get; set; }

    }
}