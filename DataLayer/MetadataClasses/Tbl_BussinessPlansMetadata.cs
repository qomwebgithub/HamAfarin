using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataLayer
{
    public class Tbl_BussinessPlansMetadata
    {
        [Key]
        public int BussinessPlanID { get; set; }
        public bool IsDeleted { get; set; }
        [Display(Name = "فعال")]
        public bool IsActive { get; set; }
        [Display(Name = "کاربر")]
        public Nullable<int> User_id { get; set; }
        [Display(Name = "عنوان(نمایش در سایت)")]
        public string Title { get; set; }

        [Display(Name = "عکس ضمانت نامه")]
        public string ImageNameWarranty { get; set; }
        [Display(Name = "تصویر در لیست")]
        public string ImageNameInListPalns { get; set; }
        [Display(Name = "تصویر در نمایش طرح")]
        public string ImageNameInSinglePlan { get; set; }
        [Display(Name = "توضیح کوتاه(نمایش در سایت)")]
        [AllowHtml]
        public string ShortDescription { get; set; }
        [Display(Name = "تصویر لوگوی طرح")]
        public string BussinessLogoImageName { get; set; }
        [Display(Name = "(کاربر)نام طرح")]
        public string BussinessName { get; set; }
        [Display(Name = "(توضیحات کاربر برای مدیر)توضیح مختصر")]
        public string BussinessSummaryDescription { get; set; }
        [Display(Name = "زمینه کسب و کار")]
        public Nullable<int> BussinessField_id { get; set; }
        [Display(Name = "تاریخ شروع سرمایه گذاری")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> InvestmentStartDate { get; set; }
        [Display(Name = "تاریخ پایان سرمایه گذاری")]
        public Nullable<System.DateTime> InvestmentExpireDate { get; set; }
        [Display(Name = "نام شرکت")]
        public string CompanyName { get; set; }
        [Display(Name = "نوع شرکت")]
        public Nullable<int> CompanyType_id { get; set; }
        [Display(Name = "شماره ثبت شرکت")]
        public string CompanyRegisterCode { get; set; }
        [Display(Name = "تاریخ ثبت شرکت")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public Nullable<System.DateTime> CompanyRegisterDate { get; set; }
        [Display(Name = "شناسنامه ملی شرکت")]
        public string CompanyNationalCertificateCode { get; set; }
        [Display(Name = "کد اقتصادی")]
        public string CompanyEconomicCode { get; set; }
        [Display(Name = "نام و نام خانوادگی نماینده شرکت")]
        public string CompanyAgentFullName { get; set; }
        [Display(Name = "سمت نماینده شرکت")]
        public string CompanyAgentRole { get; set; }
        [Display(Name = "شماره تماس نماینده شرکت")]
        public string CompanyAgentPhoneNumber { get; set; }
        [Display(Name = "ایمیل نماینده شرکت")]
        public string CompanyAgentEmail { get; set; }
        [Display(Name = "بارگذاری نامه معرفی نماینده شرکت")]
        public string CompanyIntroductionLetterFileName { get; set; }
        [Display(Name = "تصویر کد ملی نماینده شرکت")]
        public string CompanyAgentNationalCardImageName { get; set; }
        [Display(Name = "آدرس ثبتی شرکت")]
        public string CompanyRegisterAddress { get; set; }
        [Display(Name = "کد پستی شرکت")]
        public string CompanyPostalCode { get; set; }
        [Display(Name = "شهر شرکت")]
        public string CompanyCity { get; set; }
        [Display(Name = "مدت زمان پویش تامین مالی")]
        public Nullable<int> FinancialDuration_id { get; set; }
        [Display(Name = "آیا آدرس ثبتی شرکت با آدرس محل فعالیت تفاوت دارد؟")]
        public bool IsDifferentActiveAddressWithRegisterAddress { get; set; }
        [Display(Name = "آدرس محل فعالیت")]
        public string CompanyActiveAddress { get; set; }
        [Display(Name = "کد پستی")]
        public string CompanyActivePostalCode { get; set; }
        [Display(Name = "آیا دانش بنیان ثبت شده است؟")]
        public bool IsDaneshBonyan { get; set; }
        [Display(Name = "لوکیشن شرکت")]
        public string Location { get; set; }
        [Display(Name = "آدرس تارنمای کسب و کار")]
        public string BussinessWebSiteAddress { get; set; }
        [Display(Name = "آدرس اینستاگرام کسب و کار")]
        public string BussinessInstagramAddress { get; set; }
        [Display(Name = "آدرس آپارات کسب و کار")]
        public string BussinessAparatAddress { get; set; }
        [Display(Name = "متن ریسک های طرح")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string BusinessPlanRisks { get; set; }
        [Display(Name = ")توسط کاربر وارد میشود) متن ریسک های طرح")]
        public string BusinessPlanRisksUser { get; set; }
        [Display(Name = "بارگذاری مدل کسب و کار")]
        public string BussinessModelFileName { get; set; }
        [Display(Name = "بارگذاری ویدیو معرفی ایده")]
        public string IntroductionIdeaVideoFileName { get; set; }
        [Display(Name = "متن بازار هدف و مزیت های رقابتی")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string MarketTarget { get; set; }
        [Display(Name = "هزینه ها")]
        public string Coasts { get; set; }
        [Display(Name = "مزیت و معایب رقابتی کسب و کار")]
        public string CompetitiveAdvantagesAndDisadvantages { get; set; }
        [Display(Name = "بارگذاری ارائه اسلایدی")]
        public string SlideShowPresentationFileName { get; set; }
        [Display(Name = "بارگذاری اسناد و گزارش ها")]
        public string DocumentsAndReportsFileName { get; set; }
        [Display(Name = "حداکثر درصد برای سرمایه گذاری")]
        public Nullable<int> MaximumInvestmentPercentage { get; set; }
        [Display(Name = "حداقل مبلغ برای سرمایه گذاری")]
        public string MinimumAmountInvest { get; set; }
        [Display(Name = "واحد پول")]
        public Nullable<int> MonetaryUnit_id { get; set; }
        [Display(Name = "مبلغ مورد نیاز برای جذب سرمایه به تومان")]
        public string AmountRequiredRoRaiseCapital { get; set; }
        [Display(Name = "اجازه سرمایه گذاری بیش از صد در صد")]
        public bool IsOverflowInvestment { get; set; }
        [Display(Name = "آیا درگذشته برای کسب و کار خود جذب سرمایه داشته اید؟")]
        public bool HaveYouRaisedCapitalPrevious { get; set; }
        [Display(Name = "میزان سرمایه گذاری گذشته")]
        public string InvestmentAmountPrevious { get; set; }
        [Display(Name = "میزان مشارکت به درصد")]
        public string PercentageOfSharesCapitalPrevious { get; set; }
        [Display(Name = "نام سرمایه گذار")]
        public string PreviousInvestorFullName { get; set; }
        [Display(Name = "نوع سرمایه گذاری")]
        public string PreviousInvestorType { get; set; }
        [Display(Name = "نرخ بازگشت سرمایه به درصد")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public Nullable<int> PercentageReturnInvestment { get; set; }
        [Display(Name = "تاریخ سرمایه گذاری قبلی")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public Nullable<System.DateTime> PreviousInvestorDate { get; set; }
        [Display(Name = "پایان اعتبار سرمایه گذاری قبلی")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public Nullable<System.DateTime> PreviousInvestorExpireDate { get; set; }
        [Display(Name = "متن ویژگی های طرح")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string BusinessPlanFeatures { get; set; }
        [Display(Name = "اطلاعات مالی")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string FinancialInformationText { get; set; }
        [Display(Name = "گزارش پیشرفت")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string ProgressReportText { get; set; }
        [Display(Name = "سرمایه گذاران")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string InvestorsText { get; set; } 
        [Display(Name = "دلایل تمدید طرح")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string ReasonsExtendingPlan { get; set; }
    }
    [MetadataType(typeof(Tbl_BussinessPlansMetadata))]
    public partial class Tbl_BussinessPlans
    {

    }
}
