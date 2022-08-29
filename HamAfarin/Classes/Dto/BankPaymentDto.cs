using System;

namespace HamAfarin.Classes.Dto
{
    public class BankPaymentDto
    {
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string TransactionReferenceID { get; set; }
        public long Amount { get; set; }
    }
}