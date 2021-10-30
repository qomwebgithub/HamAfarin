using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels;
using Common;
using AutoMapper;

namespace HamAfarin.Areas.Admin.Controllers
{
    public class Tbl_DepositToInvestorsController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: UserPanel/DepositToInvestors
        public ActionResult Index()
        {
            List<Tbl_DepositToInvestors> listTbl_DepositToInvestors = db.Tbl_DepositToInvestors
                .Where(d => d.IsDelete == false)
                .ToList();

            return View(listTbl_DepositToInvestors);
        }

        public ActionResult Details(int id)
        {
            var listInvestorsViewModel = from u in db.Tbl_Users
                                         join UserProfiles in db.Tbl_UserProfiles
                                              on u.UserID equals UserProfiles.User_id into UserGroup
                                         from p in UserGroup.DefaultIfEmpty()
                                         join DepositToInvestorsDetails in db.Tbl_DepositToInvestorsDetails
                                              on p.User_id equals DepositToInvestorsDetails.InvestorUser_id into InvestorGroup
                                         from i in InvestorGroup.DefaultIfEmpty()
                                         where i.Deposit_id == id
                                         select new InvestorViewModel
                                         {
                                             UserID = u.UserID,
                                             FirstName = p.FirstName,
                                             LastName = p.LastName,
                                             MobileNumber = p.MobileNumber,
                                             Shaba = p.AccountSheba,
                                             DepositAmount = (long)i.DepositAmount
                                         };

            return View(listInvestorsViewModel.ToList());
        }

