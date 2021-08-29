using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using Hamafarin;
using InsertShowImage;
using KooyWebApp_MVC.Classes;
using ViewModels;
using AutoMapper;
using Common;

namespace HamAfarin.Areas.UserPanel.Controllers
{
    public class Tbl_BussinessPlansController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();
        UserService userService = new UserService();
        PlanService planService = new PlanService();

        // GET: UserPanel/Tbl_BussinessPlans
        public ActionResult Index()
        {
            int myUserId = UserSetAuthCookie.GetUserID(User.Identity.Name);
            var tbl_BussinessPlans = db.Tbl_BussinessPlans.Where(p => p.User_id == myUserId).Include(t => t.Tbl_BussinessPlan_BussenessFields).Include(t => t.Tbl_BussinessPlan_FinancialDuration).Include(t => t.Tbl_CompanyType).Include(t => t.Tbl_MonetaryUnits).Include(t => t.Tbl_Users);
            List<UserBusinessPlanViewModel> userBusinessPlanViewModels = new List<UserBusinessPlanViewModel>();
            foreach (Tbl_BussinessPlans item in tbl_BussinessPlans)
            {
                userBusinessPlanViewModels.Add(new UserBusinessPlanViewModel()
                {
                    BussinessPlanID = item.BussinessPlanID,
                    Title = item.Title,
                    ImageNameInListPalns = item.ImageNameInListPalns,
                    IsActive = item.IsActive,
                    TotalInvestmentPrice = StringExtensions.En2Fa(@Convert.ToDecimal(planService.GetGoalPrice(db, item.BussinessPlanID)).ToString("#,##0")) + " تومان"

                });
            }
            return View(userBusinessPlanViewModels.ToList());
        }

