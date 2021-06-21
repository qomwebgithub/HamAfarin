using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class AdminCreateEditPaymentViewModel
    {
        [Key]
        public int PaymentID { get; set; }
        [Display(Name = "انتخاب طرح تجاری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public Nullable<int> BusinessPlan_id { get; set; }
        [Display(Name = "پرداخت شده")]
        public bool IsPaid { get; set; }
        [Display(Name = "تایید ادمین")]
        public bool IsConfirmedFromAdmin { get; set; }
        [Display(Name = "تاریخ پرداخت")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public Nullable<System.DateTime> PaidDateTime { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }

        [Display(Name = "تاریخ پرداخت")]
        public string strPaidDateTime { get; set; }
        [Display(Name = "کاربر ایجاد کننده")]
        public Nullable<int> CreateUser_id { get; set; }
        [Display(Name = "کاربر پرداخت کننده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public Nullable<int> PaymentUser_id { get; set; }
        [Display(Name = "کد پیگیری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string TransactionPaymentCode { get; set; }
        [Display(Name = "مبلغ پرداختی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public Nullable<long> PaymentPrice { get; set; }
        [Display(Name = "نوع پرداخت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int PaymentType_id { get; set; }
        [Display(Name = "تصویر واریزی")]
        public string PaymentImageName { get; set; }
        public string InvoiceNumber { get; set; }
        [Display(Name = "وضعیت پرداخت")]
        public Nullable<int> PaymentStatus { get; set; }
        [Display(Name = "تاریخ تایید پرداخت")]
        public string strAdminCheckDate { get; set; }
        [Display(Name = "تاریخ تایید پرداخت")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> AdminCheckDate { get; set; }
        public AdminOnlineDetilsPaymentViewModel OnlineDetails { get; set; }
    }
}
