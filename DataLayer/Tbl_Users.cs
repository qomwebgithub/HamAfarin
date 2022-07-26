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
    
    public partial class Tbl_Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_Users()
        {
            this.Tbl_Affiliate = new HashSet<Tbl_Affiliate>();
            this.Tbl_ApiToken = new HashSet<Tbl_ApiToken>();
            this.Tbl_Blog = new HashSet<Tbl_Blog>();
            this.Tbl_BusinessPlanPayment = new HashSet<Tbl_BusinessPlanPayment>();
            this.Tbl_BusinessPlanPayment1 = new HashSet<Tbl_BusinessPlanPayment>();
            this.Tbl_BusinessPlanQuestion = new HashSet<Tbl_BusinessPlanQuestion>();
            this.Tbl_BussinessPlans = new HashSet<Tbl_BussinessPlans>();
            this.Tbl_CommentPlan = new HashSet<Tbl_CommentPlan>();
            this.Tbl_DepositToInvestors = new HashSet<Tbl_DepositToInvestors>();
            this.Tbl_DepositToInvestors1 = new HashSet<Tbl_DepositToInvestors>();
            this.Tbl_DepositToInvestorsDetails = new HashSet<Tbl_DepositToInvestorsDetails>();
            this.Tbl_DepositToInvestorsDetails1 = new HashSet<Tbl_DepositToInvestorsDetails>();
            this.Tbl_PaymentReturned = new HashSet<Tbl_PaymentReturned>();
            this.Tbl_PersonLegal = new HashSet<Tbl_PersonLegal>();
            this.Tbl_RequestFinancing = new HashSet<Tbl_RequestFinancing>();
            this.Tbl_Tickets = new HashSet<Tbl_Tickets>();
            this.Tbl_Tickets1 = new HashSet<Tbl_Tickets>();
            this.Tbl_Tickets2 = new HashSet<Tbl_Tickets>();
            this.Tbl_Tickets3 = new HashSet<Tbl_Tickets>();
            this.Tbl_Tickets4 = new HashSet<Tbl_Tickets>();
            this.Tbl_UserProfiles = new HashSet<Tbl_UserProfiles>();
            this.Tbl_Wallet = new HashSet<Tbl_Wallet>();
        }
    
        public int UserID { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public bool HasSejam { get; set; }
        public bool IsLegal { get; set; }
        public Nullable<int> Role_id { get; set; }
        public string UserName { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public Nullable<int> SmsCode { get; set; }
        public string UserToken { get; set; }
        public string PermanentUserToken { get; set; }
        public string UserType { get; set; }
        public string UserStatus { get; set; }
        public Nullable<System.DateTime> ActivateDate { get; set; }
        public Nullable<System.DateTime> RegisterDate { get; set; }
        public long WalletBalance { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Affiliate> Tbl_Affiliate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_ApiToken> Tbl_ApiToken { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Blog> Tbl_Blog { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_BusinessPlanPayment> Tbl_BusinessPlanPayment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_BusinessPlanPayment> Tbl_BusinessPlanPayment1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_BusinessPlanQuestion> Tbl_BusinessPlanQuestion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_BussinessPlans> Tbl_BussinessPlans { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_CommentPlan> Tbl_CommentPlan { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_DepositToInvestors> Tbl_DepositToInvestors { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_DepositToInvestors> Tbl_DepositToInvestors1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_DepositToInvestorsDetails> Tbl_DepositToInvestorsDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_DepositToInvestorsDetails> Tbl_DepositToInvestorsDetails1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_PaymentReturned> Tbl_PaymentReturned { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_PersonLegal> Tbl_PersonLegal { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_RequestFinancing> Tbl_RequestFinancing { get; set; }
        public virtual Tbl_Roles Tbl_Roles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Tickets> Tbl_Tickets { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Tickets> Tbl_Tickets1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Tickets> Tbl_Tickets2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Tickets> Tbl_Tickets3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Tickets> Tbl_Tickets4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_UserProfiles> Tbl_UserProfiles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Wallet> Tbl_Wallet { get; set; }
    }
}
