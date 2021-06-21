using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using DataLayer;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Mvc;

namespace ViewModels
{
    public class ProfileViewModel
    {
        [Key]
        public int ProfileID { get; set; }

        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(11, ErrorMessage = "{0} ۱۱ رقم می باشد")]
        [MinLength(11, ErrorMessage = "{0} ۱۱ رقم می باشد")]
        public string MobileNumber { get; set; }
        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(50, ErrorMessage = "حداکثر تعدا کاراکتر 50")]
        public string FirstName { get; set; }
        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(50, ErrorMessage = "حداکثر تعدا کاراکتر 50")]
        public string LastName { get; set; }
        [Display(Name = "تصویر")]
        public string ImageName { get; set; }
        [Display(Name = "توضیحات بیشتر")]
        [StringLength(500, ErrorMessage = "حداکثر تعدا کاراکتر 500")]
        public string Bio { get; set; }
        [Display(Name = "نام پدر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(50, ErrorMessage = "حداکثر تعدا کاراکتر 50")]
        public string FatherName { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        //[MaxLength(11, ErrorMessage = "{0} ۱۰ رقم می باشد")]
        [Display(Name = "شماره شناسنامه")]
        public string ProfileNationalId { get; set; }
        [Display(Name = "کد ملی")]
        public string NationalCode { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "کد سجام")]
        [StringLength(100, ErrorMessage = "حداکثر تعدا کاراکتر 100")]
        public string SejamCode { get; set; }
        [Display(Name = "شماره حساب")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(100, ErrorMessage = "حداکثر تعدا کاراکتر 100")]
        public string AccountNumber { get; set; }
        [Display(Name = "شبای شماره حساب")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(100, ErrorMessage = "حداکثر تعدا کاراکتر 100")]
        public string AccountSheba { get; set; }
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [EmailAddress(ErrorMessage = " ایمیل وارد شده معتبر نیست")]
        [StringLength(200, ErrorMessage = "حداکثر تعدا کاراکتر 200")]
        public string Email { get; set; }
        [Display(Name = "تاریخ تولد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public Nullable<System.DateTime> BirthDate { get; set; }
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        //[Display(Name = "تاریخ تولد")]
        //public string strBirthDate { get; set; }
    }
}
