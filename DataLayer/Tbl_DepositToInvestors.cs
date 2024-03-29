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
    
    public partial class Tbl_DepositToInvestors
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_DepositToInvestors()
        {
            this.Tbl_DepositToInvestorsDetails = new HashSet<Tbl_DepositToInvestorsDetails>();
        }
    
        public int DepositID { get; set; }
        public bool IsDelete { get; set; }
        public bool IsPaid { get; set; }
        public Nullable<int> Plan_id { get; set; }
        public Nullable<int> DepositType_id { get; set; }
        public Nullable<double> YieldPercent { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> DepositDate { get; set; }
        public Nullable<int> CreateUser_id { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> PaidDate { get; set; }
        public Nullable<System.DateTime> EditDate { get; set; }
        public Nullable<int> DeleteUser_id { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public Nullable<long> TotalDeposit { get; set; }
    
        public virtual Tbl_BussinessPlans Tbl_BussinessPlans { get; set; }
        public virtual Tbl_DepositTypes Tbl_DepositTypes { get; set; }
        public virtual Tbl_Users Tbl_Users { get; set; }
        public virtual Tbl_Users Tbl_Users1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_DepositToInvestorsDetails> Tbl_DepositToInvestorsDetails { get; set; }
    }
}
