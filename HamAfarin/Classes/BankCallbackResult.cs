namespace HamAfarin.Classes
{
    public class BankCallbackResult
    {
        public bool IsSucceed { get; set; }

        public string ReferenceId { get; set; }

        public string TransactionId { get; set; }

        public string SecurePan { get; set; }

        public string MID { get; set; }

        public string Rrn { get; set; }

        public string TraceNo { get; set; }

        public string Message { get; set; }
        public string Result { get; set; }
        public string State { get; set; }
    }
}