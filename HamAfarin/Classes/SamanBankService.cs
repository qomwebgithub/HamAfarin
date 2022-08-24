using HamAfarin.Classes.Dto;
using HamAfarin.Classes.Interface;
using HamAfarin.ZarinPal;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace HamAfarin.Classes
{
    public class SamanBankService : IBankService
    {
        private const string _terminalId = "12949549";
        private const string _terminalPassword = "1837060";

        public (bool IsSuccess, string Result) RequestToken(BankDto dto)
        {
            string json = MakeJson(dto);

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

            bool success = false;
            if (result.Contains("\"status\":1"))
                success = true;

            return (success, result);
        }

        public string GetToken(string requestToken)
        {
            string[] res = requestToken.Split(':', ',');
            return res[3].Replace("\"", "").Replace("}", "");
        }

        public string GetRedirectUrl(string token)
        {
            return "https://sep.shaparak.ir/OnlinePG/SendToken?token=" + token;
        }

        private string MakeJson(BankDto dto)
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