using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataLayer
{
    public class Tbl_InvestorMetadata
    {
        [Key]
        public int InvestorID { get; set; }
        [Display(Name = "فعال")]
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        [Display(Name = "عنوان")]
        public string Title { get; set; }
        [Display(Name = "توضیح")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Description { get; set; }
        [Display(Name = "تصویر")]
        public string ImageName { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public Nullable<System.DateTime> CreateDate { get; set; }
        [Display(Name = "جایگاه")]
        public Nullable<int> Sort { get; set; }
    }
    [MetadataType(typeof(Tbl_InvestorMetadata))]
    public partial class Tbl_Investor
    {

    }
}
