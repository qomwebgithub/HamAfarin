using System.ComponentModel.DataAnnotations;

namespace ViewModels
{
    public class MobileVerificationApiDto
    {
        public string UserToken { get; set; }

        [Display(Name = "کد تایید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید ")]
        public string VerificationCode { get; set; }

    }
}
