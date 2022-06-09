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
    public class ApiRegisterDto
    {
        public string UserName { get; set; }
        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(11, ErrorMessage = "موبایل ۱۱ رقم می باشد")]
        [MinLength(11, ErrorMessage = "موبایل ۱۱ رقم می باشد")]
        public string MobileNumber { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Remote("checknationalcode", "Account", HttpMethod = "post", ErrorMessage = "کد ملی نامعتبر است")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "تعداد کاراکتر مجاز 10 رقم می باشد")]
        public string NationalCode { get; set; }

    }
}
