using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataLayer
{
    public class Tbl_SmsMetadata
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "متن پیامک")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }

        [Display(Name = "تاریخ آخرین ویرایش")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public DateTime? EditDate { get; set; }


    }
    [MetadataType(typeof(Tbl_SmsMetadata))]
    public partial class Tbl_Sms
    {

    }
}
