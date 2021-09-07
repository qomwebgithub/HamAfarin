using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Common;
using DataLayer;
using HamAfarin;
using InsertShowImage;
using KooyWebApp_MVC.Classes;
using ViewModels;

namespace Hamafarin.Areas.Admin.Controllers
{
    public class Tbl_BussinessPlansController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();
        UserService userService = new UserService();
        SMS oSms = new SMS();

        // GET: Admin/Tbl_BussinessPlans
        public ActionResult Index()
        {
            var tbl_BussinessPlans = db.Tbl_BussinessPlans.Include(t => t.Tbl_BussinessPlan_BussenessFields).Include(t => t.Tbl_BussinessPlan_FinancialDuration).Include(t => t.Tbl_CompanyType).Include(t => t.Tbl_MonetaryUnits).Include(t => t.Tbl_Users);
            return View(tbl_BussinessPlans.OrderByDescending(c => c.CreateDate).ToList());
        }

        // GET: Admin/Tbl_BussinessPlans/Details/5
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

        // GET: Admin/Tbl_BussinessPlans/Create
        public ActionResult Create()
        {
            ViewBag.BussinessField_id = new SelectList(db.Tbl_BussinessPlan_BussenessFields, "BussinessFieldID", "BussinessFieldTitle");
            ViewBag.FinancialDuration_id = new SelectList(db.Tbl_BussinessPlan_FinancialDuration, "FinancialDurationID", "FinancialDurationTitle");
            ViewBag.BussinessPlanStatus_id = new SelectList(db.Tbl_BussinessPlan_Status, "BussinessPlanStatusID", "StatusTitle");
            ViewBag.CompanyType_id = new SelectList(db.Tbl_CompanyType, "CompanyTypeID", "CompanyTypeTitle");
            ViewBag.MonetaryUnit_id = new SelectList(db.Tbl_MonetaryUnits, "MonetaryUnitID", "MonetaryUnitTitle");
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName");
            return View(new AdminCreateEditBusinessPlan()
            {
                IsActive = true,
                MonetaryUnit_id = 2
            });
        }