        public ActionResult Create()
        {
            DepositToInvestorsViewModel depositToInvestors = new DepositToInvestorsViewModel();

            depositToInvestors.PlansList = db.Tbl_BussinessPlans
                .Where(p => p.IsActive && p.IsDeleted == false)
                .OrderByDescending(p=> p.CreateDate)
                .Select(p => new PlanViewModel { PlanID = p.BussinessPlanID, PlanName = p.Title })
                .ToList();

            depositToInvestors.DepositTypesList = db.Tbl_DepositTypes
                .Select(p => new DepositTypeViewModel { TypeID = p.DepositTypeID, TypeName = p.DepositTypeName })
                .ToList();

            return View(depositToInvestors);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DepositToInvestorsViewModel depositToInvestors)
        {
            depositToInvestors.DepositDate = StringExtensions.StringToDate(depositToInvestors.StringDepositDate);
            depositToInvestors.PlansList = db.Tbl_BussinessPlans
                .Where(p => p.IsActive && p.IsDeleted == false)
                .Select(p => new PlanViewModel { PlanID = p.BussinessPlanID, PlanName = p.Title })
                .ToList();
            depositToInvestors.DepositTypesList = db.Tbl_DepositTypes
                .Select(p => new DepositTypeViewModel { TypeID = p.DepositTypeID, TypeName = p.DepositTypeName })
                .ToList();

            if (ModelState.IsValid == false)
            {
                return View(depositToInvestors);
            }

            if (depositToInvestors.YieldPercent <= 0)
            {
                ModelState.AddModelError("YieldPercent", "درصد واریز باید بزرگتر از 0 باشد");
                return View(depositToInvestors);
            }

            int userIdentity = UserSetAuthCookie.GetUserID(User.Identity.Name);
            DateTime dateTimeNow = DateTime.Now;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DepositToInvestorsViewModel, Tbl_DepositToInvestors>();
            });
            IMapper iMapper = config.CreateMapper();
            var tblDepositToInvestors = iMapper.Map<DepositToInvestorsViewModel, Tbl_DepositToInvestors>(depositToInvestors);
            tblDepositToInvestors.IsDelete = false;
            tblDepositToInvestors.CreateUser_id = userIdentity;
            tblDepositToInvestors.CreateDate = dateTimeNow;
            if (tblDepositToInvestors.IsPaid)
                tblDepositToInvestors.PaidDate = dateTimeNow;

            db.Tbl_DepositToInvestors.Add(tblDepositToInvestors);
            db.SaveChanges();

            int depositID = tblDepositToInvestors.DepositID;

            //در صورت به خطا خوردن مرحله دوم ذخیره، اطلاعات مرحله اول پاک شود
            try
            {
                List<InvestorViewModel> listInvestorsViewModel = db.Tbl_BusinessPlanPayment
                    .Where(b => b.IsDelete == false &&
                        b.BusinessPlan_id == depositToInvestors.Plan_id &&
                        b.IsConfirmedFromFaraboors)
                    .GroupBy(c => c.PaymentUser_id)
                    .Select(g => new InvestorViewModel
                    {
                        UserID = (int)g.Key,
                        FirstName = g.Select(a => a.Tbl_Users.Tbl_UserProfiles.Select(u => u.FirstName).FirstOrDefault()).FirstOrDefault(),
                        LastName = g.Select(a => a.Tbl_Users.Tbl_UserProfiles.Select(u => u.LastName).FirstOrDefault()).FirstOrDefault(),
                        MobileNumber = g.Select(a => a.Tbl_Users.Tbl_UserProfiles.Select(u => u.MobileNumber).FirstOrDefault()).FirstOrDefault(),
                        Shaba = g.Select(a => a.Tbl_Users.Tbl_UserProfiles.Select(u => u.AccountSheba).FirstOrDefault()).FirstOrDefault(),
                        TotalPaymentPrice = (long)g.Sum(b => b.PaymentPrice),
                        DepositAmount = (long)(depositToInvestors.YieldPercent / 100 * (decimal)g.Sum(b => b.PaymentPrice))
                    })
                    .ToList();

                var listTbl_DepositToInvestorsDetails = new List<Tbl_DepositToInvestorsDetails>();
                foreach (InvestorViewModel investor in listInvestorsViewModel)
                {
                    listTbl_DepositToInvestorsDetails.Add(new Tbl_DepositToInvestorsDetails()
                    {
                        IsDelete = false,
                        Deposit_id = tblDepositToInvestors.DepositID,
                        InvestorUser_id = investor.UserID,
                        DepositAmount = investor.DepositAmount,
                        CreateDate = dateTimeNow,
                    });
                }

                long? totalDepositAmount = listTbl_DepositToInvestorsDetails.Select(d => d.DepositAmount).Sum();
                tblDepositToInvestors.TotalDeposit = totalDepositAmount;
                db.Tbl_DepositToInvestorsDetails.AddRange(listTbl_DepositToInvestorsDetails);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                db.Tbl_DepositToInvestors.Remove(db.Tbl_DepositToInvestors.Find(depositID));
                db.SaveChanges();
                throw e;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult DepositCalculation(int id, decimal percent = 0)
        {
            List<InvestorViewModel> listInvestorsViewModel = db.Tbl_BusinessPlanPayment
                .Where(b => b.IsDelete == false &&
                    b.BusinessPlan_id == id &&
                    b.IsConfirmedFromFaraboors)
                .GroupBy(c => c.PaymentUser_id)
                .Select(g => new InvestorViewModel
                {
                    UserID = (int)g.Key,
                    FirstName = g.Select(a => a.Tbl_Users.Tbl_UserProfiles.Select(u => u.FirstName).FirstOrDefault()).FirstOrDefault(),
                    LastName = g.Select(a => a.Tbl_Users.Tbl_UserProfiles.Select(u => u.LastName).FirstOrDefault()).FirstOrDefault(),
                    MobileNumber = g.Select(a => a.Tbl_Users.Tbl_UserProfiles.Select(u => u.MobileNumber).FirstOrDefault()).FirstOrDefault(),
                    Shaba = g.Select(a => a.Tbl_Users.Tbl_UserProfiles.Select(u => u.AccountSheba).FirstOrDefault()).FirstOrDefault(),
                    TotalPaymentPrice = (long)g.Sum(b => b.PaymentPrice),
                    DepositAmount = (long)(percent / 100 * (decimal)g.Sum(b => b.PaymentPrice))
                })
                .ToList();

            return Json(listInvestorsViewModel, JsonRequestBehavior.AllowGet);
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