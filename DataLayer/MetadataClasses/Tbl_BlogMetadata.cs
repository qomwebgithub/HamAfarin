using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataLayer
{
    public class Tbl_BlogMetadata
    {
        [Key]
        public int BlogID { get; set; }
        [Display(Name = "حذف شده")]
        public bool IsDeleted { get; set; }
        [Display(Name = "فعال")]
        public bool IsActive { get; set; }
        [Display(Name = "تاریخ حذف")]
        public Nullable<System.DateTime> DeleteDate { get; set; }
        [Display(Name = "کاربر ایجاد کننده")]
        public Nullable<int> CreateUser_id { get; set; }
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }
        [Display(Name = "تگ عنوان سئو")]
        public string MetaTitle { get; set; }
        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }
        [Display(Name = "تگ توضیحات سئو")]
        public string MetaDescription { get; set; }
        [Display(Name = "متن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string FullText { get; set; }
        [Display(Name = "تصویر")]
        public string ImageName { get; set; }
        [Display(Name = "تگ توضیحات تصویر")]
        public string ImageAlt { get; set; }
        [Display(Name = "تگ کنونیکال")]
        public string Canonical { get; set; }
        [Display(Name = "آدرس عنوان")]
        public string TitleUrl { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        [DisplayFormat(DataFormatString = "{0: yyyy/mm/dd}")]
        public Nullable<System.DateTime> CreateDate { get; set; }
        [Display(Name = "نمایش در صفحه اصلی")]
        public bool ShowMainPage { get; set; }
        [Display(Name = "تعداد مشاهده")]
        public Nullable<int> CountVisit { get; set; }  
        [Display(Name = "کلمات کلیدی سئو")]
        public string SeoKey { get; set; }
    }

    [MetadataType(typeof(Tbl_BlogMetadata))]
    public partial class  Tbl_Blog
    {

    }
}
