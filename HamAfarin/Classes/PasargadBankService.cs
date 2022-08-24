using HamAfarin.Classes.Dto;
using HamAfarin.Classes.Interface;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace HamAfarin.Classes
{
    public class PasargadBankService : IBankService
    {
        private const string _merchantCode = "4650168";
        private const string _terminalCode = "1837060";
        private const string _action = "1003";
        private const string _secretKey = "<RSAKeyValue><Modulus>uXkZ3gR907p+1ygpEhNCrP0dSKiSBba4V/uBopMMWfg+z5bMhzJ759D5mLXo81aQboa30Djj6CQNGx+bd7wZYlx0z3WHZi1c9UH9lwIFvGnJ/9RpD+Blr06U6EHhe/mCw6Jsg2UausqX7bhkQyzWma7EbBgc+ieyd72ba9Fe/7U=</Modulus><Exponent>AQAB</Exponent><P>5v4scbAyu1Bq+LhEzPoilqx0RxgzUtz7i3F6463QPYBD3CmZVpwgQZXrQ4bE0XesSUl/BkcIrN/mywCbIJv0Zw==</P><Q>zY1fn8kPSkHAQnFVrfn1cqo3QE4uAJfJiv6boxvUUM2mWfK8ujvOkrUjTUZ/J+O2RzdIv0VCBGeWUeKEruo5gw==</Q><DP>Hiu0wmSxO6YVUsc+tUc2nVeJGIAgtAIJGP2Jf5OET4QhWPBWBun9jJN4VymTK4jmB+yBmuBMUcgs7Pb3TBsSoQ==</DP><DQ>Yoy+ZQBjuUlu4SwvVPs7h58+YDFbcuNTOLW7buc/0wHWGNf9ThiwgLwh0cHT4w8U7G4ADdwpu6zicB33WVlo+w==</DQ><InverseQ>e6u/g2rKgb6U3At6o20nIas7x1iIAGMPDvVJBu22lw/t0u4HPROjkavo/P+SOgWG9ziS5vfprGD8spgwixpXNQ==</InverseQ><D>AD/BYSLwaFBfyzoqk/Oiq0jLuUVArPFJ3hRgYC+CXLyQmQbCz4upzu3g5+uWnH0JRJy5snXhGHaz7c1lEAwYnKCdF5IS0sma5BqOPlCQHYBjp0FTN5jI2gepRP7TUV67e29BzE3IqS1zfPk4oq8XA0PvI1FZEGNAUYXVuYRnHiE=</D></RSAKeyValue>";
        private const string _bankRedirectUrl = "https://pep.shaparak.ir/payment.aspx?n=";

        public (bool IsSuccess, string Result) RequestToken(BankDto dto)
        {
            string json = MakeJson(dto);
            string sign = SignJson(json);

            HttpWebRequest request = (HttpWebRequest)WebRequest
                .Create("https://pep.shaparak.ir/Api/v1/Payment/GetToken");
            request.Method = "POST";
            request.ContentType = "Application/Json";
            request.Headers.Add("Sign", sign);

            byte[] textArray = Encoding.UTF8.GetBytes(json);
            request.ContentLength = textArray.Length;
            request.GetRequestStream().Write(textArray, 0, textArray.Length);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream());

            var result = reader.ReadToEnd();
            var success = false;

            if (result.Contains("Token"))
                success = true;

            return (success, result);
        }

        public string GetToken(string requestToken)
        {
            string[] res = requestToken.Split(':', ',');
            return res[1].Replace("\"", "");
        }

        public string GetRedirectUrl(string token)
        {
            return _bankRedirectUrl + token;
        }

        //public VerifyBankDto VerifyPayment(HttpRequestBase request)
        //{
        //    VerifyBankDto dto = new VerifyBankDto();
            
        //    dto.TransactionReferenceID = request.QueryString["tref"];
        //    dto.TransactionResult = CheckTransactionResult(dto.TransactionReferenceID);

        //    return dto;

        //    var res = dto.TransactionResult.Split(':', ',');
        //    string[] pay = res[21].Split('.');

        //    //long raisedPrice = planService.GetRaisedPrice(db, qBusinessPlanPayment.Tbl_BussinessPlans.BussinessPlanID) + qBusinessPlanPayment.PaymentPrice.Value;
        //    //long totalPrice = long.Parse(qBusinessPlanPayment.Tbl_BussinessPlans.AmountRequiredRoRaiseCapital);

        //    //string invoiceNumber = request.QueryString["iN"];
        //    //string invoiceDate = request.QueryString["iD"];

        //    //if (qBusinessPlanPayment.Tbl_BussinessPlans.IsOverflowInvestment == false && totalPrice < raisedPrice)
        //    //{
        //    //    // برگشت وجه
        //    //    string refundResult = RefundPayment(pay[0], invoiceNumber, invoiceDate);
        //    //    ViewBag.Result = "برگشت وجه";
        //    //    return View();
        //    //}

        //    //// تایید پرداخت به درگاه
        //    //string ShaparakRefNumber = ConfirmPayment(pay[0], invoiceNumber, invoiceDate);

        //    //var shaparakbum = ShaparakRefNumber.Split(':', ',');
        //    //if (ShaparakRefNumber.Contains("ShaparakRefNumber"))
        //    //{
        //    //    qPaymentOnline.ShaparakVerifyPayment = ShaparakRefNumber;
        //    //    qPaymentOnline.IsFinally = true;
        //    //    qBusinessPlanPayment.TransactionPaymentCode = dto.TransactionReferenceID;
        //    //    qBusinessPlanPayment.IsPaid = true;
        //    //    qPaymentOnline.FinallyDate = DateTime.Now;
        //    //    db.SaveChanges();
        //    //    ViewBag.IsSuccess = true;
        //    //    ViewBag.dto.TransactionReferenceID = dto.TransactionReferenceID;

        //    //    // 4 = سرمایه گذاری
        //    //    Tbl_Sms qSms = db.Tbl_Sms.Find(4);
        //    //    Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.UserID == qPaymentOnline.Tbl_BusinessPlanPayment.Tbl_BussinessPlans.User_id);
        //    //    (bool Success, string Message) result = oSms.SendSms(qUser.MobileNumber, qSms.Message);

        //    //    return RedirectToAction("SinglePaymentBusinessPlan", "UserPaymentBusinessPlan", new { area = "UserPanel", id = qPaymentOnline.Payment_id, notify = true });
        //    //}
        //}

        private string SignJson(string data)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(_secretKey);
            byte[] signMain = rsa.SignData(Encoding.UTF8.GetBytes(data), new
            SHA1CryptoServiceProvider());
            return Convert.ToBase64String(signMain);
        }

        private string MakeJson(BankDto dto)
        {
            PasargadBankRequestTokenDto dataPost = new PasargadBankRequestTokenDto();
            dataPost.TerminalCode = _terminalCode;
            dataPost.MerchantCode = _merchantCode;
            dataPost.Action = _action;
            dataPost.InvoiceDate = dto.InvoiceDate.ToString("yyyy/MM/dd");
            dataPost.TimeStamp = dto.TimeStamp.ToString("yyyy/MM/dd HH:mm:ss");
            dataPost.Amount = dto.Amount.ToString();
            dataPost.InvoiceNumber = dto.InvoiceNumber;
            dataPost.RedirectAddress = dto.AfterPaymentRedirectAddress;
            return JsonConvert.SerializeObject(dataPost);
        }

        private string CheckTransactionResult(string transactionReferenceID)
        {
            PasargadBankRequestTokenDto dp = new PasargadBankRequestTokenDto();
            dp.TransactionReferenceID = transactionReferenceID;
            string output = JsonConvert.SerializeObject(dp);
            string sign = SignJson(output);

            byte[] textArray = Encoding.UTF8.GetBytes(output);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://pep.shaparak.ir/Api/v1/Payment/CheckTransactionResult");
            request.Method = "POST";
            request.ContentType = "Application/Json";
            request.ContentLength = textArray.Length;
            request.Headers.Add("Sign", sign);
            request.GetRequestStream().Write(textArray, 0, textArray.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());

            return reader.ReadToEnd();
        }

        private string ConfirmPayment(string amount, string invoiceNumber, string invoiceDate)
        {
            string timeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            PasargadBankRequestTokenDto dp = new PasargadBankRequestTokenDto();
            dp.InvoiceNumber = invoiceNumber;
            dp.InvoiceDate = invoiceDate;
            dp.MerchantCode = _merchantCode;
            dp.TerminalCode = _terminalCode;
            dp.Amount = amount.ToString();
            dp.TimeStamp = timeStamp;
            string output = JsonConvert.SerializeObject(dp);
            string sign = SignJson(output);

            byte[] textArray = Encoding.UTF8.GetBytes(output);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://pep.shaparak.ir/Api/v1/Payment/VerifyPayment");
            request.Method = "POST";
            request.ContentType = "Application/Json";
            request.ContentLength = textArray.Length;
            request.Headers.Add("Sign", sign);

            request.GetRequestStream().Write(textArray, 0, textArray.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream());
            string result = reader.ReadToEnd();
            return result;
        }

        private string RefundPayment(string amount, string invoiceNumber, string invoiceDate)
        {
            string timeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            PasargadBankRequestTokenDto dp = new PasargadBankRequestTokenDto();
            dp.InvoiceNumber = invoiceNumber;
            dp.InvoiceDate = invoiceDate;
            dp.MerchantCode = _merchantCode;
            dp.TerminalCode = _terminalCode;
            dp.Amount = amount.ToString();
            dp.TimeStamp = timeStamp;
            string output = JsonConvert.SerializeObject(dp);
            string sign = SignJson(output);

            byte[] textArray = Encoding.UTF8.GetBytes(output);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://pep.shaparak.ir/Api/v1/Payment/RefundPayment");
            request.Method = "POST";
            request.ContentType = "Application/Json";
            request.ContentLength = textArray.Length;
            request.Headers.Add("Sign", sign);

            request.GetRequestStream().Write(textArray, 0, textArray.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream());
            string result = reader.ReadToEnd();
            return result;
        }
    }

    public class PasargadBankRequestTokenDto
    {
        public string MerchantCode { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string TerminalCode { get; set; }
        public string Amount { get; set; }
        public string RedirectAddress { get; set; }
        public string Action { get; set; }
        public string TimeStamp { get; set; }
        public string TransactionReferenceID { get; set; }
    }


    public class VerifyBankDto
    {
        public string TransactionReferenceID { get; set; }
        public string TransactionResult { get; set; }
    }
}