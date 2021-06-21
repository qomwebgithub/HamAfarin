using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(11,ErrorMessage ="موبایل ۱۱ رقم می باشد")]
        [MinLength(11,ErrorMessage ="موبایل ۱۱ رقم می باشد")]
        public string MobileNumber { get; set; }
        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(50,ErrorMessage ="حداکثر تعدا کاراکتر 50")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
    }
}
