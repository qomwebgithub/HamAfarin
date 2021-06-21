using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.MetadataClasses
{
    class Tbl_PaymentReturnedMetadata
    {
        public int ReturnedID { get; set; }
        public bool IsDelete { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public bool IsConfirm { get; set; }
        [Required]
        public string ReasonText { get; set; }
        public Nullable<int> Payment_id { get; set; }
        public Nullable<int> BusinessPlan_id { get; set; }
        public Nullable<int> User_id { get; set; }
        public Nullable<System.DateTime> ConfirmDate { get; set; }

        public virtual Tbl_BusinessPlanPayment Tbl_BusinessPlanPayment { get; set; }
        public virtual Tbl_BussinessPlans Tbl_BussinessPlans { get; set; }
        public virtual Tbl_Users Tbl_Users { get; set; }
    }
    [MetadataType(typeof(Tbl_PaymentReturnedMetadata))]
    public partial class Tbl_PaymentReturned
    {

    }
}
