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
    
    public partial class Tbl_Sliders
    {
        public int SliderID { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string Url { get; set; }
        public bool IsShowHomePage { get; set; }
        public Nullable<int> Page_id { get; set; }
        public string TitleClick1 { get; set; }
        public string LinkClick1 { get; set; }
        public bool IsActiveClick1 { get; set; }
        public string TitleClick2 { get; set; }
        public string LinkClick2 { get; set; }
        public bool IsActiveClick2 { get; set; }
        public bool InMobile { get; set; }
    }
}
