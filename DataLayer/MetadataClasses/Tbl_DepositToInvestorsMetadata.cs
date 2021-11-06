using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataLayer
{
    public class Tbl_DepositToInvestorsMetadata
    {
        [Key]
        public int DepositID { get; set; }
        public bool IsDelete { get; set; }
        [Display(Name = "پرداخت شده")]
        public bool IsPaid { get; set; }
        public int? Plan_id { get; set; }
        [Display(Name = "نوع واریز")]
        public int? DepositType_id { get; set; }
        [Display(Name = "درصد واریز")]
        public double? YieldPercent { get; set; }
        [Display(Name = "توضیحات")]
        public string Description { get; set; }
        [Display(Name = "تاریخ واریز")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public DateTime? DepositDate { get; set; }
        public int? CreateUser_id { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public DateTime? EditDate { get; set; }
        public int? DeleteUser_id { get; set; }
        public DateTime? DeleteDate { get; set; }
        [Display(Name = "کل مبلغ واریزی")]
        public long? TotalDeposit { get; set; }


    }
    [MetadataType(typeof(Tbl_DepositToInvestorsMetadata))]
    public partial class Tbl_DepositToInvestors
    {

    }
}
