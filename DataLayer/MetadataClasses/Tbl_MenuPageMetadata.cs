using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Tbl_MenuPageMetadata
    {
        [Key]
        public int MenuPageID { get; set; }
        [Display(Name = "فعال")]
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        [Display(Name = "عنوان")]
        public string Title { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public Nullable<System.DateTime> CreateDate { get; set; }
        [Display(Name = "آدرس صفحه")]
        public string Url { get; set; }
        [Display(Name = "صفحه منو")]
        public Nullable<int> Page_id { get; set; }
        [Display(Name = "نوع صفحه")]
        public Nullable<int> MenuType_id { get; set; }
        [Display(Name = "جایگاه")]
        public Nullable<int> Sort { get; set; }
    }
    [MetadataType(typeof(Tbl_MenuPageMetadata))]
    public partial class Tbl_MenuPage
    {

    }
}
