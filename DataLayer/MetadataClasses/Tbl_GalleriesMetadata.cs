using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Tbl_GalleriesMetadata
    {
        [Key]
        public int GalleryID { get; set; }
        [Display(Name = "تصویر")]
        public string ImageName { get; set; }
        [Display(Name = "آدرس تصویر")]
        public string ImageUrl { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public Nullable<System.DateTime> CreateDate { get; set; }

    }
    [MetadataType(typeof(Tbl_GalleriesMetadata))]
    public partial class Tbl_Galleries
    {

    }
}
