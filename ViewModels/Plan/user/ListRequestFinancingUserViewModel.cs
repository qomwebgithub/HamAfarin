using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class ListRequestFinancingUserViewModel
    {
        public int Row { get; set; }
        public int RequestFinancingID { get; set; }
        public string MobileNumber { get; set; }
        public string FullName { get; set; }
        public string CompanyName { get; set; }
        public string Title { get; set; }
        [DisplayFormat(DataFormatString = "{0: yyyy/mm/dd}")]
        public DateTime RequestDate { get; set; }

        public string Status { get; set; }
    }
}
