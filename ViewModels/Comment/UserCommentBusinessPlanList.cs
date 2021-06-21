using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class UserCommentBusinessPlanList
    {
        [Display(Name = "ردیف")]
        public int Row_id { get; set; }
        public int CommentBusiness_id { get; set; }
        [Display(Name = "نام طرح")]
        public string BusinessPlanName { get; set; }
        [Display(Name = "نام شرکت")]
        public string CompanyName { get; set; }
        [Display(Name = "وضعیت طرح")]
        public string BusinessPlanStatus { get; set; }
        [Display(Name = "وضعیت نظر")]
        public string BusinessCommentStatus { get; set; }
        [Display(Name = "نظر")]
        public string CommentText { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public DateTime CreateDate { get; set; }
    }
}
