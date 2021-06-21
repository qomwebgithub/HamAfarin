using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Tbl_QuestionPlanMetadata
    {
        [Key]
        [Display(Name = "شناسه سوال")]
        public int QuestionID { get; set; }
        [Display(Name = "تایید شده")]
        public bool IsActive { get; set; }
        [Display(Name = "حذف شده")]
        public bool IsDeleted { get; set; }
        [Display(Name = "سوال اصلی")]
        public Nullable<int> Parent_id { get; set; }
        [Display(Name = "طرح تجاری")]
        public Nullable<int> BusinessPlan_id { get; set; }
        [Display(Name = "کاربر")]
        public Nullable<int> User_id { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public Nullable<System.DateTime> CreateDate { get; set; }
        [Display(Name = "سوال")]
        public string QuestionText { get; set; }


    }
    [MetadataType(typeof(Tbl_QuestionPlanMetadata))]
    public partial class Tbl_BusinessPlanQuestion
    {

    }
}
