using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using DataLayer;
using InsertShowImage;
using KooyWebApp_MVC.Classes;
using ViewModels;

namespace Hamafarin.Areas.Admin.Controllers
{
    public class Tbl_SettingsController : Controller
    {
        HamAfarinDBEntities db = new HamAfarinDBEntities();
        // GET: Admin/Tbl_Settings
        public ActionResult Index()
        {
            return View(db.Tbl_Settings.FirstOrDefault());
        }

        // GET: Admin/Tbl_Settings
        public ActionResult Edit()
        {
            return View(db.Tbl_Settings.FirstOrDefault());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tbl_Settings tbl_Settings, HttpPostedFileBase imgUp)
        {
            if (ModelState.IsValid)
            {

                if (imgUp != null && imgUp.IsImage())
                {
                    if (tbl_Settings.SiteLogo != "no-photo.jpg" && tbl_Settings.SiteLogo != null)
                    {
                        System.IO.File.Delete(Server.MapPath("/Images/SettingImages/Image/" + tbl_Settings.SiteLogo));
                        System.IO.File.Delete(Server.MapPath("/Images/SettingImages/Thumb/" + tbl_Settings.SiteLogo));
                    }
                    tbl_Settings.SiteLogo = Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                    imgUp.SaveAs(Server.MapPath("/Images/SettingImages/Image/" + tbl_Settings.SiteLogo));

                    ImageResizer img = new ImageResizer(500);
                    img.Resize(Server.MapPath("/Images/SettingImages/Image/" + tbl_Settings.SiteLogo),
                        Server.MapPath("/Images/SettingImages/Thumb/" + tbl_Settings.SiteLogo));

                }

                db.Entry(tbl_Settings).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Settings);
        }

        public ActionResult EditFooter()
        {
            Tbl_Settings tbl_Settings = db.Tbl_Settings.FirstOrDefault();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Tbl_Settings, FooterViewModel>();
            });
            IMapper iMapper = config.CreateMapper();
            var footerViewModel = iMapper.Map<Tbl_Settings, FooterViewModel>(tbl_Settings);

            return View(footerViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFooter(FooterViewModel footerViewModel)
        {
            if (ModelState.IsValid)
            {
                //var config = new MapperConfiguration(cfg =>
                //{
                //    cfg.CreateMap<FooterViewModel, Tbl_Settings>();
                //});
                //IMapper iMapper = config.CreateMapper();
                //var tbl_Settings = iMapper.Map<FooterViewModel, Tbl_Settings>(footerViewModel);
                Tbl_Settings tbl_Settings = db.Tbl_Settings.FirstOrDefault();
                tbl_Settings.AboutUs = footerViewModel.AboutUs;
                tbl_Settings.ContactUs = footerViewModel.ContactUs;
                tbl_Settings.Permits = footerViewModel.Permits;
                tbl_Settings.InvestmentProcess = footerViewModel.InvestmentProcess;
                tbl_Settings.CapitalApplicationProcess = footerViewModel.CapitalApplicationProcess;
                tbl_Settings.CampaignProcess = footerViewModel.CampaignProcess;
                tbl_Settings.SiteTermsConditions = footerViewModel.SiteTermsConditions;
                tbl_Settings.Privacy = footerViewModel.Privacy;
                tbl_Settings.RiskAlertStatement = footerViewModel.RiskAlertStatement;
                tbl_Settings.RiskAlertStatementFullText = footerViewModel.RiskAlertStatementFullText;
                db.SaveChanges();
                ViewBag.success = true;
                
            }
            return View(footerViewModel);
        }

        public ActionResult IntroductionCompany()
        {
            Tbl_Settings tbl_Settings = db.Tbl_Settings.FirstOrDefault();
            IntroductionCompanyViewModel companyViewModel = new IntroductionCompanyViewModel()
            {
                IntroductionCompanyTitle = tbl_Settings.IntroductionCompanyTitle,
                IntroductionCompanyDescription = tbl_Settings.IntroductionCompanyDescription,
                IntroductionCompanyFullText = tbl_Settings.IntroductionCompanyFullText,
            };
            return View(companyViewModel);
        }

        public ActionResult EditIntroductionCompany()
        {
            Tbl_Settings tbl_Settings = db.Tbl_Settings.FirstOrDefault();
            IntroductionCompanyViewModel companyViewModel = new IntroductionCompanyViewModel()
            {
                IntroductionCompanyTitle = tbl_Settings.IntroductionCompanyTitle,
                IntroductionCompanyDescription = tbl_Settings.IntroductionCompanyDescription,
                IntroductionCompanyFullText = tbl_Settings.IntroductionCompanyFullText,
            };
            return View(companyViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditIntroductionCompany(IntroductionCompanyViewModel companyViewModel)
        {
            Tbl_Settings tbl_Settings = db.Tbl_Settings.FirstOrDefault();

            tbl_Settings.IntroductionCompanyTitle = companyViewModel.IntroductionCompanyTitle;
            tbl_Settings.IntroductionCompanyDescription = companyViewModel.IntroductionCompanyDescription;
            tbl_Settings.IntroductionCompanyFullText = companyViewModel.IntroductionCompanyFullText;

            db.SaveChanges();

            return View("IntroductionCompany", companyViewModel);
        }

    }
}