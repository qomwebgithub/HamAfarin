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
    
    public partial class Tbl_PersonLegal
    {
        public int PersonLegalID { get; set; }
        public bool IsDelete { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> User_id { get; set; }
        public string CompanyName { get; set; }
        public string EconomicCode { get; set; }
        public string NationalId { get; set; }
        public string RegistratioNumber { get; set; }
        public string Address { get; set; }
        public string LegalFile { get; set; }
    
        public virtual Tbl_Users Tbl_Users { get; set; }
    }
}
