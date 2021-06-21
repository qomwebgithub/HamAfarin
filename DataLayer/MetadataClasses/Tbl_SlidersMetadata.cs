using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Tbl_SlidersMetadata
    {
        [Key]
        public int SliderID { get; set; }
        public bool IsDeleted { get; set; }
        [Display(Name = "نشان دادن")]
        public bool IsActive { get; set; }
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }
        [Display(Name = "تصویر")]
        public string ImageName { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        [Display(Name = "آدرس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Url]
        public string Url { get; set; }
        [Display(Name = "نمایش در صفحه اصلی")]
        public bool IsShowHomePage { get; set; }
        [Display(Name = "صفحه نمایش")]
        public Nullable<int> Page_id { get; set; }
        [Display(Name = "عنوان کلیک 1")]
        public string TitleClick1 { get; set; }
        [Display(Name = "لینک کلیک 1")]
        public string LinkClick1 { get; set; }
        [Display(Name = "فعال بودن کلکی 1")]
        public bool IsActiveClick1 { get; set; }
        [Display(Name = "عنوان کلیک 2")]
        public string TitleClick2 { get; set; }
        [Display(Name = "لینک کلیک 2")]
        public string LinkClick2 { get; set; }
        [Display(Name = "فعال بودن کلکی 2")]
        public bool IsActiveClick2 { get; set; }
    }

    [MetadataType(typeof(Tbl_SlidersMetadata))]
    public partial class Tbl_Sliders
    {

    }
}
