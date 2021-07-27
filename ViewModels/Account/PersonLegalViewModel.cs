using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using DataLayer;
using System.Threading.Tasks;

namespace ViewModels
{
    public class PersonLegalViewModel
    {

        [Display(Name = "نام شرکت")]
        
        [StringLength(100, ErrorMessage = "حداکثر تعدا کاراکتر 100")]
        public string CompanyName { get; set; }
        [Display(Name = "کد اقتصادی")]
        
        [StringLength(100, ErrorMessage = "حداکثر تعدا کاراکتر 100")]
        public string EconomicCode { get; set; }
        
        [Display(Name = "شناسه ملی")]
        [StringLength(100, ErrorMessage = "حداکثر تعدا کاراکتر 100")]
        public string NationalId { get; set; }
        
        [Display(Name = "شماره ثبت")]
        [StringLength(100, ErrorMessage = "حداکثر تعدا کاراکتر 100")]
        public string RegistratioNumber { get; set; }
        [Display(Name = "آدرس شرکت")]
        
        [StringLength(500, ErrorMessage = "حداکثر تعدا کاراکتر 500")]
        public string Address { get; set; }

        [Display(Name = "بارگذاری فایل")]
        public string LegalFile { get; set; }
    }
}
