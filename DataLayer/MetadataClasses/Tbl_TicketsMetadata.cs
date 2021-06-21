using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Tbl_TicketsMetadata
    {
        [Key]
        public int TicketID { get; set; }

        [Display(Name = "حذف شده")]
        public bool IsDelete { get; set; }

        [Display(Name = "بسته شده")]
        public bool IsClosed { get; set; }

        [Display(Name = "تیکت")]
        public Nullable<int> Parent_id { get; set; }

        [Display(Name = "تاریخ بسته شدن")]
        public Nullable<System.DateTime> ClosedDateTime { get; set; }

        [Display(Name = "کاربر")]
        public Nullable<int> User_id { get; set; }

        [Display(Name = "موضوع")]
        [StringLength(100,ErrorMessage ="تعداد کاراکتر بیش از حد مجار است")]
        public string Subject { get; set; }

        [Display(Name = "متن")]
        [StringLength(1000, ErrorMessage = "تعداد کاراکتر بیش از حد مجار است")]
        public string Text { get; set; }

        [Display(Name = "کاربر چک کرده است")]
        public bool IsUserChecked { get; set; }

        [Display(Name = "اپراتور مدیر چک کرده است")]
        public bool IsAdminChecked { get; set; }

        [Display(Name = "تاریخ چک کردن کاربر")]
        public Nullable<System.DateTime> UserCheckedDateTime { get; set; }

        [Display(Name = "تاریخ چک شدن اپراتور")]
        public Nullable<System.DateTime> AdminCheckedDateTime { get; set; }

        [Display(Name = "فایل پیوست")]
        public string AttachFileName { get; set; }

        [Display(Name = "اپراتور چک کننده")]
        public Nullable<int> AdminChecked_id { get; set; }

        [Display(Name = "کاربر ایجاد کننده")]
        public Nullable<int> UserCreate_id { get; set; }


        [Display(Name = "تاریخ ایجاد ")]
        public Nullable<System.DateTime> CreateDateTime { get; set; }


        [Display(Name = "تاریخ حذف")]
        public Nullable<System.DateTime> DeleteDateTime { get; set; }




        [Display(Name = "کاربر حذف کننده")]
        public Nullable<int> DeleteUser_id { get; set; }

        [Display(Name = "تاریخ ویرایش")]
        public Nullable<System.DateTime> EditDateTime { get; set; }

        [Display(Name = "کاربر ویرایش کننده")]
        public Nullable<int> EditUser_id { get; set; }


    }


    [MetadataType(typeof(Tbl_TicketsMetadata))]
    public partial class Tbl_Tickets
    {

    }
}
