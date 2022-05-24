﻿using DataLayer;
using HamAfarin.SmsSender;
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
        //JaxRpcMessagingServiceClient sms = new JaxRpcMessagingServiceClient();

        public (bool Success, string Message) SendSms(string mobileNumber, string message)
        {
            mobileNumber = FixMobileNumber(mobileNumber);
            //string[] lsttMobileNumber = { mobileNumber };
            (bool Success, string Message) tokenResult;

            //string a = sms.send(
            //    "irfintech",
            //    "irfintech123",
            //    "98200071072",
            //    lsttMobileNumber,
            //    "",
            //    "",
            //    "",
            //    "",
            //    "",
            //    "",
            //    "",
            //    "");
            // این کد پیش فرض داخل داکیومنت می باشد
            string url = "https://ws2.adpdigital.com/url/send?username=irfintech&password=irfintech123&dstaddress=" + mobileNumber + "&srcaddress=98200071072&body=" + message + "&unicode=1";
            WebClient client = new WebClient();
            try
            {
                byte[] respData = client.DownloadData(url);
                string response = Encoding.ASCII.GetString(respData);
                Tbl_SmsLog oSmsException = new Tbl_SmsLog()
                {
                    CreateDate = DateTime.Now,
                    Exception = null,
                    MobileNumber = mobileNumber,
                    Message = message,
                    ID = Guid.NewGuid().ToString(),
                    Method = nameof(SendSms)
                };
                db.Tbl_SmsLog.Add(oSmsException);
                db.SaveChanges();
                tokenResult = (true, response);
                return tokenResult;
            }
            catch (WebException x)
            {
                Tbl_SmsLog oSmsException = new Tbl_SmsLog()
                {
                    CreateDate = DateTime.Now,
                    Exception = "Exception: " + x.ToString() + "Message: " + x.Message + " - Response: " + x.Response,
                    MobileNumber = mobileNumber,
                    Message = message,
                    ID = Guid.NewGuid().ToString(),
                    Method = nameof(SendSms)
                };
                db.Tbl_SmsLog.Add(oSmsException);
                db.SaveChanges();
                tokenResult = (false, "Message: " + x.Message + " - Response: " + x.Response);
                return tokenResult;
            }

        }

        public async Task<(bool Success, string Message)> SendSmsAsync(string mobileNumber, string message)
        {
            mobileNumber = FixMobileNumber(mobileNumber);

            (bool Success, string Message) tokenResult;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://ws2.adpdigital.com/url/");

                HttpResponseMessage response = await client.PostAsync("send?username=irfintech&password=irfintech123&dstaddress=" + mobileNumber + "&srcaddress=98200071072&body=" + message + "&unicode=1", null);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Tbl_SmsLog oSmsException = new Tbl_SmsLog()
                    {
                        CreateDate = DateTime.Now,
                        Exception = null,
                        MobileNumber = mobileNumber,
                        Message = message,
                        ID = Guid.NewGuid().ToString(),
                        Method = nameof(SendSmsAsync)
                    };
                    db.Tbl_SmsLog.Add(oSmsException);
                    await db.SaveChangesAsync();
                    tokenResult = (true, responseContent);
                    return tokenResult;
                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Tbl_SmsLog oSmsException = new Tbl_SmsLog()
                    {
                        CreateDate = DateTime.Now,
                        Exception = "Response Code: " + response.StatusCode.ToString() + "Error Message: " + responseContent,
                        MobileNumber = mobileNumber,
                        Message = message,
                        ID = Guid.NewGuid().ToString(),
                        Method = nameof(SendSmsAsync)
                    };
                    db.Tbl_SmsLog.Add(oSmsException);
                    await db.SaveChangesAsync();
                    tokenResult = (false, responseContent);
                    return tokenResult;
                }
            }
        }

        private string FixMobileNumber(string mobileNumber)
        {
            string countryCode = "98";
            if (mobileNumber.Contains(","))
            {
                string[] mobileNumbersArray = mobileNumber.Split(',');

                List<string> lstFixedMobileNumbers = new List<string>();

                foreach (string number in mobileNumbersArray)
                {
                    if (number.Substring(0, 2) == countryCode)
                        lstFixedMobileNumbers.Add(number);
                    else
                        lstFixedMobileNumbers.Add("98" + number.Substring(1));
                }

                mobileNumber = String.Join(",", lstFixedMobileNumbers);
            }
            else if (mobileNumber.Substring(0, 2) != countryCode)
            {
                mobileNumber = "98" + mobileNumber.Substring(1);
            }

            return mobileNumber;
        }
    }
}