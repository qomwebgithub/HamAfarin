using HamAfarin.Classes.Dto;
using HamAfarin.Classes.Interface;
using HamAfarin.ZarinPal;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace HamAfarin.Classes
{
    public class ZarinPalBankService : IBankService
    {
        private const string _bankRedirectUrl = "https://www.zarinpal.com/pg/StartPay/";
        private const string _secretKey = "bfb039ca-b62b-434a-a7fc-ad2b53c981b0";

        public (bool IsSuccess, string Result) RequestToken(BankDto dto)
        {
            ServicePointManager.Expect100Continue = false;
            string token;
            var zp = new PaymentGatewayImplementationServicePortTypeClient();
            int Status = zp.PaymentRequest(_secretKey, (int)dto.Amount, dto.Description,
                dto.UserEmail, dto.UserMobile, dto.AfterPaymentRedirectAddress, out token);

            var success = false;

            if (Status == 100)
                success = true;

            return (success, token);
        }

        public string GetToken(string requestToken)
        {
            return requestToken;
        }

        public string GetRedirectUrl(string token)
        {
            return _bankRedirectUrl + token;
        }
    }
}