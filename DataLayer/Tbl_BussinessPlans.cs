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
    
    public partial class Tbl_BussinessPlans
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_BussinessPlans()
        {
            this.Tbl_BusinessPlanGallery = new HashSet<Tbl_BusinessPlanGallery>();
            this.Tbl_BusinessPlanPayment = new HashSet<Tbl_BusinessPlanPayment>();
            this.Tbl_BusinessPlanQuestion = new HashSet<Tbl_BusinessPlanQuestion>();
            this.Tbl_CommentPlan = new HashSet<Tbl_CommentPlan>();
            this.Tbl_DepositToInvestors = new HashSet<Tbl_DepositToInvestors>();
            this.Tbl_PaymentReturned = new HashSet<Tbl_PaymentReturned>();
        }
    
        public int BussinessPlanID { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> User_id { get; set; }
        public string Title { get; set; }
        public string ImageNameWarranty { get; set; }
        public string ImageNameInListPalns { get; set; }
        public string ImageNameInSinglePlan { get; set; }
        public string ShortDescription { get; set; }
        public string BussinessLogoImageName { get; set; }
        public string BussinessName { get; set; }
        public string BussinessSummaryDescription { get; set; }
        public Nullable<int> BussinessField_id { get; set; }
        public string CompanyName { get; set; }
        public Nullable<int> CompanyType_id { get; set; }
        public string CompanyRegisterCode { get; set; }
        public Nullable<System.DateTime> CompanyRegisterDate { get; set; }
        public string CompanyNationalCertificateCode { get; set; }
        public string CompanyEconomicCode { get; set; }
        public string CompanyAgentFullName { get; set; }
        public string CompanyAgentRole { get; set; }
        public string CompanyAgentPhoneNumber { get; set; }
        public string CompanyAgentEmail { get; set; }
        public string CompanyAgentNationalCardImageName { get; set; }
        public string CompanyIntroductionLetterFileName { get; set; }
        public string CompanyRegisterAddress { get; set; }
        public string CompanyPostalCode { get; set; }
        public string CompanyCity { get; set; }
        public Nullable<int> FinancialDuration_id { get; set; }
        public bool IsDifferentActiveAddressWithRegisterAddress { get; set; }
        public string CompanyActiveAddress { get; set; }
        public string CompanyActivePostalCode { get; set; }
        public bool IsDaneshBonyan { get; set; }
        public string BussinessWebSiteAddress { get; set; }
        public string BussinessInstagramAddress { get; set; }
        public string BussinessAparatAddress { get; set; }
        public string BusinessPlanRisksUser { get; set; }
        public string BusinessPlanRisks { get; set; }
        public string BussinessModelFileName { get; set; }
        public string IntroductionIdeaVideoFileName { get; set; }
        public string MarketTarget { get; set; }
        public string Coasts { get; set; }
        public string CompetitiveAdvantagesAndDisadvantages { get; set; }
        public string SlideShowPresentationFileName { get; set; }
        public string DocumentsAndReportsFileName { get; set; }
        public Nullable<int> MonetaryUnit_id { get; set; }
        public string AmountRequiredRoRaiseCapital { get; set; }
        public bool HaveYouRaisedCapitalPrevious { get; set; }
        public string InvestmentAmountPrevious { get; set; }
        public string PercentageOfSharesCapitalPrevious { get; set; }
        public string PreviousInvestorFullName { get; set; }
        public string PreviousInvestorType { get; set; }
        public Nullable<System.DateTime> PreviousInvestorDate { get; set; }
        public Nullable<System.DateTime> PreviousInvestorExpireDate { get; set; }
        public Nullable<int> PercentageReturnInvestment { get; set; }
        public string BusinessPlanFeatures { get; set; }
        public string Location { get; set; }
        public string LinkedinUrl { get; set; }
        public Nullable<int> MaximumInvestmentPercentage { get; set; }
        public string MinimumAmountInvest { get; set; }
        public Nullable<System.DateTime> InvestmentStartDate { get; set; }
        public Nullable<System.DateTime> InvestmentExpireDate { get; set; }
        public bool IsOverflowInvestment { get; set; }
        public bool IsSuccessBussinessPlan { get; set; }
        public string ReasonsForPlanFailure { get; set; }
        public string CodeOTC { get; set; }
        public string FaraboorsProjectId { get; set; }
        public string FinancialInformationText { get; set; }
        public string ProgressReportText { get; set; }
        public string InvestorsText { get; set; }
        public string ReasonsExtendingPlan { get; set; }
        public string Canonical { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string ImageAlt { get; set; }
        public string TitleUrl { get; set; }
        public string SeoKey { get; set; }
        public string ContractFileName { get; set; }
        public bool IsProjectParticipationReady { get; set; }
        public string PlanInFarabourseUrl { get; set; }
        public string EnglishCodeOTC { get; set; }
        public string IndustryGroupDescription { get; set; }
        public string SubIndustryGroupDescription { get; set; }
        public bool IsPublish { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_BusinessPlanGallery> Tbl_BusinessPlanGallery { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_BusinessPlanPayment> Tbl_BusinessPlanPayment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_BusinessPlanQuestion> Tbl_BusinessPlanQuestion { get; set; }
        public virtual Tbl_BussinessPlan_BussenessFields Tbl_BussinessPlan_BussenessFields { get; set; }
        public virtual Tbl_BussinessPlan_FinancialDuration Tbl_BussinessPlan_FinancialDuration { get; set; }
        public virtual Tbl_CompanyType Tbl_CompanyType { get; set; }
        public virtual Tbl_MonetaryUnits Tbl_MonetaryUnits { get; set; }
        public virtual Tbl_Users Tbl_Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_CommentPlan> Tbl_CommentPlan { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_DepositToInvestors> Tbl_DepositToInvestors { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_PaymentReturned> Tbl_PaymentReturned { get; set; }
    }
}
