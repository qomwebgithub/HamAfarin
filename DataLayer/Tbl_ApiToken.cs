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
    
    public partial class Tbl_ApiToken
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_ApiToken()
        {
            this.Tbl_Affiliate = new HashSet<Tbl_Affiliate>();
        }
    
        public int ID { get; set; }
        public Nullable<int> User_Id { get; set; }
        public bool IsDelete { get; set; }
        public string Token { get; set; }
        public string Url { get; set; }
        public string TokenHash { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Affiliate> Tbl_Affiliate { get; set; }
        public virtual Tbl_Users Tbl_Users { get; set; }
    }
}
