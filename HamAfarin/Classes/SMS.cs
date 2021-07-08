using DataLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HamAfarin
{
    public class SMS
    {
        HamAfarinDBEntities db = new HamAfarinDBEntities();

        public void SendSMS(string MobileNumber, string Message)
        {
            SaharSendSms(MobileNumber, Message);
            //string url = "https://www.saharsms.com/api/kZDja1GdmznrkBMwSVVuJQ5KjzRNsAsM/json/SendVerify?receptor=" + MobileNumber + "&template=HamafarinVerify-21713&token=" + Message;

            //ServicePointManager.Expect100Continue = true;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //request.Credentials = CredentialCache.DefaultCredentials;

            //// Get the response.  
            //WebResponse response = request.GetResponse();
            //// Display the status.  
            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            //// Get the stream containing content returned by the server. 
            //// The using block ensures the stream is automatically closed. 
            //using (Stream dataStream = response.GetResponseStream())
            //{
            //    // Open the stream using a StreamReader for easy access.  
            //    StreamReader reader = new StreamReader(dataStream);
            //    // Read the content.  
            //    string responseFromServer = reader.ReadToEnd();
            //    // Display the content.  
            //    Console.WriteLine(responseFromServer);
            //}

            //// Close the response.  
            //response.Close();

        }
        public void SaharSendSms(string MobileNumber, string Message)
        {
            string USER_API_KEY = "kZDja1GdmznrkBMwSVVuJQ5KjzRNsAsM";
            string Template = "HamafarinVerify-21713";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://soorsatan.org/cafe/SendSmsSahar");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "Get";
            httpWebRequest.Headers.Add("MobileNumber", MobileNumber);
            httpWebRequest.Headers.Add("Message", Message);
            httpWebRequest.Headers.Add("USER_API_KEY", USER_API_KEY);
            httpWebRequest.Headers.Add("Template", Template);

            // Get the response.
            using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
            }
        }

        public  (bool Success, string Message) AdpSendSms(string MobileNumber, string Message)
        {
            (bool Success, string Message) tokenResult;
            // این کد پیش فرض داخل داکیومنت می باشد
            string url = "https://ws2.adpdigital.com/url/send?username=irfintech&password=irfintech123&dstaddress=" + MobileNumber + "&srcaddress=98200071072&body=" + Message + "&unicode=1";
            
            WebClient client = new WebClient();
            try
            {
                byte[] respData = client.DownloadData(url);
                string response = Encoding.ASCII.GetString(respData);
                tokenResult = (true, response);
                return tokenResult;
            }
            catch (WebException x)
            {
                Tbl_SmsException oSmsException = new Tbl_SmsException()
                {
                    CreateDate = DateTime.Now,
                    Description = "Message: " + x.Message + " - Response: " + x.Response,
                    Exception = x.ToString(),
                    ID = Guid.NewGuid().ToString(),
                    Method = nameof(AdpSendSms)
                };
                db.Tbl_SmsException.Add(oSmsException);
                db.SaveChanges();
                tokenResult = (false, "Message: " + x.Message + " - Response: " + x.Response);
                return tokenResult;
            }

        }

        public async Task<(bool Success, string Message)> AdpSendSmsAsync(string MobileNumber, string Message)
        {
            (bool Success, string Message) tokenResult;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://ws2.adpdigital.com/url/");

                HttpResponseMessage response = await client.PostAsync("send?username=irfintech&password=irfintech123&dstaddress="+ MobileNumber + "&srcaddress=98200071072&body="+ Message + "&unicode=1", null);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    tokenResult = (true, responseContent);
                    return tokenResult;
                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    string errorMessage = responseContent;
                    Tbl_SmsException oSmsException = new Tbl_SmsException()
                    {
                        CreateDate = DateTime.Now,
                        Description = errorMessage,
                        ID = Guid.NewGuid().ToString(),
                        Method = nameof(AdpSendSmsAsync)
                    };
                    db.Tbl_SmsException.Add(oSmsException);
                    db.SaveChanges();
                    tokenResult = (false, errorMessage);
                    return tokenResult;
                }
            }
        }
    }
}