using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Tbl_WalletWithdrawalRequestMetadata
    {
        [Key]
        public long ID { get; set; }

        public Nullable<int> User_Id { get; set; }

        [Display(Name = "مقدار برداشت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public long Amount { get; set; }

        [Display(Name = "تاریخ برداشت")]
        public Nullable<System.DateTime> CreateDate { get; set; }

        [Display(Name = "حذف شده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public bool IsDelete { get; set; }

        [Display(Name = "تاریخ حذف")]
        public Nullable<System.DateTime> DeleteDate { get; set; }

        [Display(Name = "وضعیت برداشت")]
        public Nullable<int> WithdrawalStatus { get; set; }

        [Display(Name = "ویرایش تاریخ برداشت")]
        public Nullable<System.DateTime> StatusEditDate { get; set; }
    }

    [MetadataType(typeof(Tbl_WalletWithdrawalRequestMetadata))]
    public partial class Tbl_WalletWithdrawalRequest
    {

    }
}
