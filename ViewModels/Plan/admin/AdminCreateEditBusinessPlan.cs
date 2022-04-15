using DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages;
using System.Web;
using System.Web.Mvc;

namespace ViewModels
{
    public class AdminCreateEditBusinessPlan
    {
        [Key]
        public int BussinessPlanID { get; set; }
        public bool IsDeleted { get; set; }
        [Display(Name = "فعال")]
        public bool IsActive { get; set; }
        [Display(Name = "کاربر")]
        public int? User_id { get; set; }
        [Display(Name = "عنوان(نمایش در سایت)")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }

        [Display(Name = "عکس ضمانت نامه")]
        public string ImageNameWarranty { get; set; }
        [Display(Name = "تصویر در لیست")]
        public string ImageNameInListPalns { get; set; }
        [Display(Name = "تصویر در نمایش طرح")]
        public string ImageNameInSinglePlan { get; set; }
        [Display(Name = "گالری تصاویر")]
        public List<Tbl_BusinessPlanGallery> GalleryPlan { get; set; }
        [Display(Name = "توضیح کوتاه(نمایش در سایت)")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ShortDescription { get; set; }
        [Display(Name = "تصویر لوگوی طرح")]
        public string BussinessLogoImageName { get; set; }
        [Display(Name = "(کاربر)نام طرح")]
        public string BussinessName { get; set; }
        [Display(Name = "(توضیحات کاربر برای مدیر)توضیح مختصر")]
        public string BussinessSummaryDescription { get; set; }
        [Display(Name = "زمینه کسب و کار")]
        public int? BussinessField_id { get; set; }
        [Display(Name = "تاریخ شروع سرمایه گذاری")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public DateTime? InvestmentStartDate { get; set; }
        [Display(Name = "تاریخ شروع سرمایه گذاری")]
        public string strInvestmentStartDate
        {
            get => InvestmentStartDate?.ToString("yyyy-MM-dd");
            set { InvestmentStartDate = Common.StringExtensions.StringToDate(value); }
        }
        [Display(Name = "تاریخ پایان سرمایه گذاری")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? InvestmentExpireDate { get; set; }
        [Display(Name = "تاریخ پایان سرمایه گذاری")]
        public string strInvestmentExpireDate
        {
            get => InvestmentExpireDate?.ToString("yyyy-MM-dd");
            set { InvestmentExpireDate = Common.StringExtensions.StringToDate(value); }
        }
        [Display(Name = "نام شرکت")]
        public string CompanyName { get; set; }
        [Display(Name = "نوع شرکت")]
        public int? CompanyType_id { get; set; }
        [Display(Name = "شماره ثبت شرکت")]
        public string CompanyRegisterCode { get; set; }
        [Display(Name = "تاریخ ثبت شرکت")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public DateTime? CompanyRegisterDate { get; set; }
        [Display(Name = "تاریخ ثبت شرکت")]
        public string strCompanyRegisterDate
        {
            get => CompanyRegisterDate?.ToString("yyyy-MM-dd");
            set { CompanyRegisterDate = Common.StringExtensions.StringToDate(value); }
        }
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
        public int? FinancialDuration_id { get; set; }
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
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
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
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int? MaximumInvestmentPercentage { get; set; }
        [Display(Name = "حداقل مبلغ برای سرمایه گذاری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string MinimumAmountInvest { get; set; }
        [Display(Name = "واحد پول")]
        public int? MonetaryUnit_id { get; set; }
        [Display(Name = "مبلغ مورد نیاز برای جذب سرمایه به تومان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
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
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int? PercentageReturnInvestment { get; set; }
        [Display(Name = "تاریخ سرمایه گذاری قبلی")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public DateTime? PreviousInvestorDate { get; set; }
        [Display(Name = "تاریخ سرمایه گذاری قبلی")]
        public string strPreviousInvestorDate
        {
            get => PreviousInvestorDate?.ToString("yyyy-MM-dd");
            set { PreviousInvestorDate = Common.StringExtensions.StringToDate(value); }
        }
        [Display(Name = "پایان اعتبار سرمایه گذاری قبلی")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public DateTime? PreviousInvestorExpireDate { get; set; }
        [Display(Name = "پایان اعتبار سرمایه گذاری قبلی")]
        public string strPreviousInvestorExpireDate
        {
            get => PreviousInvestorExpireDate?.ToString("yyyy-MM-dd");
            set { PreviousInvestorExpireDate = Common.StringExtensions.StringToDate(value); }
        }
        [Display(Name = "متن ویژگی های طرح")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string BusinessPlanFeatures { get; set; }
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public DateTime? CreateDate { get; set; }
        [Display(Name = "کد فرابورس")]
        public string CodeOTC { get; set; }
        [Display(Name = "آدرس طرح در فرابورس")]
        public string PlanInFarabourseUrl { get; set; }
        [Display(Name = "آی دی پروژه فرابورس")]
        public string FaraboorsProjectId { get; set; }
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
        public bool IsSuccessBussinessPlan { get; set; }
        [Display(Name = "دلایل تمدید طرح")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string ReasonsExtendingPlan { get; set; }

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

        [Display(Name = "قرارداد سرمایه گذاری")]
        public string ContractFileName { get; set; }
        [Display(Name = "گواهی شراکت")]
        public bool IsProjectParticipationReady { get; set; }
        public HttpPostedFileBase ImageInListPalnsFile { get; set; }
        public HttpPostedFileBase ImageInSinglePlanFile { get; set; }
        public HttpPostedFileBase[] GalleryPlanFiles { get; set; }
        public HttpPostedFileBase ImageLogoFile { get; set; }
        public HttpPostedFileBase ImageWarrantyFile { get; set; }
        public HttpPostedFileBase ImageNationalCardFile { get; set; }
        public HttpPostedFileBase LetterFile { get; set; }
        public HttpPostedFileBase ModelFile { get; set; }
        public HttpPostedFileBase SlideFile { get; set; }
        public HttpPostedFileBase ReportFile { get; set; }
        public HttpPostedFileBase IdeaFile { get; set; }
        public HttpPostedFileBase ContractFile { get; set; }

    }
}
