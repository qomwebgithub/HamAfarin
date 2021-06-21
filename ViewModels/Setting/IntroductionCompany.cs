using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ViewModels
{
    public class IntroductionCompanyViewModel
    {
        [Display(Name = "عنوان معرفی شرکت")]
        public string IntroductionCompanyTitle { get; set; }
        [Display(Name = "توضیح کوتاه معرفی شرکت")]
        public string IntroductionCompanyDescription { get; set; }
        [Display(Name = "توضیح کامل شرکت")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string IntroductionCompanyFullText { get; set; }
    }
}
