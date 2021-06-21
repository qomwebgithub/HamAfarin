using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Tbl_CommentPlanMetadata
    {
        [Key]
        [Display(Name = "شناسه نظر")]
        public int CommentID { get; set; }
        [Display(Name = "تایید شده")]
        public bool IsActive { get; set; }
        [Display(Name = "حذف شده")]
        public bool IsDeleted { get; set; }
        [Display(Name = "نظر اصلی")]
        public Nullable<int> Parent_id { get; set; }
        [Display(Name = "طرح تجاری")]
        public Nullable<int> BusinessPlan_id { get; set; }
        [Display(Name = "کاربر")]
        public Nullable<int> User_id { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public Nullable<System.DateTime> CreateDate { get; set; }
        [Display(Name = "نظر")]
        public string CommentText { get; set; }

    }
    [MetadataType(typeof(Tbl_CommentPlanMetadata))]
    public partial class Tbl_CommentPlan
    {

    }
}
