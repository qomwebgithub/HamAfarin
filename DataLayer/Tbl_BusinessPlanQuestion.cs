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
    
    public partial class Tbl_BusinessPlanQuestion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_BusinessPlanQuestion()
        {
            this.Tbl_BusinessPlanQuestion1 = new HashSet<Tbl_BusinessPlanQuestion>();
        }
    
        public int QuestionID { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> Parent_id { get; set; }
        public Nullable<int> BusinessPlan_id { get; set; }
        public Nullable<int> User_id { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string QuestionText { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_BusinessPlanQuestion> Tbl_BusinessPlanQuestion1 { get; set; }
        public virtual Tbl_BusinessPlanQuestion Tbl_BusinessPlanQuestion2 { get; set; }
        public virtual Tbl_BussinessPlans Tbl_BussinessPlans { get; set; }
        public virtual Tbl_Users Tbl_Users { get; set; }
    }
}
