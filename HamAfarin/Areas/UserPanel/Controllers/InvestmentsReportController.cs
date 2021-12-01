using DataLayer;
using HamAfarin.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ViewModels;

namespace HamAfarin.Areas.UserPanel.Controllers
{
    public class InvestmentsReportController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: UserPanel/InvestmentsReport
        public ActionResult Index()
        {
            InvestmentsReportViewModel investmentsReport = new InvestmentsReportViewModel();
            int userIdentity = UserSetAuthCookie.GetUserID(User.Identity.Name);

            investmentsReport.PlansList = db.Tbl_BussinessPlans
                .Where(p => p.IsActive && p.IsDeleted == false)
                .OrderByDescending(p => p.CreateDate)
                .Select(p => new PlanViewModel { PlanID = p.BussinessPlanID, PlanName = p.Title })
                .ToList();

            investmentsReport.InvestmentReportTypes = db.Tbl_DepositTypes
                .Select(p => new DepositTypeViewModel { TypeID = p.DepositTypeID, TypeName = p.DepositTypeName })
                .ToList();

            investmentsReport.InvestmentReportTypes.Add(new DepositTypeViewModel { TypeID = 4, TypeName = "سرمایه گذاری" });

            investmentsReport.Investments = db.Tbl_DepositToInvestorsDetails
                .Where(d => d.InvestorUser_id == userIdentity && d.Tbl_DepositToInvestors.IsPaid && d.IsDelete == false)
                .Select(d => new InvestmentDetailViewModel
                {
                    ID = "d" + d.DepositDetailID.ToString(),
                    TypeID = d.Tbl_DepositToInvestors.DepositType_id,
                    TypeName = d.Tbl_DepositToInvestors.Tbl_DepositTypes.DepositTypeName,
                    Date = d.Tbl_DepositToInvestors.DepositDate,
                    Amount = d.DepositAmount,
                    PlanID = d.Tbl_DepositToInvestors.Plan_id,
                    PlanName = d.Tbl_DepositToInvestors.Tbl_BussinessPlans.Title,
                })
                .ToList();

            List<InvestmentDetailViewModel> listPayments = db.Tbl_BusinessPlanPayment
                .Where(b => b.PaymentUser_id == userIdentity && b.IsConfirmedFromFaraboors && b.IsDelete == false)
                .Select(b => new InvestmentDetailViewModel
                {
                    ID = "p" + b.PaymentID.ToString(),
                    TypeID = 4,
                    TypeName = "سرمایه گذاری",
                    Date = b.PaidDateTime,
                    Amount = b.PaymentPrice,
                    PlanID = b.BusinessPlan_id,
                    PlanName = b.Tbl_BussinessPlans.Title,
                })
                .ToList();

            investmentsReport.Investments.AddRange(listPayments);

            investmentsReport.Investments = investmentsReport.Investments.OrderByDescending(i => i.Date).ToList();

            investmentsReport.DepositUrl = $"/{nameof(UserPanel)}/{nameof(InvestmentsReportController).ControllerName()}/{nameof(DepositDetails)}/";
            investmentsReport.PaymentUrl = $"/{nameof(UserPanel)}/{nameof(UserPaymentBusinessPlanController).ControllerName()}/{nameof(UserPaymentBusinessPlanController.SinglePaymentBusinessPlan)}/";
            return View(investmentsReport);
        }

        public ActionResult DepositDetails(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            int userIdentity = UserSetAuthCookie.GetUserID(User.Identity.Name);

            var investmentDetailViewModel = db.Tbl_DepositToInvestorsDetails
                .Where(d =>
                    d.DepositDetailID == id &&
                    d.InvestorUser_id == userIdentity &&
                    d.Tbl_DepositToInvestors.IsPaid &&
                    d.IsDelete == false)
                .Select(d => new InvestmentDetailViewModel
                {
                    TypeName = d.Tbl_DepositToInvestors.Tbl_DepositTypes.DepositTypeName,
                    Amount = d.DepositAmount,
                    PlanName = d.Tbl_DepositToInvestors.Tbl_BussinessPlans.Title,
                    Date = d.Tbl_DepositToInvestors.DepositDate,
                    Description = d.Tbl_DepositToInvestors.Description,
                })
                .FirstOrDefault();

            if (investmentDetailViewModel == null)
                return HttpNotFound();

            investmentDetailViewModel.DateString = investmentDetailViewModel.Date.Value.ToString("yyyy/MM/dd");
            investmentDetailViewModel.Sheba = db.Tbl_UserProfiles
                .Where(d => d.User_id == userIdentity)
                .Select(d => d.AccountSheba)
                .FirstOrDefault();

            return View(investmentDetailViewModel);
        }

        [HttpGet]
        public ActionResult Investments(int? planId, int? typeId)
        {
            int userIdentity = UserSetAuthCookie.GetUserID(User.Identity.Name);

            List<InvestmentDetailViewModel> listInvestments = db.Tbl_DepositToInvestorsDetails
                .Where(d => d.InvestorUser_id == userIdentity && d.Tbl_DepositToInvestors.IsPaid && d.IsDelete == false)
                .Select(d => new InvestmentDetailViewModel
                {
                    ID = "d" + d.DepositDetailID.ToString(),
                    TypeID = d.Tbl_DepositToInvestors.DepositType_id,
                    TypeName = d.Tbl_DepositToInvestors.Tbl_DepositTypes.DepositTypeName,
                    Date = d.Tbl_DepositToInvestors.DepositDate,
                    Amount = d.DepositAmount,
                    PlanID = d.Tbl_DepositToInvestors.Plan_id,
                    PlanName = d.Tbl_DepositToInvestors.Tbl_BussinessPlans.Title,
                })
                .ToList();

            List<InvestmentDetailViewModel> listPayments = db.Tbl_BusinessPlanPayment
                .Where(b => b.PaymentUser_id == userIdentity && b.IsConfirmedFromFaraboors && b.IsDelete == false)
                .Select(b => new InvestmentDetailViewModel
                {
                    ID = "p" + b.PaymentID.ToString(),
                    TypeID = 4,
                    TypeName = "سرمایه گذاری",
                    Date = b.PaidDateTime,
                    Amount = b.PaymentPrice,
                    PlanID = b.BusinessPlan_id,
                    PlanName = b.Tbl_BussinessPlans.Title,
                })
                .ToList();

            listInvestments.AddRange(listPayments);

            if (planId.HasValue)
                listInvestments = listInvestments.Where(i => i.PlanID == planId).ToList();

            if (typeId.HasValue)
                listInvestments = listInvestments.Where(i => i.TypeID == typeId).ToList();

            foreach (InvestmentDetailViewModel investment in listInvestments)
                investment.DateString = investment.Date.Value.ToString("yyyy-MM-dd");

            listInvestments = listInvestments
                .Select(i => new InvestmentDetailViewModel
                {
                    ID = i.ID,
                    TypeName = i.TypeName,
                    Amount = i.Amount,
                    DateString = i.DateString,
                    PlanName = i.PlanName,
                })
                .ToList();

            return Json(new { data = listInvestments }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}