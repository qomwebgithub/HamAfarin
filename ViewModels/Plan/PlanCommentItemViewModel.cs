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
    public class PlanCommentItemViewModel
    {
        public int CommentID { get; set; }
        public Nullable<int> Parent_id { get; set; }
        public Nullable<int> BusinessPlan_id { get; set; }
        public string UserName { get; set; }
        public Nullable<int> User_id { get; set; }
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public Nullable<System.DateTime> CreateDate { get; set; }
        [Display(Name = "نظر شما")]
        [Required(ErrorMessage = "لطفا نظر خود را وارد نمایید!")]
        public string CommentText { get; set; }
        public List<PlanCommentItemViewModel> Tbl_CommentPlan1 { get; set; }
    }
}