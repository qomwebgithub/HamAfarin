using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class UserPaymentBusinessPlanSingleViewModel
    {
        public int BusinessPlanID { get; set; }
        public int PaymentID { get; set; }

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
        [Display(Name = "نوع سرمایه گذاری")]
        public bool IsOnline { get; set; }
        [Display(Name = "مبلغ سرمایه گذاری شده")]
        public long BusinessPlanPayment { get; set; }
        [Display(Name = "تصویر فیش واریزی")]
        public string PaymentImageName { get; set; }
        [Display(Name = "کد پیگیری")]
        public string TransactionPaymentCode { get; set; }
        public bool IsRequestedReturn { get; set; }
        public string PaymentStatus { get; set; }
        public int PaymentStatusColorType { get; set; }
        public string InvoiceNumber { get; set; }
        public Nullable<System.DateTime> PaidDateTime { get; set; }
        public Nullable<System.DateTime> AdminCheckDate { get; set; }
        public int RemainingTime { get; set; }
        public string RemainingTimeText { get; set; }
        public int PercentageComplate { get; set; }
        public bool IsSuccessBussinessPlan { get; set; }
        public bool IsAcceptInvestment { get; set; }
        public bool IsOverflowInvestment { get; set; }
        public long PriceComplated { get; set; }
        public string WidthPercentage { get; set; }
        public string BusinessWebSiteAddress { get; set; }
        public string BusinessInstagramAddress { get; set; }
        public string BusinessAparatAddress { get; set; }
        public int InvestorCount { get; set; }
        [Display(Name = "مدت زمان پویش تامین مالی")]
        public string FinancialDuration { get; set; }
        public string ImageName { get; set; }

    }
}
