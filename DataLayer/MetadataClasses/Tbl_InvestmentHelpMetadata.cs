using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataLayer
{
    public class Tbl_InvestmentHelpMetadata
    {
        [Key]
        public int InvestmentHelpID { get; set; }
        [Display(Name = "فعال")]
        public bool IsActive { get; set; }
        [Display(Name = "حذف شده")]
        public bool IsDelete { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public Nullable<System.DateTime> CreateDate { get; set; }
        [Display(Name = "عنوان")]
        public string InvestmentHelpTitle { get; set; }
        [Display(Name = "توضیحات مختصر")]
        public string Description { get; set; }
        [Display(Name = "توضیحات کامل")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string FullText { get; set; }
        [Display(Name = "سوالات متداول")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string FrequentlyAskedQuestions { get; set; }
        [Display(Name = "فرایند سرمایه گذاری")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string InvestmentProcess { get; set; }
        [Display(Name = "برگزاری کمپین")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string HoldingCampaign { get; set; }
        [Display(Name = "ارسال طرح")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string SendPlan { get; set; }

    }

    [MetadataType(typeof(Tbl_InvestmentHelpMetadata))]
    public partial class Tbl_InvestmentHelp
    {

    }

}
