using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels 
{ 
    public class LstUsersAffiliateViewModel
    {
        [Display(Name ="شناسه")]
        public int UserID { get; set; }


        [Display(Name ="نام")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [Display(Name = "موبایل")]
        public string MobileNumber { get; set; }

        [Display(Name = "کد ملی")]
        public string NationalCode { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive  { get; set; }

        [Display(Name = "تاریخ ثبت نام")]
        public DateTime?  RegisterDate { get; set; }

    }
}
