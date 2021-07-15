using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Tbl_UsersMetadata
    {
        [Key]
        [Display(Name = "شناسه کاربر")]
        public int UserID { get; set; }
        [Display(Name = "حذف شده")]

        public bool IsDeleted { get; set; }

        [Display(Name = "تاییدیه سجام")]
        public bool HasSejam { get; set; }

        [Display(Name = "تایید شده")]
        public bool IsActive { get; set; }
        [Display(Name = "حقوقی هستید؟")]
        public bool IsLegal { get; set; }
        [Display(Name = "سمت")]
        public Nullable<int> Role_id { get; set; }
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }
        [Display(Name = "شماره موبایل")]
        public string MobileNumber { get; set; }
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }
        [Display(Name = "کد پیامک شده")]
        public Nullable<int> SmsCode { get; set; }
        [Display(Name = "تاریخ عضویت")]
        public Nullable<System.DateTime> RegisterDate { get; set; }
        [Display(Name = "تاریخ تایید")]
        public Nullable<System.DateTime> ActivateDate { get; set; }
        [Display(Name = "نقش کاربر")]
        public virtual Tbl_Roles Tbl_Roles { get; set; }
    }


    [MetadataType(typeof(Tbl_UsersMetadata))]
    public partial class Tbl_Users
    {

    }
}
