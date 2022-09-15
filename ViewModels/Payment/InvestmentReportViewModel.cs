using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class InvestmentReportViewModel
    {
        public int CountOfInvestMent { get; set; }
        public long TotalOfInvestMent { get; set; }
        public int CountOfFirstInvestMent { get; set; }
        public long TotalFirstInvestment { get; set; }
        public int CountOfNotFirstInvestMent { get; set; }
        public long TotalNotFirstInvestment { get; set; }
        public List<InvestorViewModel> FirstInvestores { get; set; }
        public List<InvestorViewModel> NotFirstInvestores { get; set; }

    }
}
