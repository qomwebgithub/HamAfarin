﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Common;
using DataLayer;
using Hamafarin;
using Newtonsoft.Json;
using PagedList;
using ViewModels;


namespace HamAfarin.Areas.UserPanel.Controllers
{
    public class UserPaymentBusinessPlanController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();
        UserService userService = new UserService();
        PlanService planService = new PlanService();
        SmsService oSms = new SmsService();

        // GET: UserPanel/UserPaymentBusinessPlan
        /// <summary>
        /// لیست سرمایه گذاری های کاربر در طرح ها
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<ActionResult> Index(int page = 1)
        {
            int UserID = UserSetAuthCookie.GetUserID(User.Identity.Name);

            List<Tbl_BusinessPlanPayment> qlstBusinessPlanPayments = await db.Tbl_BusinessPlanPayment
                .Include(p => p.Tbl_PaymentOnlineDetils).Include(p => p.Tbl_BussinessPlans)
                .AsNoTracking().Where(p => p.IsDelete == false && p.PaymentUser_id == UserID)
                .OrderBy(p => p.BusinessPlan_id).OrderByDescending(p => p.PaidDateTime).ToListAsync();

            List<UserPaymentBusinessPlanList> lstUserPaymentBusinessPlan = new List<UserPaymentBusinessPlanList>();
            int Row_id = 1;

            foreach (var item in qlstBusinessPlanPayments)
            {
                if (item.PaymentType_id == 2)
                {
                    if (item.Tbl_PaymentOnlineDetils.Count == 0) continue;

                    Tbl_PaymentOnlineDetils qOnlineDetails = item.Tbl_PaymentOnlineDetils
                        .FirstOrDefault(d => item.PaymentID == d.Payment_id);

                    if ( !item.IsConfirmedFromFaraboors && (qOnlineDetails == null || !qOnlineDetails.IsFinally))
                        continue;
                }

                string strPaymentStatus = "تایید نشده";
                if (item.IsConfirmedFromAdmin) strPaymentStatus = "تایید شده";
                else if (item.IsReturned) strPaymentStatus = "برگشت خورده";
                else if (item.IsPaid) strPaymentStatus = "در انتظار تایید";

                int qRemainingDay = -1;
                if (item.Tbl_BussinessPlans.IsSuccessBussinessPlan == false)
                    qRemainingDay = planService.CalculateRemainDay(
                        item.Tbl_BussinessPlans.InvestmentExpireDate);

                int qPercentageComplate = planService.GetPercentage(
                    long.Parse(item.Tbl_BussinessPlans.AmountRequiredRoRaiseCapital),
                    planService.GetRaisedPrice(db, item.Tbl_BussinessPlans.BussinessPlanID));

                string strBusinessPlanStatus = "درحال تامین سرمایه";
                if (qRemainingDay <= 0 && qPercentageComplate >= 100) strBusinessPlanStatus = "تکمیل سرمایه";
                else if (qPercentageComplate >= 100) strBusinessPlanStatus = "در انتظار تایید فرابورس";
                else if (qRemainingDay <= 0 && qPercentageComplate < 100) strBusinessPlanStatus = "عدم تامین سرمایه";
                else if (qRemainingDay > 0) strBusinessPlanStatus = "درحال تامین سرمایه";


                lstUserPaymentBusinessPlan.Add(new UserPaymentBusinessPlanList()
                {
                    PaymentBusine_id = item.PaymentID,
                    Row_id = Row_id,
                    BusinessPlanName = item.Tbl_BussinessPlans.Title,
                    ImageName = item.Tbl_BussinessPlans.ImageNameInListPalns,
                    BusinessPlanPayment = item.PaymentPrice.Value,
                    BusinessPlanStatus = strBusinessPlanStatus,
                    PaymentStatus = strPaymentStatus,
                    CompanyName = item.Tbl_BussinessPlans.CompanyName,
                    PaymentDate = item.PaidDateTime.Value
                });
                Row_id++;
            }

            IPagedList PagedList = lstUserPaymentBusinessPlan.ToPagedList(page, 6);

            ViewBag.Count = qlstBusinessPlanPayments.Where(p => p.IsConfirmedFromAdmin).Count();

            ViewBag.DepositToInvestors = await db.Tbl_DepositToInvestorsDetails.AsNoTracking()
                .Where(p => p.InvestorUser_id == UserID && p.IsDelete == false &&
                p.Tbl_DepositToInvestors.IsPaid && p.Tbl_DepositToInvestors.IsDelete == false)
                .SumAsync(p => p.DepositAmount);

            ViewBag.TotalInvestment = qlstBusinessPlanPayments.Where(p => p.IsConfirmedFromAdmin)
                .Select(p => p.PaymentPrice).Sum();

            return View(PagedList);
        }

