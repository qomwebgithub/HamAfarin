using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
  public  class InvestmentSummaryViewModel
    {
        [Display(Name = "میزان سرمایه های جذب شده")]
        public long AmountCapitalRaised { get; set; }
        public int ActiveUsers { get; set; }
        public int InvestmentSuccessPlanCount { get; set; }
        public int? InvestmentCountPerson { get; set; }
        public double TotalDepositToInvestors { get; set; }
        public int CountDepositToInvestors { get; set; }


    }
}
