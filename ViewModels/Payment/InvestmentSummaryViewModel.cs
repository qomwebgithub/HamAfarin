using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
  public  class InvestmentSummaryeViewModel
    {
        [Display(Name = "میزان سرمایه های جذب شده")]
        public long AmountCapitalRaised { get; set; }
        public int InvestmentablePlanCount { get; set; }
        public int InvestmentSuccessPlanCount { get; set; }
        public int InvestmentCountPerson { get; set; }


    }
}
