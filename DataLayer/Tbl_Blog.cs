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
    
    public partial class Tbl_Blog
    {
        public int BlogID { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public Nullable<int> CreateUser_id { get; set; }
        public string Title { get; set; }
        public string MetaTitle { get; set; }
        public string Description { get; set; }
        public string MetaDescription { get; set; }
        public string FullText { get; set; }
        public string ImageName { get; set; }
        public string ImageAlt { get; set; }
        public string Canonical { get; set; }
        public string TitleUrl { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public bool ShowMainPage { get; set; }
        public Nullable<int> CountVisit { get; set; }
        public string SeoKey { get; set; }
    
        public virtual Tbl_Users Tbl_Users { get; set; }
    }
}