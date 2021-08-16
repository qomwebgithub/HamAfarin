using System;

namespace HamAfarin
{
    public class FaraboorsJsonModel
    {
        public long NationalID { get; set; }
        public bool IsLegal { get; set; }
        public string FirstName { get; set; }
        public string LastNameOrCompanyName { get; set; }
        public long? ProvidedFinancePrice { get; set; }
        public string BourseCode { get; set; }
        public string PaymentDate { get; set; }
        //public string BankAccountNumber { get; set; }
        //public string MobileNumber { get; set; }
    }

    public class FaraboorsResponseJsonModel
    {
        public string TraceCode { get; set; }
        public int ProvidedFinancePrice { get; set; }
        public string Message { get; set; }
    }
}