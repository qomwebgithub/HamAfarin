using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.Api
{
    public class ProjectInfoDto
    {
        [JsonProperty("Trace Code")]
        public string TraceCode { get; set; }

        [JsonProperty("Creation Date")]
        public DateTime CreationDate { get; set; }

        [JsonProperty("Persian Name")]
        public string PersianName { get; set; }

        [JsonProperty("Persian Suggested Symbol")]
        public string PersianSuggestedSymbol { get; set; }

        [JsonProperty("Persoan Approved Symbol")]
        public string PersoanApprovedSymbol { get; set; }

        [JsonProperty("English Name")]
        public string EnglishName { get; set; }

        [JsonProperty("English Suggested Symbol")]
        public string EnglishSuggestedSymbol { get; set; }

        [JsonProperty("English Approved Symbol")]
        public string EnglishApprovedSymbol { get; set; }

        [JsonProperty("Industry Group ID")]
        public int IndustryGroupID { get; set; }

        [JsonProperty("Industry Group Description")]
        public string IndustryGroupDescription { get; set; }

        [JsonProperty("Sub Industry Group ID")]
        public int SubIndustryGroupID { get; set; }

        [JsonProperty("Sub Industry Group Description")]
        public string SubIndustryGroupDescription { get; set; }

        [JsonProperty("Persian Subject")]
        public string PersianSubject { get; set; }

        [JsonProperty("English Subject")]
        public object EnglishSubject { get; set; }

        [JsonProperty("Unit Price")]
        public int UnitPrice { get; set; }

        [JsonProperty("Total Units")]
        public int TotalUnits { get; set; }

        [JsonProperty("Company Unit Counts")]
        public int CompanyUnitCounts { get; set; }

        [JsonProperty("Total Price")]
        public long TotalPrice { get; set; }

        [JsonProperty("Crowd Funding Type ID")]
        public int CrowdFundingTypeID { get; set; }

        [JsonProperty("Crowd Funding Type Description")]
        public string CrowdFundingTypeDescription { get; set; }

        [JsonProperty("Float Crowd Funding Type Description")]
        public string FloatCrowdFundingTypeDescription { get; set; }

        [JsonProperty("Minimum Required Price")]
        public long MinimumRequiredPrice { get; set; }

        [JsonProperty("Real Person Minimum Availabe Price")]
        public int RealPersonMinimumAvailabePrice { get; set; }

        [JsonProperty("Real Person Maximum Available Price")]
        public long RealPersonMaximumAvailablePrice { get; set; }

        [JsonProperty("Legal Person Minimum Availabe Price")]
        public long LegalPersonMinimumAvailabePrice { get; set; }

        [JsonProperty("Legal Person Maximum Availabe Price")]
        public long LegalPersonMaximumAvailabePrice { get; set; }

        [JsonProperty("Underwriting Duration")]
        public int UnderwritingDuration { get; set; }

        [JsonProperty("Suggested Underwriting Start Date")]
        public DateTime SuggestedUnderwritingStartDate { get; set; }

        [JsonProperty("Suggested Underwriting End Date")]
        public DateTime SuggestedUnderwritingEndDate { get; set; }

        [JsonProperty("Approved Underwriting Start Date")]
        public DateTime ApprovedUnderwritingStartDate { get; set; }

        [JsonProperty("Approved Underwriting End Date")]
        public DateTime ApprovedUnderwritingEndDate { get; set; }

        [JsonProperty("Project Start Date")]
        public DateTime ProjectStartDate { get; set; }

        [JsonProperty("Project End Date")]
        public DateTime ProjectEndDate { get; set; }

        [JsonProperty("Settlement Description")]
        public string SettlementDescription { get; set; }

        [JsonProperty("Project Status Description")]
        public string ProjectStatusDescription { get; set; }

        [JsonProperty("Project Status ID")]
        public int ProjectStatusID { get; set; }

        [JsonProperty("Persian Suggested Underwiring Start Date")]
        public string PersianSuggestedUnderwiringStartDate { get; set; }

        [JsonProperty("Persian Suggested Underwriting End Date")]
        public string PersianSuggestedUnderwritingEndDate { get; set; }

        [JsonProperty("Persian Approved Underwriting Start Date")]
        public string PersianApprovedUnderwritingStartDate { get; set; }

        [JsonProperty("Persian Approved Underwriting End Date")]
        public string PersianApprovedUnderwritingEndDate { get; set; }

        [JsonProperty("Persian Project Start Date")]
        public string PersianProjectStartDate { get; set; }

        [JsonProperty("Persian Project End Date")]
        public string PersianProjectEndDate { get; set; }

        [JsonProperty("Persian Creation Date")]
        public string PersianCreationDate { get; set; }

        [JsonProperty("Number of Finance Provider")]
        public int NumberOfFinanceProvider { get; set; }
        public long SumOfFundingProvided { get; set; }

        [JsonProperty("Project Owner Company")]
        public List<ProjectOwnerCompany> ProjectOwnerCompany { get; set; }

        [JsonProperty("List Of Project Big Share Holders")]
        public List<ListOfProjectBigShareHolder> ListOfProjectBigShareHolders { get; set; }

        [JsonProperty("List Of Project Board Members")]
        public List<ListOfProjectBoardMember> ListOfProjectBoardMembers { get; set; }
    }

    public class ListOfProjectBigShareHolder
    {
        [JsonProperty("National ID")]
        public object NationalID { get; set; }

        [JsonProperty("Shareholder Type")]
        public int ShareholderType { get; set; }

        [JsonProperty("First Name / Company Name")]
        public string FirstNameCompanyName { get; set; }

        [JsonProperty("Last Name / CEO Name")]
        public string LastNameCEOName { get; set; }

        [JsonProperty("Share Percent")]
        public double SharePercent { get; set; }
    }

    public class ListOfProjectBoardMember
    {
        [JsonProperty("National ID")]
        public long NationalID { get; set; }
        public int ForeignerID { get; set; }

        [JsonProperty("First Name")]
        public string FirstName { get; set; }

        [JsonProperty("Last Name")]
        public string LastName { get; set; }

        [JsonProperty("Mobile Number")]
        public string MobileNumber { get; set; }

        [JsonProperty("Email Address")]
        public string EmailAddress { get; set; }

        [JsonProperty("Organization Post ID")]
        public int OrganizationPostID { get; set; }

        [JsonProperty("Organization Post Description")]
        public string OrganizationPostDescription { get; set; }

        [JsonProperty("Is Agent from a Company")]
        public bool IsAgentFromACompany { get; set; }

        [JsonProperty("Company National ID")]
        public object CompanyNationalID { get; set; }

        [JsonProperty("Company Name")]
        public string CompanyName { get; set; }
    }

    public class ProjectOwnerCompany
    {
        [JsonProperty("National ID")]
        public long NationalID { get; set; }
        public string Name { get; set; }

        [JsonProperty("Company Type ID")]
        public int CompanyTypeID { get; set; }

        [JsonProperty("Company Type Description")]
        public string CompanyTypeDescription { get; set; }

        [JsonProperty("Registration Number")]
        public string RegistrationNumber { get; set; }

        [JsonProperty("Registration Date")]
        public DateTime RegistrationDate { get; set; }

        [JsonProperty("Economic ID")]
        public string EconomicID { get; set; }
        public string Address { get; set; }

        [JsonProperty("Postal Code")]
        public string PostalCode { get; set; }

        [JsonProperty("Phone Number")]
        public string PhoneNumber { get; set; }

        [JsonProperty("Fax Number")]
        public object FaxNumber { get; set; }

        [JsonProperty("Email Address")]
        public string EmailAddress { get; set; }
    }

}
