using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class AffiliateViewModel
    {

        [Display(Name = "شناسه")]
        public int Id { get; set; }
        [Display(Name = "موبایل")]
        public string Mobile { get; set; }
        [Display(Name = "تاریخ ثبت نام")]
        public DateTime? CreateDate { get; set; }
    }
}
