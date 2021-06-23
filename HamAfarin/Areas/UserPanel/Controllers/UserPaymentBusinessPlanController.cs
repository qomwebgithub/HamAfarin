﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using Hamafarin;
using PagedList;
using ViewModels;

namespace HamAfarin.Areas.UserPanel.Controllers
{
    public class UserPaymentBusinessPlanController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();
        UserService userService = new UserService();
        PlanService planService = new PlanService();

        // GET: UserPanel/UserPaymentBusinessPlan
        /// <summary>
        /// لیست سرمایه گذاری های کاربر در طرح ها
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index(int page = 1)
        {
            // Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.MobileNumber == User.Identity.Name);
            int UserID = UserSetAuthCookie.GetUserID(User.Identity.Name);
            List<Tbl_BusinessPlanPayment> qlstBusinessPlanPayments = db.Tbl_BusinessPlanPayment.Where(p => p.IsDelete == false && p.PaymentUser_id == UserID).OrderBy(p => p.BusinessPlan_id).OrderByDescending(p => p.PaidDateTime).ToList();
            List<UserPaymentBusinessPlanList> lstUserPaymentBusinessPlan = new List<UserPaymentBusinessPlanList>();
            int Row_id = 1;
            foreach (var item in qlstBusinessPlanPayments)
            {
                if (item.PaymentType_id == 2)
                {
                    if (item.Tbl_PaymentOnlineDetils.Count == 0) continue;
                    Tbl_PaymentOnlineDetils qOnlineDetails = item.Tbl_PaymentOnlineDetils.First(d => item.PaymentID == d.Payment_id);
                    if (qOnlineDetails == null || !qOnlineDetails.IsFinally)
                        continue;
                }
                string strPaymentStatus = "تایید نشده";
                if (item.IsConfirmedFromAdmin)
                {
                    strPaymentStatus = "تایید شده";
                }
                else if (item.IsReturned)
                {
                    strPaymentStatus = "برگشت خورده";
                }
                else if (item.IsPaid)
                {
                    strPaymentStatus = "در انتظار تایید";
                }
                string strBusinessPlanStatus = "درحال تامین سرمایه";
                int qRemainingDay = planService.calculateRemainDay(item.Tbl_BussinessPlans);
                int qPercentageComplate = planService.GetPercentage(long.Parse(item.Tbl_BussinessPlans.AmountRequiredRoRaiseCapital),
                    planService.GetRaisedPrice(db, item.Tbl_BussinessPlans.BussinessPlanID));

                if (qRemainingDay > 0)
                {
                    strBusinessPlanStatus = "درحال تامین سرمایه";
                }
                else if (qRemainingDay <= 0 && qPercentageComplate >= 100)
                {
                    strBusinessPlanStatus = "تکمیل سرمایه";
                }
                else if (qRemainingDay <= 0 && qPercentageComplate < 100)
                {
                    strBusinessPlanStatus = "عدم تامین سرمایه";
                }
                else if (qRemainingDay <= 0 && qPercentageComplate >= 100)
                {
                    strBusinessPlanStatus = "شروع طرح";
                }
                else if (item.Tbl_BussinessPlans.IsSuccessBussinessPlan)
                {
                    strBusinessPlanStatus = "پایان طرح";
                }

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
            ViewBag.Count = lstUserPaymentBusinessPlan.Count();
            return View(PagedList);
        }

        public ActionResult SinglePaymentBusinessPlan(int id, bool notify= false)
        {
            Tbl_BusinessPlanPayment qBusinessPlanPayment = db.Tbl_BusinessPlanPayment.FirstOrDefault(p => p.PaymentID == id);
            UserPaymentBusinessPlanSingleViewModel selectPayment = new UserPaymentBusinessPlanSingleViewModel();
            Tbl_BussinessPlans tbl_BussinessPlans = db.Tbl_BussinessPlans.FirstOrDefault(p => p.BussinessPlanID == qBusinessPlanPayment.BusinessPlan_id); ;
            bool boolIsRequestedReturn = false;
            Tbl_PaymentReturned qReturned = db.Tbl_PaymentReturned.FirstOrDefault(r => r.Payment_id == id);
            // تعداد روز های باقیمانده
            int qRemainingDay = planService.calculateRemainDay(tbl_BussinessPlans);
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

            //اگر پرداخت آنلاین باشد رکورد جزییات آن را دریافت میکنیم
            if (qBusinessPlanPayment.PaymentType_id == 2)
            {
               Tbl_PaymentOnlineDetils qOnlineDetails= db.Tbl_PaymentOnlineDetils.FirstOrDefault(d => d.Payment_id == id);
                selectPayment.TransactionPaymentCode = qOnlineDetails.TransactionReferenceID;
            }
            else { 
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

            string strPaymentStatus = "انصراف";
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

            ViewBag.Notify = notify;

            return View(selectPayment);
        }

    }
}