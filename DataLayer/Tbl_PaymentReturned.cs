//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_PaymentReturned
    {
        public int ReturnedID { get; set; }
        public bool IsDelete { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public bool IsConfirm { get; set; }
        public string ReasonText { get; set; }
        public Nullable<int> Payment_id { get; set; }
        public Nullable<int> BusinessPlan_id { get; set; }
        public Nullable<int> User_id { get; set; }
        public Nullable<System.DateTime> ConfirmDate { get; set; }
    
        public virtual Tbl_BusinessPlanPayment Tbl_BusinessPlanPayment { get; set; }
        public virtual Tbl_BussinessPlans Tbl_BussinessPlans { get; set; }
        public virtual Tbl_Users Tbl_Users { get; set; }
    }
}