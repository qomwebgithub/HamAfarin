using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ViewModels
{
    public class FooterViewModel
    {
        public int SettingID { get; set; }
        [Display(Name = "درباره ما")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string AboutUs { get; set; }
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
        [Display(Name = "سوالات متداول")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string CampaignProcess { get; set; }
        [Display(Name = "قوانین و مقررات سایت")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string SiteTermsConditions { get; set; }
        [Display(Name = "حفظ حریم خصوصی")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Privacy { get; set; }
        [Display(Name = "بیانه هشدار ریسک")]
        [DataType(DataType.MultilineText)]
        public string RiskAlertStatement { get; set; }
        [Display(Name = "متن بیانه هشدار ریسک")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string RiskAlertStatementFullText { get; set; }
        [Display(Name = "نمایش بیانیه ریسک")]
        public bool RiskAlertStatementAvtive { get; set; }
    }
}
