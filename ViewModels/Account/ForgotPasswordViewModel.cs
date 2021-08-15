using HamAfarin.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ViewModels
{
    public class ForgotPasswordViewModel
    {
        public string UserName { get; set; }
        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(11, ErrorMessage = "موبایل ۱۱ رقم می باشد")]
        [MinLength(11, ErrorMessage = "موبایل ۱۱ رقم می باشد")]
        public string MobileNumber { get; set; }
    }
}
