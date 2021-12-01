using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class InvestmentDetailViewModel
    {
        public string ID { get; set; }
        public int? TypeID { get; set; }
        [Display(Name = "نوع تراکنش")]
        public string TypeName { get; set; }
        public DateTime? Date { get; set; }
        [Display(Name = "تاریخ واریز")]
        public string DateString { get; set; }
        [Display(Name = "مبلغ سود")]
        public long? Amount { get; set; }
        public int? PlanID { get; set; }
        [Display(Name = "نام طرح")]
        public string PlanName { get; set; }
        [Display(Name = "شبای مقصد")]
        public string Sheba { get; set; }
        [Display(Name = "توضیحات")]
        public string Description { get; set; }
    }
}
