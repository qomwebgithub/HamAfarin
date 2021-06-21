using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Tbl_UserProfilesMetadata
    {
        [Key]
        public int ProfileID { get; set; }
        [Display(Name = "فعال")]
        public bool IsActive { get; set; }
        [Display(Name = "حذف شده")]
        public bool IsDeleted { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public Nullable<System.DateTime> CreateDate { get; set; }
        [Display(Name = "شناسه کاربری")]
        public Nullable<int> User_id { get; set; }
        [Display(Name = "شماره موبایل")]
        public string MobileNumber { get; set; }
        [Display(Name = "نام")]
        public string FirstName { get; set; }
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }
        [Display(Name = "تصویر")]
        public string ImageName { get; set; }
        [Display(Name = "بیوگرافی")]
        public string Bio { get; set; }
        [Display(Name = "کد ملی")]
        public string NationalCode { get; set; }
        [Display(Name = "نام پدر")]
        public string FatherName { get; set; }
        [Display(Name = "شماره شناسنامه")]
        public string ProfileNationalId { get; set; }
        [Display(Name = "کد سجام")]
        public string SejamCode { get; set; }
        [Display(Name = "شماره حساب")]
        public string AccountNumber { get; set; }
        [Display(Name = "شبای شماره حساب")]
        public string AccountSheba { get; set; }
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
        [Display(Name = "تاریخ تولد")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public Nullable<System.DateTime> BirthDate { get; set; }
        [Display(Name = "جنسیت")]
        public string Gender { get; set; }
    }

    [MetadataType(typeof(Tbl_UserProfilesMetadata))]
    public partial class Tbl_UserProfiles
    {

    }
}
