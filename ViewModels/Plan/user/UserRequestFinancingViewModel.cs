using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class UserRequestFinancingViewModel
    {
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(300, ErrorMessage = "حداکثر 300 کاراکتر میتوانید وارد کنید")]
        public string Title { get; set; }
        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(5000,ErrorMessage ="حداکثر 5000 کاراکتر میتوانید وارد کنید")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Display(Name = "بارگذاری فایل")]
        public string DocumentFile { get; set; }

        public string Status { get; set; }
        public int? Status_id { get; set; }
        [Display(Name = "توضیحات مدیر سایت")]
        public string DescriptionAdmin { get; set; }

    }
}
