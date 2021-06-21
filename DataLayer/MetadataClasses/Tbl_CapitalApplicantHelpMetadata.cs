using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataLayer
{
    public class Tbl_CapitalApplicantHelpMetadata
    {
        [Key]
        public int CapitalApplicantID { get; set; }
        [Display(Name = "فعال")]
        public bool IsActive { get; set; }
        [Display(Name = "حذف شده")]
        public bool IsDelete { get; set; }
        [Display(Name = "فرایند تقاضای سرمایه")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string CapitalDemandProcess { get; set; }
        [Display(Name = "سوالات متداول")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string FrequentlyAskedQuestions { get; set; }
        [Display(Name = "برگزاری کمپین")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string HoldingCampaign { get; set; }
        [Display(Name = "ارسال طرح")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string SendPlan { get; set; }
    }


    [MetadataType(typeof(Tbl_CapitalApplicantHelpMetadata))]
    public partial class Tbl_CapitalApplicantHelp
    {

    }
}
