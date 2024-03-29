﻿using Common;
using DataLayer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HamAfarin
{
    public class FaraboorsClass
    {
        SmsService oSms = new SmsService();
        private const string _apiKey = "e84ef828-f196-4dce-ae77-cc7e23a2742b";

        HamAfarinDBEntities db = new HamAfarinDBEntities();

        public async Task<(bool Success, byte[] File)> GetProjectParticipationReportAsync(int id, int user)
        {

            Tbl_BusinessPlanPayment qBusinessPlanPayment = db.Tbl_BusinessPlanPayment
                .FirstOrDefault(b => b.PaymentID == id && b.IsDelete == false);

            if (user != qBusinessPlanPayment.PaymentUser_id)
                return (false, null);

            string projectId = db.Tbl_BussinessPlans
                .Where(b => b.BussinessPlanID == qBusinessPlanPayment.BusinessPlan_id)
                .Select(b => b.FaraboorsProjectId).FirstOrDefault();

            bool isLegal = db.Tbl_Users
               .Where(u => u.UserID == qBusinessPlanPayment.PaymentUser_id)
               .Select(u => u.IsLegal).FirstOrDefault();

            string nationalID;
            if (isLegal)
            {
                nationalID = db.Tbl_PersonLegal
                    .Where(p => p.User_id == qBusinessPlanPayment.PaymentUser_id)
                    .Select(p => p.NationalId).FirstOrDefault();
            }
            else
            {
                nationalID = db.Tbl_UserProfiles
                    .Where(a => a.User_id == qBusinessPlanPayment.PaymentUser_id)
                    .Select(u => u.NationalCode).FirstOrDefault();
            }

            using (HttpClient client = new HttpClient())
            {
                //Real API
                client.BaseAddress = new Uri("https://cfapi.ifb.ir/projects/");
                //string projectId = "914c43ac-e70a-44e6-aa3e-8a252997fb71";
                //var subUrl = $"GetProjectParticipationReport?apiKey={_apiKey}&projectId={projectId}&nationalID={nationalID}";

                //Test API
                //client.BaseAddress = new Uri("http://cfapitest.ifb.ir/projects/");
                //string subUrl = "GetProjectParticipationReport?apiKey=85d5ff91-0c4d-4142-beab-d734b72a40fe&projectId=3403cbaa-911b-44c3-af6f-de3c97367627&nationalID=" + nationalID;

                var subUrl = "GetProjectParticipationReport";

                ListDictionary body = new ListDictionary();
                body.Add("ApiKey", _apiKey);
                body.Add("projectID", projectId);
                body.Add("NationalID", long.Parse(nationalID));

                string json = JsonConvert.SerializeObject(body);

                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(subUrl, content);

                if (!response.IsSuccessStatusCode)
                    return (false, null);

                byte[] responseContent = await response.Content.ReadAsByteArrayAsync();
                return (true, responseContent);
            }
        }

        public async Task<(bool Success, string Message)> PostProjectFinancingProviderAsync(int id, string payDate)
        {
            try
            {
                Tbl_BusinessPlanPayment qBusinessPlanPayment = db.Tbl_BusinessPlanPayment.FirstOrDefault(b => b.PaymentID == id && b.IsDelete == false);
                Tbl_UserProfiles qUserProfile = db.Tbl_UserProfiles.FirstOrDefault(a => a.User_id == qBusinessPlanPayment.PaymentUser_id);
                Tbl_PersonLegal qPersonLegal = db.Tbl_PersonLegal.FirstOrDefault(p => p.User_id == qBusinessPlanPayment.PaymentUser_id);
                bool isLegal = db.Tbl_Users
                   .Where(u => u.UserID == qBusinessPlanPayment.PaymentUser_id)
                   .Select(u => u.IsLegal)
                   .FirstOrDefault();

                using (HttpClient client = new HttpClient())
                {
                    //Real API
                    client.BaseAddress = new Uri("https://cfapi.ifb.ir/projects/");

                    string projectId = db.Tbl_BussinessPlans
                        .Where(b => b.BussinessPlanID == qBusinessPlanPayment.BusinessPlan_id)
                        .Select(b => b.FaraboorsProjectId)
                        .FirstOrDefault();

                    var subUrl = $"projectfinancingprovider";

                    #region FormatExample
                    // Test API
                    //client.BaseAddress = new Uri("http://cfapitest.ifb.ir/projects/");
                    //string projectId = "914c43ac-e70a-44e6-aa3e-8a252997fb71";

                    //string subUrl = "3403cbaa-911b-44c3-af6f-de3c97367627/projectfinancingprovider?apiKey=85d5ff91-0c4d-4142-beab-d734b72a40fe";

                    //FaraboorsJsonModel body = new FaraboorsJsonModel
                    //{
                    //    NationalID = 1290485941,
                    //    IsLegal = false,
                    //    FirstName = "443",
                    //    LastNameOrCompanyName = "434",
                    //    ProvidedFinancePrice = 5000,
                    //    BourseCode = "456456",
                    //    PaymentDate = "2021-07-14T11:48:27.974Z",
                    //    ApiKey: "85d5ff91-0c4d-4142-beab-d734b72a40fe",
                    //    ProjectId: "3403cbaa-911b-44c3-af6f-de3c97367627"
                    //};
                    #endregion

                    ProjectFinancingProviderDto body = new ProjectFinancingProviderDto();

                    body.ApiKey = _apiKey;
                    body.ProjectID = projectId;
                    body.NationalID = Convert.ToInt64(StringExtensions.Fa2En(Convert.ToString(isLegal ? long.Parse(StringExtensions.Fa2En(qPersonLegal.NationalId)) : long.Parse(qUserProfile.NationalCode))));
                    body.IsLegal = isLegal;
                    body.FirstName = isLegal ? "" : qUserProfile.FirstName;
                    body.LastNameOrCompanyName = isLegal ? qPersonLegal.CompanyName : qUserProfile.LastName;
                    body.ProvidedFinancePrice = qBusinessPlanPayment.PaymentPrice * 10;
                    body.BourseCode = qUserProfile.SejamCode;
                    body.PaymentDate = payDate;
                    body.BankTrackingNumber = qBusinessPlanPayment.TransactionPaymentCode;
                    body.ShebaBankAccountNumber = qUserProfile.AccountSheba;

                    //شماره دارای کد کشور است یا خیر چک شود
                    if (qUserProfile.MobileNumber.Substring(0, 2) == "98")
                        body.MobileNumber = "0" + qUserProfile.MobileNumber.Substring(2);
                    else
                        body.MobileNumber = qUserProfile.MobileNumber;

                    string json = JsonConvert.SerializeObject(body);

                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(subUrl, content);
                    string responseContent = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                        return (false, responseContent);

                    return (true, responseContent);
                }
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }
        }

        public async Task<(bool Success, string Message)> GetProjectInfoAsync(string projectID)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    //Real API
                    client.BaseAddress = new Uri("https://cfapi.ifb.ir/projects/");
                    var subUrl = "GetProjectInfo";

                    var body = new JObject();
                    body.Add("ApiKey", _apiKey);
                    body.Add("projectID", projectID);

                    #region TestCode
                    //object body = new
                    //{
                    //    ApiKey = _apiKey,
                    //    projectID = projectID
                    //};

                    //dynamic body = new JObject();
                    //body.ApiKey = _apiKey;
                    //body.projectID = projectID;


                    //var request = new HttpRequestMessage
                    //{
                    //    Method = HttpMethod.Get,
                    //    RequestUri = new Uri("https://cfapi.ifb.ir/projects/GetProjectInfo"),
                    //    Content = new StringContent(json, Encoding.UTF8, "application/json")
                    //};

                    //StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    //HttpResponseMessage response = await client.GetAsync(subUrl, content);
                    #endregion

                    string json = JsonConvert.SerializeObject(body);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(subUrl, content);
                    string responseBody = await response.Content.ReadAsStringAsync();


                    if (!response.IsSuccessStatusCode)
                        return (false, responseBody);

                    return (true, responseBody);
                }
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }
        }
    }
}