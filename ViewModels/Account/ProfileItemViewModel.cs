using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using DataLayer;
using System.Threading.Tasks;

namespace ViewModels
{
    public class ProfileItemViewModel
    {
        public int ProfileID { get; set; }
        [Display(Name = "شماره موبایل")]
        public string MobileNumber { get; set; }
        [Display(Name = "نام و نام خانوادگی")]
        public string Name { get; set; }
        //public string ImageName { get; set; }
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }
        [Display(Name = "جنسیت")]
        public string Gender { get; set; }
        [Display(Name = "حقیقی/حقوقی")]
        public string IsLegal { get; set; }
    }
}