        // POST: Admin/Tbl_BussinessPlans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AdminCreateEditBusinessPlan adminCreateEditBusinessPlan, HttpPostedFileBase imgInListPalns,
            HttpPostedFileBase imgInSinglePlan, HttpPostedFileBase[] GalleryPlan, HttpPostedFileBase imgLogo,
            HttpPostedFileBase imgWarranty, HttpPostedFileBase imgNationalCard, HttpPostedFileBase letterFile,
            HttpPostedFileBase modelFile, HttpPostedFileBase slideFile, HttpPostedFileBase reportFile,
            HttpPostedFileBase ideaFile, HttpPostedFileBase contractFile)
        {
            ////////////****************/////////////////////////////
            // تبدیل تاریخ تولد از 
            // string
            // به
            // datetime
            adminCreateEditBusinessPlan.InvestmentStartDate = StringExtensions.StringToDate(adminCreateEditBusinessPlan.strInvestmentStartDate);
            adminCreateEditBusinessPlan.InvestmentExpireDate = StringExtensions.StringToDate(adminCreateEditBusinessPlan.strInvestmentExpireDate);
            adminCreateEditBusinessPlan.CompanyRegisterDate = StringExtensions.StringToDate(adminCreateEditBusinessPlan.strCompanyRegisterDate);
            adminCreateEditBusinessPlan.PreviousInvestorDate = StringExtensions.StringToDate(adminCreateEditBusinessPlan.strPreviousInvestorDate);
            adminCreateEditBusinessPlan.PreviousInvestorExpireDate = StringExtensions.StringToDate(adminCreateEditBusinessPlan.strPreviousInvestorExpireDate);
            ////////////****************/////////////////////////////

            adminCreateEditBusinessPlan.ImageNameInListPalns = "no-photo.jpg";
            if (imgInListPalns != null && imgInListPalns.IsImage())
            {
                adminCreateEditBusinessPlan.ImageNameInListPalns = Guid.NewGuid().ToString() + Path.GetExtension(imgInListPalns.FileName);
                imgInListPalns.SaveAs(Server.MapPath("/Images/BusinessPlans/Image/" + adminCreateEditBusinessPlan.ImageNameInListPalns));

                ImageResizer img = new ImageResizer(500);
                img.Resize(Server.MapPath("/Images/BusinessPlans/Image/" + adminCreateEditBusinessPlan.ImageNameInListPalns),
                    Server.MapPath("/Images/BusinessPlans/Thumb/" + adminCreateEditBusinessPlan.ImageNameInListPalns));

            }

            adminCreateEditBusinessPlan.ImageNameInSinglePlan = "no-photo.jpg";
            if (imgInSinglePlan != null && imgInSinglePlan.IsImage())
            {
                adminCreateEditBusinessPlan.ImageNameInSinglePlan = Guid.NewGuid().ToString() + Path.GetExtension(imgInSinglePlan.FileName);
                imgInSinglePlan.SaveAs(Server.MapPath("/Images/BusinessPlans/Image/" + adminCreateEditBusinessPlan.ImageNameInSinglePlan));

            }

            if (imgLogo != null && imgLogo.IsImage())
            {
                adminCreateEditBusinessPlan.BussinessLogoImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgLogo.FileName);
                imgLogo.SaveAs(Server.MapPath("/Images/BusinessPlans/Logo/Image/" + adminCreateEditBusinessPlan.BussinessLogoImageName));

                ImageResizer img = new ImageResizer(500);
                img.Resize(Server.MapPath("/Images/BusinessPlans/Logo/Image/" + adminCreateEditBusinessPlan.BussinessLogoImageName),
                    Server.MapPath("/Images/BusinessPlans/Logo/Thumb/" + adminCreateEditBusinessPlan.BussinessLogoImageName));

            }
            // ذخیره عکس ضمانت نامه
            if (imgWarranty != null && imgWarranty.IsImage())
            {
                adminCreateEditBusinessPlan.ImageNameWarranty = Guid.NewGuid().ToString() + Path.GetExtension(imgWarranty.FileName);
                imgWarranty.SaveAs(Server.MapPath("/Resources/BusinessPlans/Warranty/" + adminCreateEditBusinessPlan.ImageNameWarranty));
            }

            if (imgNationalCard != null && imgNationalCard.IsImage())
            {
                adminCreateEditBusinessPlan.CompanyAgentNationalCardImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgNationalCard.FileName);
                imgNationalCard.SaveAs(Server.MapPath("/Resources/BusinessPlans/NationalCard/" + adminCreateEditBusinessPlan.CompanyAgentNationalCardImageName));
            }

            if (letterFile != null)
            {
                adminCreateEditBusinessPlan.CompanyIntroductionLetterFileName = Guid.NewGuid().ToString() + Path.GetExtension(letterFile.FileName);
                letterFile.SaveAs(Server.MapPath("/Resources/BusinessPlans/Letter/" + adminCreateEditBusinessPlan.CompanyIntroductionLetterFileName));
            }

            if (modelFile != null)
            {
                adminCreateEditBusinessPlan.BussinessModelFileName = Guid.NewGuid().ToString() + Path.GetExtension(modelFile.FileName);
                modelFile.SaveAs(Server.MapPath("/Resources/BusinessPlans/Model/" + adminCreateEditBusinessPlan.BussinessModelFileName));
            }

            if (slideFile != null)
            {
                adminCreateEditBusinessPlan.SlideShowPresentationFileName = Guid.NewGuid().ToString() + Path.GetExtension(slideFile.FileName);
                slideFile.SaveAs(Server.MapPath("/Resources/BusinessPlans/Slide/" + adminCreateEditBusinessPlan.SlideShowPresentationFileName));
            }

            if (reportFile != null)
            {
                adminCreateEditBusinessPlan.DocumentsAndReportsFileName = Guid.NewGuid().ToString() + Path.GetExtension(reportFile.FileName);
                reportFile.SaveAs(Server.MapPath("/Resources/BusinessPlans/Report/" + adminCreateEditBusinessPlan.DocumentsAndReportsFileName));
            }

            if (ideaFile != null)
            {
                adminCreateEditBusinessPlan.IntroductionIdeaVideoFileName = Guid.NewGuid().ToString() + Path.GetExtension(ideaFile.FileName);
                ideaFile.SaveAs(Server.MapPath("/Resources/BusinessPlans/Idea/" + adminCreateEditBusinessPlan.IntroductionIdeaVideoFileName));
            }

            if (contractFile != null)
            {
                adminCreateEditBusinessPlan.ContractFileName = Guid.NewGuid().ToString() + Path.GetExtension(contractFile.FileName);
                contractFile.SaveAs(Server.MapPath("/Resources/BusinessPlans/Contract/" + adminCreateEditBusinessPlan.IntroductionIdeaVideoFileName));
            }

            adminCreateEditBusinessPlan.CreateDate = DateTime.Now;
            adminCreateEditBusinessPlan.User_id = UserSetAuthCookie.GetUserID(User.Identity.Name);

            //مپ کردن مدل طرح کاربری به طرح اصلی
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AdminCreateEditBusinessPlan, Tbl_BussinessPlans>();
            });
            IMapper iMapper = config.CreateMapper();
            var tblBussinessPlans = iMapper.Map<AdminCreateEditBusinessPlan, Tbl_BussinessPlans>(adminCreateEditBusinessPlan);
            if (tblBussinessPlans.TitleUrl == null)
            {
                tblBussinessPlans.TitleUrl = tblBussinessPlans.Title.Trim().Replace(" ", "-");
            }
            else
            {
                tblBussinessPlans.TitleUrl = tblBussinessPlans.TitleUrl.Trim().Replace(" ", "-");
            }
            tblBussinessPlans.IsSuccessBussinessPlan = false;
            db.Tbl_BussinessPlans.Add(tblBussinessPlans);
            db.SaveChanges();
            // کنترل گالری عکس
            if (GalleryPlan != null && GalleryPlan.Any())
            {
                foreach (HttpPostedFileBase item in GalleryPlan)
                {
                    string imageName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                    item.SaveAs(Server.MapPath("/Images/BusinessPlans/Image/" + imageName));

                    ImageResizer img = new ImageResizer(500);
                    img.Resize(Server.MapPath("/Images/BusinessPlans/Image/" + imageName),
                        Server.MapPath("/Images/BusinessPlans/Thumb/" + imageName));
                    db.Tbl_BusinessPlanGallery.Add(new Tbl_BusinessPlanGallery()
                    {
                        BusinessPlan_id = tblBussinessPlans.BussinessPlanID,
                        ImageName = imageName,
                        IsActive = true,
                        IsDelete = false
                    });

                }
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Admin/Tbl_BussinessPlans/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.BussinessField_id = new SelectList(db.Tbl_BussinessPlan_BussenessFields, "BussinessFieldID", "BussinessFieldTitle", tbl_BussinessPlans.BussinessField_id);
            ViewBag.FinancialDuration_id = new SelectList(db.Tbl_BussinessPlan_FinancialDuration, "FinancialDurationID", "FinancialDurationTitle", tbl_BussinessPlans.FinancialDuration_id);
            ViewBag.CompanyType_id = new SelectList(db.Tbl_CompanyType, "CompanyTypeID", "CompanyTypeTitle", tbl_BussinessPlans.CompanyType_id);
            ViewBag.MonetaryUnit_id = new SelectList(db.Tbl_MonetaryUnits, "MonetaryUnitID", "MonetaryUnitTitle", tbl_BussinessPlans.MonetaryUnit_id);
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_BussinessPlans.User_id);
            //مپ کردن مدل طرح کاربری به طرح اصلی
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Tbl_BussinessPlans, AdminCreateEditBusinessPlan>();
            });
            IMapper iMapper = config.CreateMapper();
            var adminCreateEditBusinessPlan = iMapper.Map<Tbl_BussinessPlans, AdminCreateEditBusinessPlan>(tbl_BussinessPlans);
            List<Tbl_BusinessPlanGallery> qGallery = db.Tbl_BusinessPlanGallery.Where(g => g.BusinessPlan_id == adminCreateEditBusinessPlan.BussinessPlanID).ToList();
            adminCreateEditBusinessPlan.GalleryPlan = qGallery;
            return View(adminCreateEditBusinessPlan);
        }

        // POST: Admin/Tbl_BussinessPlans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AdminCreateEditBusinessPlan adminCreateEditBusinessPlan, HttpPostedFileBase imgInListPalns,
            HttpPostedFileBase imgInSinglePlan, HttpPostedFileBase[] GalleryPlan, HttpPostedFileBase imgLogo,
            HttpPostedFileBase imgWarranty, HttpPostedFileBase imgNationalCard, HttpPostedFileBase letterFile,
            HttpPostedFileBase modelFile, HttpPostedFileBase slideFile, HttpPostedFileBase reportFile,
            HttpPostedFileBase ideaFile, HttpPostedFileBase contractFile)
        {
            ////////////****************/////////////////////////////
            // تبدیل تاریخ تولد از 
            // string
            // به
            // datetime
            if (string.IsNullOrEmpty(adminCreateEditBusinessPlan.strInvestmentStartDate) == false)
            {
                adminCreateEditBusinessPlan.InvestmentStartDate = StringExtensions.StringToDate(adminCreateEditBusinessPlan.strInvestmentStartDate);
            }
            if (string.IsNullOrEmpty(adminCreateEditBusinessPlan.strInvestmentExpireDate) == false)
            {
                adminCreateEditBusinessPlan.InvestmentExpireDate = StringExtensions.StringToDate(adminCreateEditBusinessPlan.strInvestmentExpireDate);
            }
            if (string.IsNullOrEmpty(adminCreateEditBusinessPlan.strCompanyRegisterDate) == false)
            {
                adminCreateEditBusinessPlan.CompanyRegisterDate = StringExtensions.StringToDate(adminCreateEditBusinessPlan.strCompanyRegisterDate);
            }
            if (string.IsNullOrEmpty(adminCreateEditBusinessPlan.strPreviousInvestorDate) == false)
            {
                adminCreateEditBusinessPlan.PreviousInvestorDate = StringExtensions.StringToDate(adminCreateEditBusinessPlan.strPreviousInvestorDate);
            }
            if (string.IsNullOrEmpty(adminCreateEditBusinessPlan.strPreviousInvestorExpireDate) == false)
            {
                adminCreateEditBusinessPlan.PreviousInvestorExpireDate = StringExtensions.StringToDate(adminCreateEditBusinessPlan.strPreviousInvestorExpireDate);
            }

            if (imgWarranty != null && imgWarranty.IsImage())
            {
                if (adminCreateEditBusinessPlan.ImageNameWarranty != null && adminCreateEditBusinessPlan.ImageNameWarranty != "no-photo.jpg")
                {
                    System.IO.File.Delete(Server.MapPath("/Resources/BusinessPlans/Warranty/" + adminCreateEditBusinessPlan.ImageNameWarranty));
                }
                adminCreateEditBusinessPlan.ImageNameWarranty = Guid.NewGuid().ToString() + Path.GetExtension(imgWarranty.FileName);
                imgWarranty.SaveAs(Server.MapPath("/Resources/BusinessPlans/Warranty/" + adminCreateEditBusinessPlan.ImageNameWarranty));
            }

            if (imgInSinglePlan != null && imgInSinglePlan.IsImage())
            {
                if (adminCreateEditBusinessPlan.ImageNameInSinglePlan != "no-photo.jpg")
                {
                    System.IO.File.Delete(Server.MapPath("/Images/BusinessPlans/Image/" + adminCreateEditBusinessPlan.ImageNameInSinglePlan));
                    System.IO.File.Delete(Server.MapPath("/Images/BusinessPlans/Thumb/" + adminCreateEditBusinessPlan.ImageNameInSinglePlan));
                }
                adminCreateEditBusinessPlan.ImageNameInSinglePlan = Guid.NewGuid().ToString() + Path.GetExtension(imgInSinglePlan.FileName);
                imgInSinglePlan.SaveAs(Server.MapPath("/Images/BusinessPlans/Image/" + adminCreateEditBusinessPlan.ImageNameInSinglePlan));

                ImageResizer img = new ImageResizer(500);
                img.Resize(Server.MapPath("/Images/BusinessPlans/Image/" + adminCreateEditBusinessPlan.ImageNameInSinglePlan),
                    Server.MapPath("/Images/BusinessPlans/Thumb/" + adminCreateEditBusinessPlan.ImageNameInSinglePlan));

            }

            if (imgInListPalns != null && imgInListPalns.IsImage())
            {
                if (adminCreateEditBusinessPlan.ImageNameInListPalns != "no-photo.jpg")
                {
                    System.IO.File.Delete(Server.MapPath("/Images/BusinessPlans/Image/" + adminCreateEditBusinessPlan.ImageNameInListPalns));
                    System.IO.File.Delete(Server.MapPath("/Images/BusinessPlans/Thumb/" + adminCreateEditBusinessPlan.ImageNameInListPalns));
                }
                adminCreateEditBusinessPlan.ImageNameInListPalns = Guid.NewGuid().ToString() + Path.GetExtension(imgInListPalns.FileName);
                imgInListPalns.SaveAs(Server.MapPath("/Images/BusinessPlans/Image/" + adminCreateEditBusinessPlan.ImageNameInListPalns));

                ImageResizer img = new ImageResizer(500);
                img.Resize(Server.MapPath("/Images/BusinessPlans/Image/" + adminCreateEditBusinessPlan.ImageNameInListPalns),
                    Server.MapPath("/Images/BusinessPlans/Thumb/" + adminCreateEditBusinessPlan.ImageNameInListPalns));

            }
            if (imgLogo != null && imgLogo.IsImage())
            {
                if (adminCreateEditBusinessPlan.BussinessLogoImageName != "no-photo.jpg")
                {
                    System.IO.File.Delete(Server.MapPath("/Images/BusinessPlans/Logo/Image/" + adminCreateEditBusinessPlan.BussinessLogoImageName));
                    System.IO.File.Delete(Server.MapPath("/Images/BusinessPlans/Logo/Thumb/" + adminCreateEditBusinessPlan.BussinessLogoImageName));
                }
                adminCreateEditBusinessPlan.BussinessLogoImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgLogo.FileName);
                imgLogo.SaveAs(Server.MapPath("/Images/BusinessPlans/Logo/Image/" + adminCreateEditBusinessPlan.BussinessLogoImageName));

                ImageResizer img = new ImageResizer(500);
                img.Resize(Server.MapPath("/Images/BusinessPlans/Logo/Image/" + adminCreateEditBusinessPlan.BussinessLogoImageName),
                    Server.MapPath("/Images/BusinessPlans/Logo/Thumb/" + adminCreateEditBusinessPlan.BussinessLogoImageName));

            }

            if (letterFile != null)
            {
                if (adminCreateEditBusinessPlan.CompanyIntroductionLetterFileName != null)
                {
                    System.IO.File.Delete(Server.MapPath("/Resources/BusinessPlans/Letter/" + adminCreateEditBusinessPlan.CompanyIntroductionLetterFileName));
                }
                adminCreateEditBusinessPlan.CompanyIntroductionLetterFileName = Guid.NewGuid().ToString() + Path.GetExtension(letterFile.FileName);
                letterFile.SaveAs(Server.MapPath("/Resources/BusinessPlans/Letter/" + adminCreateEditBusinessPlan.CompanyIntroductionLetterFileName));
            }

            if (modelFile != null)
            {
                if (adminCreateEditBusinessPlan.CompanyIntroductionLetterFileName != null)
                {
                    System.IO.File.Delete(Server.MapPath("/Resources/BusinessPlans/Model/" + adminCreateEditBusinessPlan.BussinessModelFileName));
                }
                adminCreateEditBusinessPlan.BussinessModelFileName = Guid.NewGuid().ToString() + Path.GetExtension(modelFile.FileName);
                modelFile.SaveAs(Server.MapPath("/Resources/BusinessPlans/Model/" + adminCreateEditBusinessPlan.BussinessModelFileName));
            }

            if (slideFile != null)
            {
                if (adminCreateEditBusinessPlan.CompanyIntroductionLetterFileName != null)
                {
                    System.IO.File.Delete(Server.MapPath("/Resources/BusinessPlans/Slide/" + adminCreateEditBusinessPlan.SlideShowPresentationFileName));
                }
                adminCreateEditBusinessPlan.SlideShowPresentationFileName = Guid.NewGuid().ToString() + Path.GetExtension(slideFile.FileName);
                slideFile.SaveAs(Server.MapPath("/Resources/BusinessPlans/Slide/" + adminCreateEditBusinessPlan.SlideShowPresentationFileName));
            }

            if (reportFile != null)
            {
                if (adminCreateEditBusinessPlan.CompanyIntroductionLetterFileName != null)
                {
                    System.IO.File.Delete(Server.MapPath("/Resources/BusinessPlans/Report/" + adminCreateEditBusinessPlan.DocumentsAndReportsFileName));
                }
                adminCreateEditBusinessPlan.DocumentsAndReportsFileName = Guid.NewGuid().ToString() + Path.GetExtension(reportFile.FileName);
                reportFile.SaveAs(Server.MapPath("/Resources/BusinessPlans/Report/" + adminCreateEditBusinessPlan.DocumentsAndReportsFileName));
            }

            if (ideaFile != null)
            {
                if (adminCreateEditBusinessPlan.CompanyIntroductionLetterFileName != null)
                {
                    System.IO.File.Delete(Server.MapPath("/Resources/BusinessPlans/Idea/" + adminCreateEditBusinessPlan.IntroductionIdeaVideoFileName));
                }
                adminCreateEditBusinessPlan.IntroductionIdeaVideoFileName = Guid.NewGuid().ToString() + Path.GetExtension(ideaFile.FileName);
                ideaFile.SaveAs(Server.MapPath("/Resources/BusinessPlans/Idea/" + adminCreateEditBusinessPlan.IntroductionIdeaVideoFileName));
            }

            if (contractFile != null)
            {
                if (adminCreateEditBusinessPlan.ContractFileName != null)
                {
                    System.IO.File.Delete(Server.MapPath("/Resources/BusinessPlans/Contract/" + adminCreateEditBusinessPlan.ContractFileName));
                }
                adminCreateEditBusinessPlan.ContractFileName = Guid.NewGuid().ToString() + Path.GetExtension(contractFile.FileName);
                contractFile.SaveAs(Server.MapPath("/Resources/BusinessPlans/Contract/" + adminCreateEditBusinessPlan.ContractFileName));
            }

            //مپ کردن مدل طرح کاربری به طرح اصلی
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AdminCreateEditBusinessPlan, Tbl_BussinessPlans>();
            });
            IMapper iMapper = config.CreateMapper();
            var tbl_BussinessPlans = iMapper.Map<AdminCreateEditBusinessPlan, Tbl_BussinessPlans>(adminCreateEditBusinessPlan);
            if (tbl_BussinessPlans.TitleUrl == null)
            {
                tbl_BussinessPlans.TitleUrl = tbl_BussinessPlans.Title.Trim().Replace(" ", "-");
            }
            db.Entry(tbl_BussinessPlans).State = EntityState.Modified;
            db.SaveChanges();
            // کنترل گالری عکس
            if (GalleryPlan != null && GalleryPlan.Any())
            {
                foreach (HttpPostedFileBase item in GalleryPlan)
                {
                    string imageName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                    item.SaveAs(Server.MapPath("/Images/BusinessPlans/Image/" + imageName));

                    ImageResizer img = new ImageResizer(500);
                    img.Resize(Server.MapPath("/Images/BusinessPlans/Image/" + imageName),
                        Server.MapPath("/Images/BusinessPlans/Thumb/" + imageName));
                    db.Tbl_BusinessPlanGallery.Add(new Tbl_BusinessPlanGallery()
                    {
                        BusinessPlan_id = tbl_BussinessPlans.BussinessPlanID,
                        ImageName = imageName,
                        IsActive = true,
                        IsDelete = false
                    });

                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BussinessField_id = new SelectList(db.Tbl_BussinessPlan_BussenessFields, "BussinessFieldID", "BussinessFieldTitle", adminCreateEditBusinessPlan.BussinessField_id);
            ViewBag.FinancialDuration_id = new SelectList(db.Tbl_BussinessPlan_FinancialDuration, "FinancialDurationID", "FinancialDurationTitle", adminCreateEditBusinessPlan.FinancialDuration_id);
            ViewBag.CompanyType_id = new SelectList(db.Tbl_CompanyType, "CompanyTypeID", "CompanyTypeTitle", adminCreateEditBusinessPlan.CompanyType_id);
            ViewBag.MonetaryUnit_id = new SelectList(db.Tbl_MonetaryUnits, "MonetaryUnitID", "MonetaryUnitTitle", adminCreateEditBusinessPlan.MonetaryUnit_id);
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName", adminCreateEditBusinessPlan.User_id);
            return View(adminCreateEditBusinessPlan);
        }

        public void DoSuccessPlan(int id)
        {
            if (UserSetAuthCookie.GetRoleID(User.Identity.Name) == 1)
            {
                Tbl_BussinessPlans qPlan = db.Tbl_BussinessPlans.Find(id);
                qPlan.IsSuccessBussinessPlan = true;
                db.SaveChanges();
            }
        }

        // GET: Admin/Tbl_BussinessPlans/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Admin/Tbl_BussinessPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_BussinessPlans tbl_BussinessPlans = db.Tbl_BussinessPlans.Find(id);
            tbl_BussinessPlans.IsDeleted = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> SendSMSCertificateReady(int id)
        {
            List<string> lstUserID = await db.Tbl_BusinessPlanPayment
                .Include(p => p.Tbl_Users)
                .Where(p => p.BusinessPlan_id == id && p.IsConfirmedFromFaraboors)
                .Select(p => p.Tbl_Users.MobileNumber)
                .Distinct()
                .ToListAsync();
            var mobileNumbers = String.Join(",",lstUserID);
            Tbl_Sms qSms = await db.Tbl_Sms.FindAsync(6);
            string message = qSms.Message;
            if (message.Contains("@T"))
            {
                Tbl_BussinessPlans qBussinessPlan = await db.Tbl_BussinessPlans.FirstOrDefaultAsync(b => b.BussinessPlanID == id);
                message = message.Replace("@T", qBussinessPlan.Title);
            }
            (bool Success, string Message) smsResult = await oSms.AdpSendSMSAsync(mobileNumbers, message);

            return Json(new { success = smsResult.Success, message = smsResult.Message });
        }

        /// <summary>
        /// حذف تصویر از گالری
        /// </summary>
        /// <param name="id">شناسه تصویر در گالری</param>
        public void DeleteImageGallery(int id)
        {
            var qGallery = db.Tbl_BusinessPlanGallery.Find(id);
            System.IO.File.Delete(Server.MapPath("/Images/BusinessPlans/Image/" + qGallery.ImageName));
            System.IO.File.Delete(Server.MapPath("/Images/BusinessPlans/Thumb/" + qGallery.ImageName));
            db.Tbl_BusinessPlanGallery.Remove(qGallery);
            db.SaveChanges();
        }

        /// <summary>
        /// حذف ویدیو
        /// </summary>
        /// <param name="id">شناسه ویدیو</param>
        public void DeleteVideo(int id)
        {
            var qBussinessPlans = db.Tbl_BussinessPlans.Find(id);
            System.IO.File.Delete(Server.MapPath("/Resources/BusinessPlans/Idea/" + qBussinessPlans.IntroductionIdeaVideoFileName));
            qBussinessPlans.IntroductionIdeaVideoFileName = null;
            db.Entry(qBussinessPlans).State = EntityState.Modified;
            db.SaveChanges();
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
