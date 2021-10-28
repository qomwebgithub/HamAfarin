using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class InvestorViewModel
    {
        public int UserID { get; set; }
        public long TotalPaymentPrice { get; set; }
        public long DepositAmount { get; set; }
        public string MobileNumber { get; set; }
        public string Shaba { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
