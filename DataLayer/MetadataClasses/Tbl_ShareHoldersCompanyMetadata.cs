using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataLayer
{
    public class Tbl_ShareHoldersCompanyMetadata
    {
        [Display(Name ="سهامدار")]
        public int ShareHolderID { get; set; }

        [Display(Name = "عنوان سهامدار")]
        public string ShareHolderTitle { get; set; }

        [Display(Name = "توضیحات مختصر")]
        public string Description { get; set; }

        [Display(Name = "توضیحات کامل")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string FullText { get; set; }

        [Display(Name = "لوگو")]
        public string LogoIcon { get; set; }

        [Display(Name = "تصویر")]
        public string ImageName { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public Nullable<System.DateTime> CreateDate { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; }

        [Display(Name = "حذف شده")]
        public bool IsDelete { get; set; }
    }

    [MetadataType(typeof(Tbl_ShareHoldersCompanyMetadata))]
    public partial class Tbl_ShareHoldersCompany
    {

    }

}
