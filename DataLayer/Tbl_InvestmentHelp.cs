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
    
    public partial class Tbl_InvestmentHelp
    {
        public int InvestmentHelpID { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string InvestmentHelpTitle { get; set; }
        public string Description { get; set; }
        public string FullText { get; set; }
        public string InvestmentProcess { get; set; }
        public string FrequentlyAskedQuestions { get; set; }
        public string HoldingCampaign { get; set; }
        public string SendPlan { get; set; }
        public string QuestionsText { get; set; }
    }
}