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
    
    public partial class Tbl_CompanyType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_CompanyType()
        {
            this.Tbl_BussinessPlans = new HashSet<Tbl_BussinessPlans>();
        }
    
        public int CompanyTypeID { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string CompanyTypeTitle { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_BussinessPlans> Tbl_BussinessPlans { get; set; }
    }
}