        public ActionResult SinglePaymentBusinessPlan(int? id, bool notify = false)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            int UserID = UserSetAuthCookie.GetUserID(User.Identity.Name);

            Tbl_BusinessPlanPayment qBusinessPlanPayment = db.Tbl_BusinessPlanPayment.FirstOrDefault(p => p.PaymentID == id && p.IsDelete == false && p.PaymentUser_id == UserID);
            if (qBusinessPlanPayment == null)
            {
                return RedirectToAction("Index");
            }
            UserPaymentBusinessPlanSingleViewModel selectPayment = new UserPaymentBusinessPlanSingleViewModel();
            Tbl_BussinessPlans tbl_BussinessPlans = db.Tbl_BussinessPlans.FirstOrDefault(p => p.BussinessPlanID == qBusinessPlanPayment.BusinessPlan_id && p.IsActive && p.IsDeleted == false);
            bool boolIsRequestedReturn = false;
            Tbl_PaymentReturned qReturned = db.Tbl_PaymentReturned.FirstOrDefault(r => r.Payment_id == id);

            // تعداد روز های باقیمانده
            int qRemainingDay = -1;
            if (tbl_BussinessPlans.IsSuccessBussinessPlan == false)
            {
                qRemainingDay = planService.CalculateRemainDay(tbl_BussinessPlans.InvestmentExpireDate);
            }

            string qRemainingText = qRemainingDay + " روز";
            if (qRemainingDay == -1)
                qRemainingText = "پایان";
            // تبدیل اعداد به انگلیسی
            string AmountRequiredRoRaiseCapital = planService.GetEnglishNumber(tbl_BussinessPlans.AmountRequiredRoRaiseCapital);
            // مبلغ سرمایه گذاری planService
            long intRaisedPrice = planService.GetRaisedPrice(db, tbl_BussinessPlans.BussinessPlanID);
            // مقدار درصد سرمایه گذاری شده
            int qPercentageComplate = planService.GetPercentageInvestmentPlan(AmountRequiredRoRaiseCapital, intRaisedPrice);
            // تعداد سرمایه گذاران
            int qInvestorCount = planService.GetPlanInvestorCount(db, tbl_BussinessPlans.BussinessPlanID);

            if (qReturned != null && qReturned.IsActive) boolIsRequestedReturn = true;
            selectPayment.BusinessPlanID = tbl_BussinessPlans.BussinessPlanID;
            selectPayment.PaymentID = qBusinessPlanPayment.PaymentID;
            selectPayment.CompanyName = tbl_BussinessPlans.CompanyName;
            selectPayment.ImageName = tbl_BussinessPlans.ImageNameInSinglePlan;
            selectPayment.BussinessName = tbl_BussinessPlans.Title;
            selectPayment.RemainingTime = qRemainingDay;
            selectPayment.PriceComplated = intRaisedPrice;
            selectPayment.WidthPercentage = qPercentageComplate + "%";
            selectPayment.PercentageComplate = qPercentageComplate;
            selectPayment.InvestorCount = qInvestorCount;
            selectPayment.FinancialDuration = tbl_BussinessPlans.Tbl_BussinessPlan_FinancialDuration?.FinancialDurationTitle;
            selectPayment.BusinessInstagramAddress = tbl_BussinessPlans.BussinessInstagramAddress;
            selectPayment.BusinessAparatAddress = tbl_BussinessPlans.BussinessAparatAddress;
            selectPayment.BusinessWebSiteAddress = tbl_BussinessPlans.BussinessWebSiteAddress;
            selectPayment.RemainingTimeText = qRemainingText;
            selectPayment.FinancialDuration_id = tbl_BussinessPlans.Tbl_BussinessPlan_FinancialDuration.FinancialDurationTitle;
            selectPayment.CodeOTC = tbl_BussinessPlans.CodeOTC;
            selectPayment.ImageNameWarranty = tbl_BussinessPlans.ImageNameWarranty;
            selectPayment.PercentageReturnInvestment = tbl_BussinessPlans.PercentageReturnInvestment;
            selectPayment.InvoiceNumber = qBusinessPlanPayment.InvoiceNumber;
            selectPayment.PercentageReturnInvestment = tbl_BussinessPlans.PercentageReturnInvestment;
            selectPayment.AmountRequiredRoRaiseCapital = tbl_BussinessPlans.AmountRequiredRoRaiseCapital;
            selectPayment.BusinessPlanPayment = qBusinessPlanPayment.PaymentPrice.Value;
            selectPayment.IsOverflowInvestment = tbl_BussinessPlans.IsOverflowInvestment;
            selectPayment.IsSuccessBussinessPlan = tbl_BussinessPlans.IsSuccessBussinessPlan;
            selectPayment.ContractFileName = tbl_BussinessPlans.ContractFileName;
            selectPayment.IsProjectParticipationReady = tbl_BussinessPlans.IsProjectParticipationReady;
            selectPayment.MobileImageName = tbl_BussinessPlans.ImageNameInListPalns;

