using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ViewModels
{
    public class UserBusinessPlanViewModel
    {
        [Key]
        public int BussinessPlanID { get; set; }
        [Display(Name = "تصویر در لیست")]
        public string ImageNameInListPalns { get; set; }
        [Display(Name = "عنوان(نمایش در سایت)")]
        public string Title { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; }

        [Display(Name = "تایید مدیر")]
        public bool IsConfirmedFromAdmin { get; set; }

        [Display(Name = "مبلغ سرمایه گذاری شده")]
        public string TotalInvestmentPrice { get; set; }

        [Display(Name = "زمان سرمایه گذاری")]
        public string PaidDateTime { get; set; }

        [Display(Name = "نوع پرداخت")]
        public string PaymentType { get; set; }

        [Display(Name = "کد رهگیری")]
        public string TransactionPaymentCode { get; set; }
    }
}
