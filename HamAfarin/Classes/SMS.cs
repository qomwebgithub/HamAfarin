using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace HamAfarin
{
    public class SMS
    {
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
    }
}