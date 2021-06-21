using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class AdminOnlineDetilsPaymentViewModel
    {
        public string PaymentDetilsID { get; set; }
        [Display(Name = "نهایی شده")]
        public bool IsFinally { get; set; }
        [Display(Name = "شناسه شاپرک")]
        public string ShaparakToken { get; set; }
        [Display(Name = "شناسه ارجاع")]
        public string TransactionReferenceID { get; set; }
        [Display(Name = "شناسه ارجاع شاپرک")]
        public string ShaparakCheckTransactionResult { get; set; }
        [Display(Name = "پرداخت نهایی شاپرک")]
        public string ShaparakVerifyPayment { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public Nullable<System.DateTime> CreateDate { get; set; }
        [Display(Name = "تاریخ نهایی")]
        public Nullable<System.DateTime> FinallyDate { get; set; }
    }
}
