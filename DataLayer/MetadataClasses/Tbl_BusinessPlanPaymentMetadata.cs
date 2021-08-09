using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Tbl_BusinessPlanPaymentMetadata
    {
        [Key]
        public int PaymentID { get; set; }
        [Display(Name = "انتخاب طرح تجاری")]
        public Nullable<int> BusinessPlan_id { get; set; }
        [Display(Name = "پرداخت شده")]
        public bool IsPaid { get; set; }
        [Display(Name = "تایید ادمین")]
        public bool IsConfirmedFromAdmin { get; set; }
        [Display(Name = "تاریخ پرداخت")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public Nullable<System.DateTime> PaidDateTime { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public Nullable<System.DateTime> CreateDate { get; set; }
        [Display(Name = "کاربر ایجاد کننده")]
        public Nullable<int> CreateUser_id { get; set; }
        [Display(Name = "کاربر پرداخت کننده")]
        public Nullable<int> PaymentUser_id { get; set; }
        [Display(Name = "کد پیگیری")]
        public string TransactionPaymentCode { get; set; }
        [Display(Name = "مبلغ پرداختی")]
        public Nullable<int> PaymentPrice { get; set; }
        [Display(Name = "نوع پرداخت")]
        public int PaymentType_id { get; set; }
        [Display(Name = "تصویر واریزی")]
        public string PaymentImageName { get; set; }

        [Display(Name = "تایید فرابورس")]
        public bool IsConfirmedFromFaraboors { get; set; }
        [Display(Name = "تاریخ تایید فرابورس")]
        public DateTime? FaraboorsConfirmDate { get; set; }
        [Display(Name = "کد دریافتی از فرابورس")]
        public string FaraboorsResponse { get; set; }
    }
    [MetadataType(typeof(Tbl_BusinessPlanPaymentMetadata))]
    public partial class Tbl_BusinessPlanPayment
    {

    }
}
