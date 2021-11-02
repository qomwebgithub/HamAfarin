using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class InvestmentDetailViewModel
    {
        public int? TypeID { get; set; }
        public string TypeName { get; set; }
        public DateTime? Date { get; set; }
        public string DateString { get; set; }
        public long? Amount { get; set; }
        public int? PlanID { get; set; }
        public string PlanName { get; set; }
    }
}
