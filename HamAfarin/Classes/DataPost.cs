using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HamAfarin
{
    public class DataPost
    {
        private string merchantCode;
        private string invoiceNumber;
        private string invoiceDate;
        private string terminalCode;
        private string amount;
        private string redirectAddress;
        private string action;
        private string timeStamp;
        private string transactionReferenceID;

        public string TransactionReferenceID
        {
            get
            {
                return transactionReferenceID;
            }
            set
            {
                transactionReferenceID = value;
            }
        } 
        public string InvoiceNumber
        {
            get
            {
                return invoiceNumber;
            }
            set
            {
                invoiceNumber = value;
            }
        }
        public string InvoiceDate
        {
            get
            {
                return invoiceDate;
            }
            set
            {
                invoiceDate = value;
            }
        }
        public string MerchantCode
        {
            get
            {
                return merchantCode;
            }
            set
            {
                merchantCode = value;
            }
        }
        public string TerminalCode
        {
            get
            {
                return terminalCode;
            }
            set
            {
                terminalCode = value;
            }
        }
        public string Amount
        {
            get
            {
                return amount;
            }
            set
            {
                amount = value;
            }
        }
        public string RedirectAddress
        {
            get
            {
                return redirectAddress;
            }
            set
            {
                redirectAddress = value;
            }
        }
        public string Action
        {
            get
            {
                return action;
            }
            set
            {
                action = value;
            }
        }
        public string TimeStamp
        {
            get
            {
                return timeStamp;
            }
            set
            {
                timeStamp = value;
            }
        }
    }
}