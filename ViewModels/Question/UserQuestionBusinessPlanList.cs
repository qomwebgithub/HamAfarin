using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class UserQuestionBusinessPlanList
    {
        [Display(Name = "ردیف")]
        public int Row_id { get; set; }
        public int QuestionBusiness_id { get; set; }
        [Display(Name = "نام طرح")]
        public string BusinessPlanName { get; set; }
        [Display(Name = "نام شرکت")]
        public string CompanyName { get; set; }
        [Display(Name = "وضعیت طرح")]
        public string BusinessPlanStatus { get; set; }
        [Display(Name = "وضعیت سوال")]
        public string BusinessQuestionStatus { get; set; }
        [Display(Name = "سوال")]
        public string QuestionText { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public DateTime CreateDate { get; set; }
    }
}
