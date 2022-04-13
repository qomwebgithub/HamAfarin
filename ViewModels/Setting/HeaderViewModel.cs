using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class HeaderViewModel
    {
        [Display(Name = "شعار")]
        public string Slogen { get; set; }
        [Display(Name = "لوگو")]
        public string SiteLogo { get; set; }
        [Display(Name = "بیانیه ریسک")]
        public string RiskWarningStatement { get; set; } 
        [Display(Name = "آدرس اینستاگرام")]
        public string InstagramUrl { get; set; }
        [Display(Name = "آدرس تلگرام")]
        public string TelegramUrl { get; set; }
        [Display(Name = "آدرس لینکدین")]
        public string LinkedinUrl { get; set; }
        [Display(Name = "آدرس واتساپ")]
        public string WhatsappUrl { get; set; }
        [Display(Name = "آدرس آپارات")]
        public string AparatUrl { get; set; }
        [Display(Name = "لوکیشن")]
        public string Location { get; set; }
    }
}
