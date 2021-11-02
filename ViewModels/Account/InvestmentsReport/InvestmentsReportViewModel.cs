using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class InvestmentsReportViewModel
    {
        public List<PlanViewModel> PlansList { get; set; }
        public List<DepositTypeViewModel> InvestmentReportTypes { get; set; }
    }
}
