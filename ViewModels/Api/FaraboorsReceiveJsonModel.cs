﻿using System;

namespace HamAfarin
{
    public class FaraboorsReceiveJsonModel
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
    }

    //public class FaraboorsReceiveJsonModel
    //{
    //    public long NationalID { get; set; }
    //    public bool IsLegal { get; set; }
    //    public string FirstName { get; set; }
    //    public string LastNameOrCompanyName { get; set; }
    //    public long? ProvidedFinancePrice { get; set; }
    //    public string BourseCode { get; set; }
    //    public string PaymentDate { get; set; }
    //}
}