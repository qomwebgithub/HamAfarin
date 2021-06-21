using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ViewModels
{
    public class SejamLoginViewModel
    {
        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Remote("checknationalcode", "Account", HttpMethod = "post", ErrorMessage = "کد ملی نامعتبر است")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "تعداد کاراکتر مجاز 10 رقم می باشد")]
        public string NationalCode { get; set; }
        public string ReturnUrl { get; set; }
    }
}
