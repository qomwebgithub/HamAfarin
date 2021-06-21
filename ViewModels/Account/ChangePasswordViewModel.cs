using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class ChangePasswordViewModel
    {
        //public string UserName { get; set; }
        //[Display(Name = "نام کاربری (شماره موبایل)")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        //[MaxLength(11, ErrorMessage = "موبایل ۱۱ رقم می باشد")]
        //[MinLength(11, ErrorMessage = "موبایل ۱۱ رقم می باشد")]
        //public string MobileNumber { get; set; }
        [Display(Name = "کلمه عبور قبلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Display(Name = "کلمه عبور جدید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "تکرار کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "کلمه های عبور مغایرت دارند")]
        public string ReNewPassword { get; set; }
    }
}
