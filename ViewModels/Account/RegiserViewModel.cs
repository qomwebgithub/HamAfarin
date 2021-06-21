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
    public class RegiserViewModel
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
        //[Remote("CheckPassword", "Account", HttpMethod = "post", ErrorMessage = "پسورد باید حداقل شامل یک حروف بزرگ , کوچک انگلیسی , یک عدد , یکی از علامت های !@#$%^&* , 8 کاراکتر باشد")]
        //[CustomPasswordValidator(ErrorMessage = "پسورد باید حداقل شامل: یک حروف بزرگ و کوچک انگلیسی - یک عدد - و یکی از علامت های !@#$%^&* - 8 کاراکتر باشد.")]
        public string Password { get; set; }

        [Display(Name = "تکرار کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Password)]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "کلمه های عبور مغایرت دارند")]
        public string RePassword { get; set; }

        [Display(Name = "آیا حقوقی هستید")]
        public bool IsLegal { get; set; }
    }
}