            if (qBusinessPlanPayment.FaraboorsResponse != null &&
                qBusinessPlanPayment.FaraboorsResponse.Contains("TraceCode"))
            {
                FaraboorsResponseJsonModel faraboorsResponse = JsonConvert
                    .DeserializeObject<FaraboorsResponseJsonModel>(qBusinessPlanPayment.FaraboorsResponse);

                selectPayment.FaraboorsTraceCode = faraboorsResponse.TraceCode;
            }

            //اگر پرداخت آنلاین باشد رکورد جزییات آن را دریافت میکنیم
            if (qBusinessPlanPayment.PaymentType_id == 2)
            {
                Tbl_PaymentOnlineDetils qOnlineDetails = db.Tbl_PaymentOnlineDetils.FirstOrDefault(d => d.Payment_id == id);
                selectPayment.TransactionPaymentCode = qOnlineDetails.TransactionReferenceID;
            }
            else
            {
                selectPayment.TransactionPaymentCode = qBusinessPlanPayment.TransactionPaymentCode;
            }
            int myUserId = UserSetAuthCookie.GetUserID(User.Identity.Name);
            Tbl_UserProfiles tbl_UserProfiles = db.Tbl_UserProfiles.FirstOrDefault(u => u.User_id == myUserId);
            selectPayment.InvestorFullName = tbl_UserProfiles.FirstName + " " + tbl_UserProfiles.LastName;
            selectPayment.InvestorSejamId = "";
            selectPayment.IsOnline = true;
            selectPayment.InvestorMobile = tbl_UserProfiles.MobileNumber;
            selectPayment.InvestorNationalCode = tbl_UserProfiles.NationalCode;
            selectPayment.IsRequestedReturn = boolIsRequestedReturn;

            //string strPaymentStatus = "انصراف";
            string strPaymentStatus = "در انتظار تایید";

            int intPaymentStatusColorType = 0;
            if (qBusinessPlanPayment.IsConfirmedFromAdmin)
            {
                strPaymentStatus = "دارد";
                intPaymentStatusColorType = 1;
                selectPayment.AdminCheckDate = qBusinessPlanPayment.AdminCheckDate;
            }
            else if (qBusinessPlanPayment.IsReturned)
            {
                strPaymentStatus = "برگشت خورده";
                intPaymentStatusColorType = 2;
            }
            else if (qBusinessPlanPayment.IsPaid)
            {
                strPaymentStatus = "در انتظار تایید";
                intPaymentStatusColorType = 3;
            }
            selectPayment.PaymentStatus = strPaymentStatus;
            selectPayment.PaymentStatusColorType = intPaymentStatusColorType;
            selectPayment.PaidDateTime = qBusinessPlanPayment.PaidDateTime;
            //اجازه ی سرمایه گذاری
            selectPayment.IsAcceptInvestment = planService.IsAcceptInvestmentPlan(db, tbl_BussinessPlans.BussinessPlanID);

            ViewBag.Privacy = db.Tbl_Settings.Select(s => s.Privacy).FirstOrDefault();
            List<Tbl_BusinessPlanPayment> qlstBusinessPlanPayment = db.Tbl_BusinessPlanPayment
                 .Where(p => p.BusinessPlan_id == qBusinessPlanPayment.BusinessPlan_id &&
                     p.PaymentUser_id == qBusinessPlanPayment.PaymentUser_id &&
                     p.IsConfirmedFromAdmin &&
                     p.IsPaid && p.IsDelete == false).ToList();

