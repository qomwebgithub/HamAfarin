using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class BusinessPlansItemViewModel
    {
        public int BussinessPlanID { get; set; }
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public string ImageName { get; set; }
        public string ShortDescription { get; set; }
        public string AmountRequiredRoRaiseCapital { get; set; }
        public int RemainingTime { get; set; }
        public string RemainingTimeText { get; set; }
        public int PercentageComplate { get; set; }
        public int PriceComplated { get; set; }
        public string widthPercentage { get; set; }
        public int InvestorCount { get; set; }
        public string MarketTarget { get; set; }
        public string CodeOTC { get; set; }
        public int PercentageReturnInvestment { get; set; }

    }
}
