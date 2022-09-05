namespace Hamafarin.Controllers
{
    public class DataPost
    {
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string MerchantCode { get; set; }
        public string TerminalCode { get; set; }
        public string Amount { get; set; }
        public string RedirectAddress { get; set; }
        public string Action { get; set; }
        public string TimeStamp { get; set; }
    }
}