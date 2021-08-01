using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class VerifySmsViewModel
    {
        public string UserToken { get; set; }
        [Display(Name = "کد تایید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید ")]
        public string SmsCode { get; set; }
        public string ReturnUrl { get; set; }
    }
}
