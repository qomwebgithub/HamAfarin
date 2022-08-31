using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class ApiTokenViewModel
    {
        [Display(Name = "شناسه")]
        public int ID { get; set; }
        [Display(Name = "شناسه کاربری")]
        public int? UserID { get; set; }
        [Display(Name = "تعداد کاربران")]
        public int UserCount { get; set; }
        [Display(Name = "نام کاربری")]
        public string Username { get; set; }
        [Display(Name = "موبایل")]
        public string Mobile { get; set; }
        [Display(Name = "آدرس بازاریابی")]
        public string Url { get; set; }
        [Display(Name = "نام")]
        public string Name { get; set; }
        [Display(Name = "کل سرمایه گذاری")]
        public long TotalInvestment { get; set; }
    }
}
