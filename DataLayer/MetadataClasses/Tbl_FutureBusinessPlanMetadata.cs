using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataLayer
{
    public class Tbl_FutureBusinessPlanMetadata
    {
        [Key]
        public int FutureBusinessPlanID { get; set; }
        [Display(Name = "فعال")]
        public bool IsActive { get; set; }
        [Display(Name = "حذف شده")]
        public bool IsDeleted { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public Nullable<System.DateTime> CreateDate { get; set; }
        [Display(Name = "عنوان")]
        public string FutureBusinessPlanTitle { get; set; }
        [Display(Name = "توضیحات مختصر")]
        public string Description { get; set; }
        [Display(Name = "توضیحات کامل")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string FullText { get; set; }
        [Display(Name = "تصویر")]
        public string ImageName { get; set; }

        [Display(Name = "تگ عنوان سئو")]
        public string MetaTitle { get; set; }

        [Display(Name = "تگ توضیحات سئو")]
        public string MetaDescription { get; set; }

        [Display(Name = "تگ توضیحات تصویر")]
        public string ImageAlt { get; set; }

        [Display(Name = "تگ کنونیکال")]
        public string Canonical { get; set; }

        [Display(Name = "آدرس عنوان")]
        public string TitleUrl { get; set; }

        [Display(Name = "کلمات کلیدی سئو(با , از هم جدا کنید)")]
        public string SeoKey { get; set; }

    }

    [MetadataType(typeof(Tbl_FutureBusinessPlanMetadata))]
    public partial class Tbl_FutureBusinessPlan
    {

    }

}
