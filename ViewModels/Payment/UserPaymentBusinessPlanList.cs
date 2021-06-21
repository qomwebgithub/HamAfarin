using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class UserPaymentBusinessPlanList
    {
        [Display(Name = "ردیف")]
        public int Row_id { get; set; }
        public int PaymentBusine_id { get; set; }
        [Display(Name = "نام طرح")]
        public string BusinessPlanName { get; set; }
        [Display(Name = "نام شرکت")]
        public string CompanyName { get; set; }
        [Display(Name = "تصویر")]
        public string ImageName { get; set; }
        [Display(Name = "مبلغ سرمایه گذاری شده")]
        public long BusinessPlanPayment { get; set; }
        [Display(Name = "وضعیت طرح")]
        public string BusinessPlanStatus { get; set; }
        [Display(Name = "وضعیت مبلغ سرمایه گذاری شده")]
        public string PaymentStatus { get; set; }  
        [Display(Name = "تاریخ سرمایه گذاری")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public DateTime PaymentDate { get; set; }
    }
}
