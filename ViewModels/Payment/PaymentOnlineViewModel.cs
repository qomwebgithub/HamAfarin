using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class PaymentOnlineViewModel
    {
        [Display(Name = "انتخاب طرح تجاری")]
        public Nullable<int> BusinessPlan_id { get; set; }
        [Display(Name = "مبلغ پرداختی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public Nullable<int> PaymentPrice { get; set; }
    }
}
