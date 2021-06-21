using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class BusinessPlanSingleViewModel
    {
        public int BussinessPlanID { get; set; }
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public string ImageName { get; set; }
        public string ImageNameWarranty { get; set; }
        [Display(Name = "مدت زمان پویش تامین مالی")]
        public string FinancialDuration { get; set; }
        [Display(Name = "متن ویژگی های طرح")]
        public string BusinessPlanFeatures { get; set; }
        [Display(Name = "متن بازار هدف و مزیت های رقابتی")]
        public string MarketTarget { get; set; }
        [Display(Name = "متن ریسک های طرح")]
        public string BusinessPlanRisks { get; set; }
        [Display(Name = "مبلغ مورد نیاز برای جذب سرمایه به تومان")]
        public string AmountRequiredRoRaiseCapital { get; set; }
        public int RemainingTime { get; set; }
        public string RemainingTimeText { get; set; }
        public string PercentageReturnInvestment { get; set; }
        public int PercentageComplate { get; set; }
        public long PriceComplated { get; set; }
        public string WidthPercentage { get; set; }
        public int InvestorCount { get; set; }
        public string BussinessWebSiteAddress { get; set; }
        public string BussinessInstagramAddress { get; set; }
        public string BussinessAparatAddress { get; set; }
        public string BussinessLinkAddress { get; set; }
        public string Location { get; set; }
        public bool IsAcceptInvestment { get; set; }
        public bool IsOverflowInvestment { get; set; }
        public bool IsSuccessBussinessPlan { get; set; }
        public string VideoFileName { get; set; }
        public string FinancialInformationText { get; set; }
        public string ProgressReportText { get; set; }
        public string InvestorsText { get; set; }
    }
}
