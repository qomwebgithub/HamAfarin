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
    
    public partial class Tbl_Pages
    {
        public int PageID { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public string ImageName { get; set; }
        public string FullText { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string UrlTitle { get; set; }
    }
}