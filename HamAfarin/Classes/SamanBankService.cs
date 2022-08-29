using HamAfarin.Classes.Dto;
using HamAfarin.Classes.Interface;
using HamAfarin.ZarinPal;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace HamAfarin.Classes
{
    public class SamanBankService : IBankService
    {
        private const string _terminalId = "12949549";
        private const string _terminalPassword = "1837060";

        public (bool IsSuccess, string Result) CheckTransaction(string transactionReferenceID)
        {
            throw new NotImplementedException();
        }

        public (bool IsSuccess, string Result) ConfirmPayment(BankPaymentDto dto)
        {
            throw new NotImplementedException();
        }

        public BankPaymentDto DeserializeBankRequest(NameValueCollection requestQueryString)
        {
            string mid = requestQueryString["MID"],
                state = requestQueryString["State"],
                status = requestQueryString["Status"],
                rrn = requestQueryString["RRN"],
                refNum = requestQueryString["RefNum"],
                resNum = requestQueryString["ResNum"],
                traceNo = requestQueryString["TraceNo"],
                amount = requestQueryString["Amount"],
                wage = requestQueryString["Wage"],
                securePan = requestQueryString["SecurePan"],
                hashedCardNumber = requestQueryString["HashedCardNumber"];

            return new BankPaymentDto();
        }

        public long GetTransactionAmount(string transactionResult)
        {
            throw new NotImplementedException();
        }

        public (bool IsSuccess, string Result) RefundPayment(BankPaymentDto dto)
        {
            throw new NotImplementedException();
        }

        public BankRequestDto Request(BankInvoice bankInvoice)
        {
            string json = MakeJson(bankInvoice);

            HttpWebRequest request = (HttpWebRequest)WebRequest
                .Create("https://sep.shaparak.ir/onlinepg/onlinepg");

            request.Method = "POST";
            request.ContentType = "Application/Json";

            byte[] textArray = Encoding.UTF8.GetBytes(json);
            request.ContentLength = textArray.Length;
            request.GetRequestStream().Write(textArray, 0, textArray.Length);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream());

            string result = reader.ReadToEnd();

            var dto = new BankRequestDto() { IsSuccess = false, Result = result };

            if (!result.Contains("\"status\":1"))
                return dto;

            dto.IsSuccess = true;
            dto.Token = result.Split(':', ',')[3].Replace("\"", "").Replace("}", "");
            dto.RedirectUrl = "https://sep.shaparak.ir/OnlinePG/SendToken?token=" + dto.Token;

            return dto;
        }

        private string MakeJson(BankInvoice dto)
        {
            var requestDto = new SamanBankRequestTokenDto()
            {
                Action = "token",
                Amount = dto.Amount,
                CellNumber = dto.UserMobile,
                RedirectUrl = dto.AfterPaymentRedirectAddress,
                ResNum = dto.InvoiceNumber,
                TerminalId = _terminalId
            };
            return JsonConvert.SerializeObject(requestDto);
        }

    }

    public class SamanBankRequestTokenDto
    {
        public string Action { get; set; }
        public string TerminalId { get; set; }
        public long Amount { get; set; }
        public string ResNum { get; set; }
        public string RedirectUrl { get; set; }
        public string CellNumber { get; set; }
    }
}