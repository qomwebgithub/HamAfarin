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

        public async Task<(bool Success, string Message)> ProjectFinancingProviderAsync(int id)
        {
            (bool Success, string Message) tokenResult;

            Tbl_BusinessPlanPayment qBusinessPlanPayment = db.Tbl_BusinessPlanPayment.FirstOrDefault(b => b.PaymentID == id);
            Tbl_BussinessPlans qBusinessPlan = db.Tbl_BussinessPlans.FirstOrDefault(b => b.BussinessPlanID == qBusinessPlanPayment.BusinessPlan_id);
            Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(a => a.UserID == qBusinessPlanPayment.PaymentUser_id);
            Tbl_UserProfiles qUserProfile = db.Tbl_UserProfiles.FirstOrDefault(a => a.User_id == qUser.UserID);

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://cfapi.ifb.ir/projects/");

                var projectId = qBusinessPlan.CodeOTC;
                var apiKey = "85d5ff91-0c4d-4142-beab-d734b72a40fe";
                var subUrl = projectId + "/projectfinancingprovider?apiKey=" + apiKey;

                FaraboorsJsonModel body = new FaraboorsJsonModel
                {
                    NationalID = int.Parse(qUserProfile.NationalCode),
                    IsLegal = qUser.IsLegal,
                    FirstName = qUserProfile.FirstName,
                    LastNameOrCompanyName = qUserProfile.LastName,
                    ProvidedFinancePrice = qBusinessPlanPayment.PaymentPrice,
                    BourseCode = qUserProfile.SejamCode,
                    PaymentDate = qBusinessPlanPayment.PaidDateTime.ToString(),
                };

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