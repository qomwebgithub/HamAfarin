using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class ProjectParticipationViewModel
    {
        [Key]
        public int PaymentID { get; set; }

        [Display(Name = "عنوان(نمایش در سایت)")]
        public string Title { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public Nullable<System.DateTime> CreateDate { get; set; }

        [Display(Name = "مبلغ پرداختی")]
        public Nullable<long> PaymentPrice { get; set; }
    }
}
