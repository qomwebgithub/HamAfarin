using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Tbl_RequestFinancingMetadata
    {
        [Key]
        public int ID { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<int> User_id { get; set; }
        public Nullable<int> Status_id { get; set; }
        [Display(Name = "عنوان")]
        public string Title { get; set; }
        [Display(Name = "توضیحات")]
        public string Description { get; set; }
        [Display(Name = "فایل مدارک")]
        public string DocumentFile { get; set; }
        [Display(Name = "تاریخ درخواست")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public Nullable<System.DateTime> CreateDate { get; set; }
        [Display(Name = "تاریخ بررسی شده")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public Nullable<System.DateTime> CheckDate { get; set; }
        [Display(Name = "توضیحات ادمین")]
        public string DescriptionAdmin { get; set; }
        [Display(Name = "توضیحات مخفی ادمین")]
        public string DescriptionAdminHidden { get; set; }
    }
    [MetadataType(typeof(Tbl_RequestFinancingMetadata))]
    public partial class Tbl_RequestFinancing
    {

    }
}
