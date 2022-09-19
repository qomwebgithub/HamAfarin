using HamAfarin.Classes.Dto;
using HamAfarin.Classes.Interface;
using HamAfarin.ZarinPal;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace HamAfarin.Classes
{
    public class SamanBankService : IBankService
    {
        private const string _terminalId = "12949549";
        private const string _terminalPassword = "1837060";

        public async Task<BankRequestDto> RequestAsync(BankInvoice bankInvoice)
        {
            string json = MakeJson(bankInvoice);

            #region Old
            //HttpWebRequest request = (HttpWebRequest)WebRequest
            //    .Create("https://sep.shaparak.ir/onlinepg/onlinepg");

            //request.Method = "POST";
            //request.ContentType = "Application/Json";

            //byte[] textArray = Encoding.UTF8.GetBytes(json);
            //request.ContentLength = textArray.Length;
            //request.GetRequestStream().Write(textArray, 0, textArray.Length);

            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //StreamReader reader = new StreamReader(response.GetResponseStream());

            //string result = reader.ReadToEnd();

            //var dto = new BankRequestDto() { IsSuccess = false, Result = result };
            #endregion

            var dto = new BankRequestDto() { IsSuccess = false, Result = null };

            using (var client = new HttpClient())
            {
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("https://sep.shaparak.ir/onlinepg/onlinepg", content);
                string responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    dto.Result = responseContent;
                    return dto;
                }

                if (!responseContent.Contains("\"status\":1"))
                {
                    dto.Result = responseContent;
                    return dto;
                }

                dto.IsSuccess = true;
                dto.Token = responseContent.Split(':', ',')[3].Replace("\"", "").Replace("}", "");
                dto.RedirectUrl = "https://sep.shaparak.ir/OnlinePG/SendToken?token=" + dto.Token;

                return dto;
            }
        }

        public BankCallbackResult Fetch(HttpRequestBase httpRequest)
        {
            var isSuccess = false;
            string message = null;
            string referenceIdResult = "";
            string transactionIdResult = "";

            //(MIDشماره ترمینال)
            //(Stateوضعیت تراکنش(حروف انگلیسی))
            //(Statusوضعیت تراکنش(مقدار عددی))
            //(RRNشماره مرجع)
            //(RefNumرسید دیجیتالی خرید)
            //(ResNumشماره خرید)
            //(TerminalIdشماره ترمینال)
            //(TraceNoشماره رهگیری)
            //Amount
            //Wage
            //(SecurePanشماره کارتی که تراکنش با آن انجام شده است).
            // SHA256 شماره کارت هش شدهHashedCardNumber


            //var securePan = httpRequest.QueryString["SecurePan"];
            var securePan = httpRequest.Form["SecurePan"];
            var cid = httpRequest.Form["CID"];
            var traceNo = httpRequest.Form["TraceNo"];
            var rrn = httpRequest.Form["RRN"];

            var state = httpRequest.Form["state"];

            if (string.IsNullOrEmpty(state))
                message = "اطلاعات نا معتبر از درگاه دریافت شد";
            else
            {
                referenceIdResult = httpRequest.Form["ResNum"];

                transactionIdResult = httpRequest.Form["RefNum"];

                isSuccess = state.Equals("OK", StringComparison.OrdinalIgnoreCase);

                if (!isSuccess)
                    message = StateTranslator(state);
            }

            return new BankCallbackResult
            {
                IsSucceed = isSuccess,
                ReferenceId = referenceIdResult,
                TransactionId = transactionIdResult,
                SecurePan = securePan,
                MID = cid,
                TraceNo = traceNo,
                Rrn = rrn,
                Message = message,
                Result = httpRequest.Form.ToString()
            };
        }

        public async Task<PaymentVerifyResult> VerifyAsync(BankCallbackResult callbackResult)
        {
            using (var client = new HttpClient())
            {
                ListDictionary body = new ListDictionary();
                body.Add("RefNum", callbackResult.TransactionId);
                body.Add("TerminalNumber", _terminalId);
                string json = JsonConvert.SerializeObject(body);

                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("https://sep.shaparak.ir/verifyTxnRandomSessionkey/ipg/VerifyTransaction", content);
                string responseContent = await response.Content.ReadAsStringAsync();

                var dto = new PaymentVerifyResult();

                if (response.IsSuccessStatusCode == false)
                {
                    dto.Result = responseContent;
                    return dto;
                }

                #region SampleResponse
                //"VerifyInfo": {
                //      "RRN": "14226761817",
                //      "RefNum": "50",
                //      "MaskedPan": "621986****8080",
                //      "HashedPan":
                //      "b96a14400c3a59249e87c300ecc06e5920327e70220213b5bbb7d7b2410f7e0d",
                //      "TerminalNumber": 2001,
                //      "OrginalAmount": 1000,
                //      "AffectiveAmount": 1000,
                //      "StraceDate": "2019-09-16 18:11:06",
                //      "StraceNo": "100428"
                //},
                //"ResultCode": 0,
                //,"عملیات با موفقیت انجام شد" :""ResultDescription
                //"Success": true
                #endregion

                var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

                dto.Message = dic["ResultDescription"];
                dto.RefNum = dic["RefNum"];
                dto.RRN = dic["RRN"];
                dto.IsSuccess = bool.Parse(dic["Success"]);

                return dto;
            }
        }

        private string StateTranslator(string result)
        {
            string message = "";
            switch (result)
            {
                case "": message = "تراكنش توسط خريدار كنسل شد"; break;
                case "Canceled By User": message = "تراكنش توسط خريدار كنسل شد"; break;
                case "Invalid Amount": message = "مبلغ سند برگش ت ي، از مبلغ تراکنش اص ل ي بیشتراست."; break;
                case "Invalid Transaction": message = "درخواست برگشت یک تراکنش رسیده است، در حالي که تراکنش اصلي پیدا نمي شود."; break;
                case "Invalid Card Number": message = "شماره کارت اشتباه است."; break;
                case "No Such Issuer": message = "چنین صادر کننده کارتي وجود ندارد."; break;
                case "Expired Card Pick Up": message = "از تاریخ انقضاي کارت گذشته اس ت و کارت دیگر معتبر نیست."; break;
                case "Allowable PIN Tries Exceeded Pick Up":
                    message = "۳ مرتبه اشتباه وارد شده است (PIN) رمز کارت در نتیجه کارت غیر فعال خواهد شد."; break;
                case "Incorrect PIN": message = "را اشتباه وارد کرده (PIN) خریدار رمز کارت"; break;
                case "Exceeds Withdrawal Amount Limit": message = "مبلغ بیش از سقف برداشت مي باشد."; break;
                case "Transaction Cannot Be Completed":
                    message = "PIN شده است ( شماره Authorize تراکنش درست هستند) ولي امکان سند خوردن PAN ووجود ندارد."; break;
                case "Response Received Too Late": message = "خورده Timeout تراکنش در شبکه بانکي است."; break;
                case "Suspected Fraud Pick Up":
                    message = "را ExpDate و یا فیلد CVV خریدار یا فیلد 2 اشتباه زده است. (یا اصلا وارد نکرده است)"; break;
                case "No Sufficient Funds": message = "موجودي به اندازي کافي در حساب وجود ندارد."; break;
                case "Issuer Down Slm":
                    message = "سیستم کارت بانک صادر کننده در وضعیت عملیاتي نیست."; break;
                default: message = $"خطای نا شناخته Response: {result}"; break;
            };

            return message;
        }

        private string MakeJson(BankInvoice bankInvoice)
        {
            var data = new SamanBankRequestTokenDto()
            {
                Action = "token",
                Amount = bankInvoice.Amount,
                CellNumber = bankInvoice.UserMobile,
                RedirectUrl = bankInvoice.AfterPaymentRedirectAddress,
                ResNum = bankInvoice.InvoiceNumber,
                TerminalId = _terminalId
            };
            return JsonConvert.SerializeObject(data);
        }
        
        #region old
        //public (bool IsSuccess, string Result) CheckTransaction(string transactionReferenceID)
        //{
        //    throw new NotImplementedException();
        //}

        //public (bool IsSuccess, string Result) ConfirmPayment(BankPaymentDto dto)
        //{
        //    throw new NotImplementedException();
        //}

        //public BankPaymentDto DeserializeBankRequest(NameValueCollection requestQueryString)
        //{
        //    string mid = requestQueryString["MID"],
        //        state = requestQueryString["State"],
        //        status = requestQueryString["Status"],
        //        rrn = requestQueryString["RRN"],
        //        refNum = requestQueryString["RefNum"],
        //        resNum = requestQueryString["ResNum"],
        //        traceNo = requestQueryString["TraceNo"],
        //        amount = requestQueryString["Amount"],
        //        wage = requestQueryString["Wage"],
        //        securePan = requestQueryString["SecurePan"],
        //        hashedCardNumber = requestQueryString["HashedCardNumber"];

        //    return new BankPaymentDto();
        //}

        //public long GetTransactionAmount(string transactionResult)
        //{
        //    throw new NotImplementedException();
        //}

        //public (bool IsSuccess, string Result) RefundPayment(BankPaymentDto dto)
        //{
        //    throw new NotImplementedException();
        //}
        #endregion
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