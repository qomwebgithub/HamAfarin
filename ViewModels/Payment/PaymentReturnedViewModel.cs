using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class PaymentReturnedViewModel
    {
        [Display(Name = "انتخاب طرح تجاری")]
        public Nullable<int> BusinessPlan_id { get; set; }

        public Nullable<int> PaymentId { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ReasonText { get; set; }
    }
}
