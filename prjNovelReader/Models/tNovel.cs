//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace prjNovelReader.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tNovel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tNovel()
        {
            this.tNovelTextC = new HashSet<tNovelTextC>();
            this.tNovelTextJ = new HashSet<tNovelTextJ>();
        }
    
        public int NovelId { get; set; }
        public string Name { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> AuthorId { get; set; }
        public string Type { get; set; }
    
        public virtual tAuthor tAuthor { get; set; }
        public virtual tCategory tCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tNovelTextC> tNovelTextC { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tNovelTextJ> tNovelTextJ { get; set; }
    }
}
