namespace HamAfarin.Classes
{
    public class PaymentVerifyResult
    {
        public string TransactionCode { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string RefNum { get; set; }
        public string RRN { get; set; }
        public string Result { get; internal set; }
    }
}