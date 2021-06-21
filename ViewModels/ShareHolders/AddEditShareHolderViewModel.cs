using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ViewModels
{
    public class AddEditShareHolderViewModel
    {
        [Display(Name = "عنوان سهامدار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ShareHolderTitle { get; set; }
        [Display(Name = "توضیحات مختصر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }
        [Display(Name = "توضیحات کامل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string FullText { get; set; }
        [Display(Name = "لوگو")]
        public string LogoIcon { get; set; }
        [Display(Name = "تصویر")]
        public string ImageName { get; set; }

    }
}