        // GET: UserPanel/Tbl_BussinessPlans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_BussinessPlans tbl_BussinessPlans = db.Tbl_BussinessPlans.Find(id);
            if (tbl_BussinessPlans == null)
            {
                return HttpNotFound();
            }
            return View(tbl_BussinessPlans);
        }

        // GET: UserPanel/Tbl_BussinessPlans/Create
        public ActionResult Create()
        {
            ViewBag.BussinessField_id = new SelectList(db.Tbl_BussinessPlan_BussenessFields, "BussinessFieldID", "BussinessFieldTitle");
            ViewBag.FinancialDuration_id = new SelectList(db.Tbl_BussinessPlan_FinancialDuration, "FinancialDurationID", "FinancialDurationTitle");
            ViewBag.CompanyType_id = new SelectList(db.Tbl_CompanyType, "CompanyTypeID", "CompanyTypeTitle");
            ViewBag.MonetaryUnit_id = new SelectList(db.Tbl_MonetaryUnits, "MonetaryUnitID", "MonetaryUnitTitle");
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName");
            return View();
        }

        // POST: UserPanel/Tbl_BussinessPlans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserCreateBusinessPlan userCreateBusinessPlan, HttpPostedFileBase imgInListPalns, HttpPostedFileBase imgInSinglePlan, HttpPostedFileBase imgLogo,
           HttpPostedFileBase imgNationalCard, HttpPostedFileBase letterFile, HttpPostedFileBase modelFile, HttpPostedFileBase slideFile, HttpPostedFileBase reportFile,
            HttpPostedFileBase ideaFile)
        {
            ////////////****************/////////////////////////////
            ////////////****************/////////////////////////////
            // تبدیل تاریخ از 
            // string   
            // به
            // datetime
            // PersianCalendar pc = new PersianCalendar();
            //    string strDateTime = userCreateBusinessPlan.InvestmentStartDate.ToString();
            //    string[] parts = strDateTime.Split('/', '-');
            //     userCreateBusinessPlan.InvestmentStartDate = pc.ToDateTime(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]), 0, 0, 0, 0);
            ////////////****************/////////////////////////////
            ////////////****************/////////////////////////////
            ///
            userCreateBusinessPlan.User_id = UserSetAuthCookie.GetUserID(User.Identity.Name);
            userCreateBusinessPlan.ShortDescription = "";
            if (ModelState.IsValid)
            {
                userCreateBusinessPlan.ImageNameInListPalns = "no-photo.jpg";
                if (imgInListPalns != null && imgInListPalns.IsImage())
                {
                    userCreateBusinessPlan.ImageNameInListPalns = Guid.NewGuid().ToString() + Path.GetExtension(imgInListPalns.FileName);
                    imgInListPalns.SaveAs(Server.MapPath("/Images/BusinessPlans/Image/" + userCreateBusinessPlan.ImageNameInListPalns));

                    ImageResizer img = new ImageResizer(500);
                    img.Resize(Server.MapPath("/Images/BusinessPlans/Image/" + userCreateBusinessPlan.ImageNameInListPalns),
                        Server.MapPath("/Images/BusinessPlans/Thumb/" + userCreateBusinessPlan.ImageNameInListPalns));

                }

                userCreateBusinessPlan.ImageNameInSinglePlan = "no-photo.jpg";
                if (imgInSinglePlan != null && imgInSinglePlan.IsImage())
                {
                    userCreateBusinessPlan.ImageNameInSinglePlan = Guid.NewGuid().ToString() + Path.GetExtension(imgInSinglePlan.FileName);
                    imgInSinglePlan.SaveAs(Server.MapPath("/Images/BusinessPlans/Image/" + userCreateBusinessPlan.ImageNameInSinglePlan));

                }

                if (imgLogo != null && imgLogo.IsImage())
                {
                    userCreateBusinessPlan.BussinessLogoImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgLogo.FileName);
                    imgLogo.SaveAs(Server.MapPath("/Images/BusinessPlans/Logo/Image/" + userCreateBusinessPlan.BussinessLogoImageName));

                    ImageResizer img = new ImageResizer(500);
                    img.Resize(Server.MapPath("/Images/BusinessPlans/Logo/Image/" + userCreateBusinessPlan.BussinessLogoImageName),
                        Server.MapPath("/Images/BusinessPlans/Logo/Thumb/" + userCreateBusinessPlan.BussinessLogoImageName));

                }

                if (imgNationalCard != null && imgNationalCard.IsImage())
                {
                    userCreateBusinessPlan.CompanyAgentNationalCardImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgNationalCard.FileName);
                    imgNationalCard.SaveAs(Server.MapPath("/Resources/BusinessPlans/NationalCard/" + userCreateBusinessPlan.CompanyAgentNationalCardImageName));
                }

                if (letterFile != null)
                {
                    userCreateBusinessPlan.CompanyIntroductionLetterFileName = Guid.NewGuid().ToString() + Path.GetExtension(letterFile.FileName);
                    letterFile.SaveAs(Server.MapPath("/Resources/BusinessPlans/Letter/" + userCreateBusinessPlan.CompanyIntroductionLetterFileName));
                }

                if (modelFile != null)
                {
                    userCreateBusinessPlan.BussinessModelFileName = Guid.NewGuid().ToString() + Path.GetExtension(modelFile.FileName);
                    modelFile.SaveAs(Server.MapPath("/Resources/BusinessPlans/Model/" + userCreateBusinessPlan.BussinessModelFileName));
                }

                if (slideFile != null)
                {
                    userCreateBusinessPlan.SlideShowPresentationFileName = Guid.NewGuid().ToString() + Path.GetExtension(slideFile.FileName);
                    slideFile.SaveAs(Server.MapPath("/Resources/BusinessPlans/Slide/" + userCreateBusinessPlan.SlideShowPresentationFileName));
                }

                if (reportFile != null)
                {
                    userCreateBusinessPlan.DocumentsAndReportsFileName = Guid.NewGuid().ToString() + Path.GetExtension(reportFile.FileName);
                    reportFile.SaveAs(Server.MapPath("/Resources/BusinessPlans/Report/" + userCreateBusinessPlan.DocumentsAndReportsFileName));
                }

                if (ideaFile != null)
                {
                    userCreateBusinessPlan.IntroductionIdeaVideoFileName = Guid.NewGuid().ToString() + Path.GetExtension(ideaFile.FileName);
                    ideaFile.SaveAs(Server.MapPath("/Resources/BusinessPlans/Idea/" + userCreateBusinessPlan.IntroductionIdeaVideoFileName));
                }
                userCreateBusinessPlan.IsActive = false;
                userCreateBusinessPlan.MonetaryUnit_id = 2;
                userCreateBusinessPlan.CreateDate = DateTime.Now;
                //مپ کردن مدل طرح کاربری به طرح اصلی
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<UserCreateBusinessPlan, Tbl_BussinessPlans>();
                });
                IMapper iMapper = config.CreateMapper();
                var tblBussinessPlans = iMapper.Map<UserCreateBusinessPlan, Tbl_BussinessPlans>(userCreateBusinessPlan);

                db.Tbl_BussinessPlans.Add(tblBussinessPlans);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FinancialDuration_id = new SelectList(db.Tbl_BussinessPlan_FinancialDuration, "FinancialDurationID", "FinancialDurationTitle", userCreateBusinessPlan.FinancialDuration_id);
            ViewBag.CompanyType_id = new SelectList(db.Tbl_CompanyType, "CompanyTypeID", "CompanyTypeTitle", userCreateBusinessPlan.CompanyType_id);
            ViewBag.MonetaryUnit_id = new SelectList(db.Tbl_MonetaryUnits, "MonetaryUnitID", "MonetaryUnitTitle", userCreateBusinessPlan.MonetaryUnit_id);
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName", userCreateBusinessPlan.User_id);
            return View(userCreateBusinessPlan);
        }


        public ActionResult InvestedPlans()
        {
            int myUserId = UserSetAuthCookie.GetUserID(User.Identity.Name);
            //لیست طرح های من
            List<Tbl_BussinessPlans> qMyPlans = db.Tbl_BussinessPlans.Where(p => p.User_id == myUserId && p.IsDeleted == false).ToList();

            //لیست پلن های پرداختی
            List<Tbl_BusinessPlanPayment> qPaymentPlans = db.Tbl_BusinessPlanPayment.Where(p => p.IsConfirmedFromAdmin && p.IsPaid).ToList();

            List<UserBusinessPlanViewModel> userBusinessPlanViewModels = new List<UserBusinessPlanViewModel>();

            foreach (Tbl_BussinessPlans item in qMyPlans)
            {
                List<Tbl_BusinessPlanPayment> qThisPlanPayment = qPaymentPlans.Where(p => p.BusinessPlan_id == item.BussinessPlanID).ToList();
                foreach (var item2 in qThisPlanPayment)
                {
                    userBusinessPlanViewModels.Add(new UserBusinessPlanViewModel()
                    {
                        BussinessPlanID = item.BussinessPlanID,
                        Title = item.Title,
                        ImageNameInListPalns = item.ImageNameInListPalns,
                        IsActive = item.IsActive,
                        TotalInvestmentPrice = StringExtensions.En2Fa(planService.GetPlanSubmittedPaidByUserId(db, item.BussinessPlanID, myUserId).ToString("#,##0")) + " تومان",
                        IsConfirmedFromAdmin = item2.IsConfirmedFromAdmin,
                        PaidDateTime = item2.PaidDateTime.ToString(),
                        PaymentType = item2.Tbl_PaymentType.PaymentTitle,
                        TransactionPaymentCode = item2.TransactionPaymentCode,

                    });

                }
            }
            return View(userBusinessPlanViewModels.ToList());
        }

        // GET: Admin/Tbl_BusinessPlanPayment/BusinessPlanID
        /// <summary>
        /// دریافت لیست پرداختی های یک طرح
        /// </summary>
        /// <param name="id">شناسه طرح تجاری</param>
        /// <returns>لیست سرمایه گذری های یک طرح</returns>
        public ActionResult PlanPayments(int id)
        {
            ViewBag.Title = db.Tbl_BussinessPlans.FirstOrDefault(b => id == b.BussinessPlanID).Title;
            var tbl_BusinessPlanPayment = db.Tbl_BusinessPlanPayment.Include(t => t.Tbl_PaymentType).Include(t => t.Tbl_Users).Include(t => t.Tbl_Users1).Include(t => t.Tbl_BussinessPlans).Where(t => t.BusinessPlan_id == id);
            return View(tbl_BusinessPlanPayment.ToList());
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
