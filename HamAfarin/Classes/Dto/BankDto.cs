using System;

namespace HamAfarin.Classes.Dto
{
    public class BankDto
    {
        public string UserEmail { get; set; }
        public string UserMobile { get; set; }
        public string Description { get; set; }
        public long Amount { get; set; }
        public string InvoiceNumber { get; set; }
        public string AfterPaymentRedirectAddress { get; set; }
        public DateTime TimeStamp { get; set; }
        public DateTime InvoiceDate { get; set; }
    }
}