using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Tbl_InvestmentProcessMetadata
    {
        [Key]
        [Display(Name = "شناسه")] 
        public int InvestmentProcessID { get; set; }
        [Display(Name = "حذف شده")]
        public bool IsDeleted { get; set; }
        [Display(Name = "فعال/غیرفعال")]
        public bool IsActive { get; set; }
        [Display(Name = "عنوان")]
        public string Title { get; set; }
        [Display(Name = "زمان ایجاد")]
        public Nullable<System.DateTime> CreateDate { get; set; }
        [Display(Name = "آدرس")]
        public string Url { get; set; }
        [Display(Name = "تصویر")]
        public string ImageName { get; set; }
        [Display(Name = "جایگاه")]
        public Nullable<int> Sort { get; set; }
    }


    [MetadataType(typeof(Tbl_InvestmentProcessMetadata))]
    public partial class Tbl_InvestmentProcess
    {

    }
}
