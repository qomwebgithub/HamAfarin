using System;

namespace HamAfarin
{
    public class ProjectFinancingProviderDto
    {
        public string ApiKey { get; set; }
        public string ProjectID { get; set; }
        public long NationalID { get; set; }
        public bool IsLegal { get; set; }
        public string FirstName { get; set; }
        public string LastNameOrCompanyName { get; set; }
        public long? ProvidedFinancePrice { get; set; }
        public string BourseCode { get; set; }
        public string PaymentDate { get; set; }
        public string BankTrackingNumber { get; set; }
        public string ShebaBankAccountNumber { get; set; }
        public string MobileNumber { get; set; }
    }
}