using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
  public  class PaymentOfflineViewModel
    {
        [Display(Name = "انتخاب طرح تجاری")]
        public Nullable<int> BusinessPlan_id { get; set; }
        [Display(Name = "تاریخ پرداخت")]
        public Nullable<System.DateTime> PaidDateTime { get; set; }
        [Display(Name = "کد پیگیری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string TransactionPaymentCode { get; set; }
        [Display(Name = "مبلغ پرداختی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public Nullable<int> PaymentPrice { get; set; }
        [Display(Name = "تصویر واریزی")]
        public string PaymentImageName { get; set; }

    }
}
