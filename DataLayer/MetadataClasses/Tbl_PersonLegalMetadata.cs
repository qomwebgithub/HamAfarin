using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Tbl_PersonLegalMetadata
    {
        [Key]
        public int PersonLegalID { get; set; }
        [Display(Name = "حذف شده")]
        public bool IsDelete { get; set; }
        [Display(Name = "فعال")]
        public bool IsActive { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public Nullable<System.DateTime> CreateDate { get; set; }
        [Display(Name = "شناسه کاربری")]
        public Nullable<int> User_id { get; set; }
        [Display(Name = "نام شرکت")]
        public string CompanyName { get; set; }
        [Display(Name = "کد اقتصادی")]
        public string EconomicCode { get; set; }
        [Display(Name = "شناسه ملی")]
        public string NationalId { get; set; }
        [Display(Name = "شماره ثبت")]
        public string RegistratioNumber { get; set; }
        [Display(Name = "آدرس شرکت")]
        public string Address { get; set; }
    }

    [MetadataType(typeof(Tbl_PersonLegalMetadata))]
    public partial class Tbl_PersonLegal
    {

    }
}
