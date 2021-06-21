using DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ViewModels
{
    public class PlanQuestionItemViewModel
    {
        public int QuestionID { get; set; }
        public Nullable<int> Parent_id { get; set; }
        public Nullable<int> BusinessPlan_id { get; set; }
        public string UserName { get; set; }
        public Nullable<int> User_id { get; set; }
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public Nullable<System.DateTime> CreateDate { get; set; }
        [Display(Name = "سوال شما")]
        [Required(ErrorMessage ="لطفا سوال خود را وارد نمایید!")]
        public string QuestionText { get; set; }
        public List<PlanQuestionItemViewModel> Tbl_BusinessPlanQuestion1 { get; set; }
    }
}