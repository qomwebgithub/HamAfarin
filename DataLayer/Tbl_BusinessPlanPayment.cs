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
    
    public partial class Tbl_BusinessPlanPayment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_BusinessPlanPayment()
        {
            this.Tbl_PaymentOnlineDetils = new HashSet<Tbl_PaymentOnlineDetils>();
            this.Tbl_PaymentReturned = new HashSet<Tbl_PaymentReturned>();
        }
    
        public int PaymentID { get; set; }
        public bool IsDelete { get; set; }
        public bool IsPaid { get; set; }
        public bool IsConfirmedFromAdmin { get; set; }
        public bool IsReturned { get; set; }
        public Nullable<int> BusinessPlan_id { get; set; }
        public string InvoiceNumber { get; set; }
        public string TransactionPaymentCode { get; set; }
        public Nullable<System.DateTime> PaidDateTime { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> CreateUser_id { get; set; }
        public Nullable<int> PaymentUser_id { get; set; }
        public Nullable<long> PaymentPrice { get; set; }
        public Nullable<int> PaymentType_id { get; set; }
        public string PaymentImageName { get; set; }
        public Nullable<int> PaymentStatus { get; set; }
        public Nullable<System.DateTime> AdminCheckDate { get; set; }
        public bool IsConfirmedFromFaraboors { get; set; }
        public Nullable<System.DateTime> FaraboorsConfirmDate { get; set; }
        public string FaraboorsResponse { get; set; }
    
        public virtual Tbl_BussinessPlans Tbl_BussinessPlans { get; set; }
        public virtual Tbl_PaymentType Tbl_PaymentType { get; set; }
        public virtual Tbl_Users Tbl_Users { get; set; }
        public virtual Tbl_Users Tbl_Users1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_PaymentOnlineDetils> Tbl_PaymentOnlineDetils { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_PaymentReturned> Tbl_PaymentReturned { get; set; }
    }
}
