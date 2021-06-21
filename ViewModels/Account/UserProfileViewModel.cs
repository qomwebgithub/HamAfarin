using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using DataLayer;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ViewModels
{
    public class UserProfileViewModel
    {
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }
        [Display(Name = "فعال")]
        public bool IsActive { get; set; }
        [Display(Name = " مایل به تکمیل اطلاعات حقوقی برای سرمایه گذاری سازمانی هستم. ")]
        public bool IsLegal { get; set; }

        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "جنسیت")]
        public int Gender { get; set; }

        [Display(Name = "جنسیت")]
        public string strGender { get; set; }

        [Display(Name = "تاریخ تولد")]
        public string strBirthDate { get; set; }
        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Remote("checknationalcode", "UserProfile", HttpMethod = "post", ErrorMessage = "کد ملی نامعتبر است")]
        //[Remote("DuplicateNationalCode", "Account", AdditionalFields = "LastNationalCode", HttpMethod = "POST", ErrorMessage = "کد ملی تکراری می باشد")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "تعداد کاراکتر مجاز 10 رقم می باشد")]
        public string NationalCode { get; set; }
        public ProfileViewModel Profile { get; set; }
        public int Profile_id { get; set; }

        public PersonLegalViewModel PersonLegal { get; set; }
    }
}
