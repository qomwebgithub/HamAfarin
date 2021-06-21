using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataLayer
{
    public class Tbl_PagesMetadata
    {
        [Key]
        public int PageID { get; set; }
        [Display(Name = "فعال")]
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        [Display(Name = "تصویر")]
        public string ImageName { get; set; }
        [Display(Name = "متن")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string FullText { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public Nullable<System.DateTime> CreateDate { get; set; }
        [Display(Name = "عنوان url")]
        public string UrlTitle { get; set; }
    }
    [MetadataType(typeof(Tbl_PagesMetadata))]
    public partial class Tbl_Pages
    {

    }
}
