using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class SelectPaymentTypeViewModel
    {
        public int BusinessPlanID { get; set; }

        [Display(Name = "نام طرح")]
        public string BussinessName { get; set; }
        [Display(Name = "نام شرکت")]
        public string CompanyName { get; set; }
        [Display(Name = "مبلغ هدف(تومان)")]
        public string AmountRequiredRoRaiseCapital { get; set; }
        [Display(Name = "کد فرابورس")]
        public string CodeOTC { get; set; }
        [Display(Name = "سود طرح")]
        public Nullable<int> PercentageReturnInvestment { get; set; }
        [Display(Name = "مدت طرح")]
        public string FinancialDuration_id { get; set; }

        [Display(Name = "تضامین طرح")]
        public string ImageNameWarranty { get; set; }
        [Display(Name = "نام و نام خانوادگی")]
        public string InvestorFullName { get; set; }
        [Display(Name = "کد ملی")]
        public string InvestorNationalCode { get; set; }
        [Display(Name = "کد سجام")]
        public string InvestorSejamId { get; set; }
        [Display(Name = "شماره موبایل")]
        public string InvestorMobile { get; set; }
        [Display(Name = "قوانین و مقررات و قرارداد سرمایه گذاری را مطالعه نمودم.")]
        public bool SiteRules { get; set; }
        public bool RiskStatement { get; set; }
        public bool InvestmentContract { get; set; }
        public string Privacy { get; set; }
        public string SiteTermsConditions { get; set; }
        [Display(Name = "حداقل میزان سرمایه گذاری")]
        public long MinimumAmountInvest { get; set; }
        [Display(Name = "نوع سرمایه گذاری")]
        public bool IsOnline { get; set; }
        [Display(Name = "درگاه پرداخت")]
        public int Dargah { get; set; }
        [Display(Name = "کل سرمایه گذاری شما")]
        public long TotalInvestment { get; set; }
        [Display(Name = "حداکثر امکان سرمایه گذاری شما")]
        public long CanInvestment { get; set; }
        [Display(Name = "حداکثر میزان سرمایه گذاری")]
        public long MaximumInvestment { get; set; }
        [Display(Name = "مبلغ سرمایه گذاری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public long? OfflinePaymentPrice { get; set; }
        [Display(Name = "مبلغ سرمایه گذاری شده)")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public long? OnlinePaymentPrice { get; set; }
        [Display(Name = "تصویر فیش واریزی")]
        public string PaymentImageName { get; set; }
        [Display(Name = "شماره فیش واریزی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string TransactionPaymentCode { get; set; }
        [Display(Name = "قرارداد سرمایه گذاری")]
        public string ContractFileName { get; set; }
    }
}
