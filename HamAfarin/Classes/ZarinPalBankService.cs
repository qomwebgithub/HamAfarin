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
        private const string _secretKey = "bfb039ca-b62b-434a-a7fc-ad2b53c981b0";

        public BankRequestDto Request(BankInvoice bankInvoice)
        {
            ServicePointManager.Expect100Continue = false;
            string token;
            var zp = new PaymentGatewayImplementationServicePortTypeClient();
            int Status = zp.PaymentRequest(_secretKey, (int)bankInvoice.Amount, bankInvoice.Description,
                bankInvoice.UserEmail, bankInvoice.UserMobile, bankInvoice.AfterPaymentRedirectAddress, out token);

            var dto = new BankRequestDto() { IsSuccess = false, Result = token };

            if (Status != 100)
                return dto;

            dto.IsSuccess = true;
            dto.Token = token;
            dto.RedirectUrl = "https://www.zarinpal.com/pg/StartPay/" + token;

            return dto;
        }
    }
}