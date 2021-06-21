using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataLayer
{
  public  class Tbl_SettingsMetadata
    {
        [Key]
        public int SettingID { get; set; }
        [Display(Name = "عنوان")]
        public string SiteTItle { get; set; }
        [Display(Name = "توضیحات")]
        public string SiteDescription { get; set; }
        [Display(Name = "درباره ما")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string AboutUs { get; set; }
        [Display(Name = "شعار")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Slogen { get; set; }
        [Display(Name = "لوگو")]
        public string SiteLogo { get; set; }
        [Display(Name = "قوانین سایت")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string SiteRules { get; set; }
        [Display(Name = "موافقت نامه های مشارکت")]
        public string PartnershipAgreements { get; set; }
        [Display(Name = "اخطار")]
        public string RiskWarningStatement { get; set; }
        [Display(Name = "آدرس اینستاگرام")]
        public string InstagramUrl { get; set; }
        [Display(Name = "آدرس لینکدین")]
        public string LinkedinUrl { get; set; }
        [Display(Name = "آدرس واتساپ")]
        public string WhatsappUrl { get; set; }
        [Display(Name = "آدرس آپارات")]
        public string AparatUrl { get; set; }
        [Display(Name = "لوکیشن")]
        public string Location { get; set; }
        [Display(Name = "مجوزات")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Permits { get; set; }
        [Display(Name = "تماس با ما")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string ContactUs { get; set; }
        [Display(Name = "فرایند سرمایه گذاری")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string InvestmentProcess { get; set; }
        [Display(Name = "فرایند درخواست سرمایه")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string CapitalApplicationProcess { get; set; }
        [Display(Name = "فرایند برگزاری کمپین")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string CampaignProcess { get; set; }
        [Display(Name = "قوانین و مقررات سایت")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string SiteTermsConditions { get; set; }
        [Display(Name = "حریم خصوصی")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Privacy { get; set; }
        [Display(Name = "بیانیه ریسک")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string RiskAlertStatement { get; set; }
        [Display(Name = "متن بیانیه ریسک")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string RiskAlertStatementFullText { get; set; }
        [Display(Name = "قوانین و مقررات و قرار داد سرمایه گذاری")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string InvestmentRole { get; set; }
        [Display(Name = "عنوان معرفی شرکت")]
        public string IntroductionCompanyTitle { get; set; }
        [Display(Name = " معرفی شرکت")]
        public string IntroductionCompanyDescription { get; set; }
        [Display(Name = " متن معرفی شرکت")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string IntroductionCompanyFullText { get; set; }

    }

    [MetadataType(typeof(Tbl_SettingsMetadata))]
    public partial class Tbl_Settings
    {

    }
}
