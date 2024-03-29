﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Tbl_MenuTypeMetadata
    {
        [Key]
        public int MenuTypeID { get; set; }
        [Display(Name = "فعال")]
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        [Display(Name = "عنوان نوع")]
        public string Title { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public Nullable<System.DateTime> CreateDate { get; set; }
        [Display(Name = "آدرس صفحه")]
        public string Url { get; set; }
    }
    [MetadataType(typeof(Tbl_MenuTypeMetadata))]
    public partial class Tbl_MenuType
    {

    }
}