            ViewBag.TotalPayment = qlstBusinessPlanPayment.Select(p => p.PaymentPrice).Sum();
            ViewBag.Count = qlstBusinessPlanPayment.Count;

            ViewBag.DepositToInvestors = db.Tbl_DepositToInvestorsDetails.Where(p => p.InvestorUser_id == UserID && p.Tbl_DepositToInvestors.Plan_id == tbl_BussinessPlans.BussinessPlanID && p.Tbl_DepositToInvestors.IsPaid && p.Tbl_DepositToInvestors.IsDelete == false).Sum(p => p.DepositAmount);
            ViewBag.Notify = notify;

            return View(selectPayment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param paymentID="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> ProjectParticipationPDF(int id)
        {
            try
            {
                if (User.Identity.IsAuthenticated == false)
                    return View();

                FaraboorsClass faraboorsClass = new FaraboorsClass();
                var apiResult = await faraboorsClass.GetProjectParticipationReportAsync(id, UserSetAuthCookie.GetUserID(User.Identity.Name));

                if (apiResult.Success == false)
                    return View();

                return File(apiResult.File, "application/pdf", "گواهی شراکت.pdf");
            }
            catch (Exception)
            {
                return View();
            }
        }

        public async Task<ActionResult> SendFaraboors(int id)
        {
            int userIdentity = UserSetAuthCookie.GetUserID(User.Identity.Name);
            int onlinePayment = 2;

            Tbl_BusinessPlanPayment qBusinessPlanPayment = await db.Tbl_BusinessPlanPayment
                .FirstOrDefaultAsync(
                    b => b.PaymentID == id &&
                    b.PaymentUser_id == userIdentity &&
                    b.IsPaid &&
                    b.PaymentType_id == onlinePayment &&
                    b.IsDelete == false &&
                    b.IsConfirmedFromFaraboors == false
                );

            if (qBusinessPlanPayment == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Date format example : "2021-07-14T11:48:27.974Z"
            string date = qBusinessPlanPayment.PaidDateTime.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

            FaraboorsClass faraboorsClass = new FaraboorsClass();
            var apiResult = await faraboorsClass.PostProjectFinancingProviderAsync(id, date);
            qBusinessPlanPayment.FaraboorsResponse = apiResult.Message;
            if (apiResult.Success)
            {
                qBusinessPlanPayment.PaymentStatus = (int)PaymentStatusType.SUCCESS;
                qBusinessPlanPayment.IsConfirmedFromAdmin = true;
                qBusinessPlanPayment.AdminCheckDate = DateTime.Now;
                AdminConfimSendSMS(qBusinessPlanPayment.BusinessPlan_id, qBusinessPlanPayment.PaymentUser_id);
                qBusinessPlanPayment.IsConfirmedFromFaraboors = true;
                qBusinessPlanPayment.FaraboorsConfirmDate = DateTime.Now;
            }
            await db.SaveChangesAsync();
            return Json(new { success = apiResult.Success, message = apiResult.Message });
        }

        private void AdminConfimSendSMS(int? businessPlan_id, int? paymentUser_id)
        {
            // 5 = تایید سرمایه گذاری توسط ادمین
            string message = db.Tbl_Sms.Find(5).Message;
            if (message.Contains("@T"))
            {
                Tbl_BussinessPlans qBussinessPlan = db.Tbl_BussinessPlans.FirstOrDefault(b => b.BussinessPlanID == businessPlan_id && b.IsActive && b.IsDeleted == false);
                message = message.Replace("@T", qBussinessPlan.Title);
            }

            Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.UserID == paymentUser_id);
            oSms.SendSms(qUser.MobileNumber, message);
        }

        public ActionResult CertificateList()
        {
            int UserID = UserSetAuthCookie.GetUserID(User.Identity.Name);
            var CertifiedCertificate = db.Tbl_BusinessPlanPayment.Where(b => b.IsConfirmedFromFaraboors && b.IsDelete == false && b.PaymentUser_id == UserID && b.Tbl_BussinessPlans.IsProjectParticipationReady).ToList();
            List<ProjectParticipationViewModel> projectParticipations = new List<ProjectParticipationViewModel>();
            foreach (var item in CertifiedCertificate)
            {
                projectParticipations.Add(new ProjectParticipationViewModel()
                {
                    PaymentID = item.PaymentID,
                    CreateDate = item.CreateDate,
                    PaymentPrice = item.PaymentPrice,
                    Title = item.Tbl_BussinessPlans.Title
                });
            }
            return View(projectParticipations);
        }
    }
}