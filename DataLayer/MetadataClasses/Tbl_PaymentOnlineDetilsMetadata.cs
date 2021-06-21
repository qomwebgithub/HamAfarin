using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Tbl_PaymentOnlineDetilsMetadata
    {
        [Key]
        public string PaymentDetilsID { get; set; }
        [Display(Name = "حذف شده")]
        public bool IsDelete { get; set; }
        [Display(Name = "پایان یافته")]
        public bool IsFinally { get; set; }
        [Display(Name = "کد پرداخت")]
        public Nullable<int> Payment_id { get; set; }
        [Display(Name = "متن دریافت شاپرک")]
        public string ShaparakMessageGetToken { get; set; }
        [Display(Name = "شناسه شاپرک")]
        public string ShaparakToken { get; set; }
        [Display(Name = "شناسه ارجاع")]
        public string TransactionReferenceID { get; set; }
        [Display(Name = "شناسه ارجاع")]
        public string ShaparakCheckTransactionResult { get; set; }
        [Display(Name = "پرداخت نهایی شاپرک")]
        public string ShaparakVerifyPayment { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public Nullable<System.DateTime> CreateDate { get; set; }
        [Display(Name = "تاریخ نهایی")]
        public Nullable<System.DateTime> FinallyDate { get; set; }
    }
    [MetadataType(typeof(Tbl_PaymentOnlineDetilsMetadata))]
    public partial class Tbl_PaymentOnlineDetils
    {

    }
}
