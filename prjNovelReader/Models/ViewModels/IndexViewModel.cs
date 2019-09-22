using System.ComponentModel;

namespace prjNovelReader.Models.ViewModels
{
    public class IndexViewModel 
    {
        public int Id { get; set; }
        [DisplayName("小說名稱")]
        public string NovelName { get; set; }
        [DisplayName("種類")]
        public string Category { get; set; }
        [DisplayName("作者")]
        public string Author { get; set; }

    }
}