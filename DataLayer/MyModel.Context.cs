﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HamAfarinDBEntities : DbContext
    {
        public HamAfarinDBEntities()
            : base("name=HamAfarinDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Tbl_Blog> Tbl_Blog { get; set; }
        public virtual DbSet<Tbl_BusinessLevels> Tbl_BusinessLevels { get; set; }
        public virtual DbSet<Tbl_BusinessPlanGallery> Tbl_BusinessPlanGallery { get; set; }
        public virtual DbSet<Tbl_BusinessPlanPayment> Tbl_BusinessPlanPayment { get; set; }
        public virtual DbSet<Tbl_BusinessPlanQuestion> Tbl_BusinessPlanQuestion { get; set; }
        public virtual DbSet<Tbl_BussinessPlan_BussenessFields> Tbl_BussinessPlan_BussenessFields { get; set; }
        public virtual DbSet<Tbl_BussinessPlan_BussinessTypes> Tbl_BussinessPlan_BussinessTypes { get; set; }
        public virtual DbSet<Tbl_BussinessPlan_FinancialDuration> Tbl_BussinessPlan_FinancialDuration { get; set; }
        public virtual DbSet<Tbl_BussinessPlan_Status> Tbl_BussinessPlan_Status { get; set; }
        public virtual DbSet<Tbl_BussinessPlans> Tbl_BussinessPlans { get; set; }
        public virtual DbSet<Tbl_CapitalApplicantHelp> Tbl_CapitalApplicantHelp { get; set; }
        public virtual DbSet<Tbl_CommentPlan> Tbl_CommentPlan { get; set; }
        public virtual DbSet<Tbl_CompanyType> Tbl_CompanyType { get; set; }
        public virtual DbSet<Tbl_DepositToInvestors> Tbl_DepositToInvestors { get; set; }
        public virtual DbSet<Tbl_DepositToInvestorsDetails> Tbl_DepositToInvestorsDetails { get; set; }
        public virtual DbSet<Tbl_DepositTypes> Tbl_DepositTypes { get; set; }
        public virtual DbSet<Tbl_FutureBusinessPlan> Tbl_FutureBusinessPlan { get; set; }
        public virtual DbSet<Tbl_Galleries> Tbl_Galleries { get; set; }
        public virtual DbSet<Tbl_Investable> Tbl_Investable { get; set; }
        public virtual DbSet<Tbl_InvestmentHelp> Tbl_InvestmentHelp { get; set; }
        public virtual DbSet<Tbl_InvestmentProcess> Tbl_InvestmentProcess { get; set; }
        public virtual DbSet<Tbl_Investor> Tbl_Investor { get; set; }
        public virtual DbSet<Tbl_Menu> Tbl_Menu { get; set; }
        public virtual DbSet<Tbl_MenuPage> Tbl_MenuPage { get; set; }
        public virtual DbSet<Tbl_MenuType> Tbl_MenuType { get; set; }
        public virtual DbSet<Tbl_MonetaryUnits> Tbl_MonetaryUnits { get; set; }
        public virtual DbSet<Tbl_Pages> Tbl_Pages { get; set; }
        public virtual DbSet<Tbl_PaymentOnlineDetils> Tbl_PaymentOnlineDetils { get; set; }
        public virtual DbSet<Tbl_PaymentReturned> Tbl_PaymentReturned { get; set; }
        public virtual DbSet<Tbl_PaymentType> Tbl_PaymentType { get; set; }
        public virtual DbSet<Tbl_PersonLegal> Tbl_PersonLegal { get; set; }
        public virtual DbSet<Tbl_Roles> Tbl_Roles { get; set; }
        public virtual DbSet<Tbl_SajamToken> Tbl_SajamToken { get; set; }
        public virtual DbSet<Tbl_SejamException> Tbl_SejamException { get; set; }
        public virtual DbSet<Tbl_SejamTempNationalCode> Tbl_SejamTempNationalCode { get; set; }
        public virtual DbSet<Tbl_Settings> Tbl_Settings { get; set; }
        public virtual DbSet<Tbl_ShareHoldersCompany> Tbl_ShareHoldersCompany { get; set; }
        public virtual DbSet<Tbl_Sliders> Tbl_Sliders { get; set; }
        public virtual DbSet<Tbl_Sms> Tbl_Sms { get; set; }
        public virtual DbSet<Tbl_SmsLog> Tbl_SmsLog { get; set; }
        public virtual DbSet<Tbl_Tickets> Tbl_Tickets { get; set; }
        public virtual DbSet<Tbl_UserProfiles> Tbl_UserProfiles { get; set; }
        public virtual DbSet<Tbl_Users> Tbl_Users { get; set; }
    }
}
