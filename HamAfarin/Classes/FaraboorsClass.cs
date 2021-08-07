using Common;
using DataLayer;
using HamAfarin.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ViewModels;

namespace HamAfarin
{
    public class FaraboorsClass
    {
        HamAfarinDBEntities db = new HamAfarinDBEntities();
        public async Task<(bool Success, string Message)> GetProjectParticipationReportAsync()
        {
            (bool Success, string Message) tokenResult;

            using (HttpClient client = new HttpClient())
            {
                ////Real API
                //client.BaseAddress = new Uri("https://cfapi.ifb.ir/projects/");


                client.BaseAddress = new Uri("http://cfapitest.ifb.ir/projects/");

                //Test API
                string subUrl = "GetProjectParticipationReport?apiKey=85d5ff91-0c4d-4142-beab-d734b72a40fe&projectId=3403cbaa-911b-44c3-af6f-de3c97367627&nationalID=1290485941";
                
                HttpResponseMessage response = await client.PostAsync(subUrl, null);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    tokenResult = (true, responseContent);
                    return tokenResult;
                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    tokenResult = (false, responseContent);
                    return tokenResult;
                }

            }
        }

        public async Task<(bool Success, string Message)> ProjectFinancingProviderAsync(int id, string payDate)
        {
            (bool Success, string Message) tokenResult;

            Tbl_BusinessPlanPayment qBusinessPlanPayment = db.Tbl_BusinessPlanPayment.FirstOrDefault(b => b.PaymentID == id);
            Tbl_BussinessPlans qBusinessPlan = db.Tbl_BussinessPlans.FirstOrDefault(b => b.BussinessPlanID == qBusinessPlanPayment.BusinessPlan_id);
            Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(a => a.UserID == qBusinessPlanPayment.PaymentUser_id);
            Tbl_UserProfiles qUserProfile = db.Tbl_UserProfiles.FirstOrDefault(a => a.User_id == qUser.UserID);

            using (HttpClient client = new HttpClient())
            {
                //Real API
                client.BaseAddress = new Uri("https://cfapi.ifb.ir/projects/");
                //string projectId = qBusinessPlan.CodeOTC;
                string projectId = "914c43ac-e70a-44e6-aa3e-8a252997fb71";
                string apiKey = "e84ef828-f196-4dce-ae77-cc7e23a2742b";
                var subUrl = projectId + "/projectfinancingprovider?apiKey=" + apiKey;

                ////Test API
                //client.BaseAddress = new Uri("http://cfapitest.ifb.ir/projects/");
                //string subUrl = "3403cbaa-911b-44c3-af6f-de3c97367627/projectfinancingprovider?apiKey=85d5ff91-0c4d-4142-beab-d734b72a40fe";

                FaraboorsJsonModel body = new FaraboorsJsonModel
                {
                    NationalID = long.Parse(qUserProfile.NationalCode),
                    IsLegal = qUser.IsLegal,
                    FirstName = qUserProfile.FirstName,
                    LastNameOrCompanyName = qUserProfile.LastName,
                    ProvidedFinancePrice = qBusinessPlanPayment.PaymentPrice * 10,
                    BourseCode = qUserProfile.SejamCode,
                    PaymentDate = payDate,
                };
                //FaraboorsJsonModel body = new FaraboorsJsonModel
                //{
                //    NationalID = 1290485941,
                //    IsLegal = false,
                //    FirstName = "443",
                //    LastNameOrCompanyName = "434",
                //    ProvidedFinancePrice = 5000,
                //    BourseCode = "456456",
                //    PaymentDate = "2021-07-14T11:48:27.974Z",
                //};

                string json = JsonConvert.SerializeObject(body);

                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(subUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    //Tbl_FaraboorsLog oFaraboorsException = new Tbl_FaraboorsLog()
                    //{
                    //    CreateDate = DateTime.Now,
                    //    Exception = null,
                    //    MobileNumber = mobileNumber,
                    //    Message = message,
                    //    ID = Guid.NewGuid().ToString(),
                    //    Method = nameof(ProjectFinancingProvider)
                    //};
                    //db.Tbl_FaraboorsLog.Add(oFaraboorsException);
                    //await db.SaveChangesAsync();
                    tokenResult = (true, responseContent);
                    return tokenResult;
                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    //Tbl_FaraboorsLog oFaraboorsException = new Tbl_FaraboorsLog()
                    //{
                    //    CreateDate = DateTime.Now,
                    //    Exception = "Response Code: " + response.StatusCode.ToString() + "Error Message: " + responseContent,
                    //    MobileNumber = mobileNumber,
                    //    Message = message,
                    //    ID = Guid.NewGuid().ToString(),
                    //    Method = nameof(AdpSendSmsAsync)
                    //};
                    //db.Tbl_FaraboorsLog.Add(oFaraboorsException);
                    //await db.SaveChangesAsync();
                    tokenResult = (false, responseContent);
                    return tokenResult;
                }
            }
        }

    }
}