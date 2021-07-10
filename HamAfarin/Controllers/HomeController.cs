using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using ViewModels;
using Hamafarin.Classes;
using HamAfarin;
using System.Net;
using System.IO;
using Common;
using System.Globalization;
using Newtonsoft.Json;

namespace Hamafarin.Controllers
{
    public class HomeController : Controller
    {
        SejamClass oSejamClass;
        HamAfarinDBEntities db = new HamAfarinDBEntities();
        PlanService planService = new PlanService();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Header()
        {
            Tbl_Settings qTbl_Settings = db.Tbl_Settings.FirstOrDefault();
            HeaderViewModel headerViewModel = new HeaderViewModel()
            {
                SiteLogo = qTbl_Settings.SiteLogo,
                Slogen = qTbl_Settings.Slogen,
                AparatUrl = qTbl_Settings.AparatUrl,
                InstagramUrl = qTbl_Settings.InstagramUrl,
                LinkedinUrl = qTbl_Settings.LinkedinUrl,
                Location = qTbl_Settings.Location,
                WhatsappUrl = qTbl_Settings.WhatsappUrl
            };
            return PartialView(headerViewModel);
        }

        public ActionResult Menu()
        {
            List<Tbl_Menu> qlstMenu = db.Tbl_Menu.Where(m => m.IsActive && m.IsDelete == false).ToList();
            if (User.Identity.IsAuthenticated)
            {
                UserService userService = new UserService();
                int roleId = UserSetAuthCookie.GetRoleID(User.Identity.Name);
                if (roleId == 1)
                    ViewBag.RoleType = "admin";
                else
                    ViewBag.RoleType = "user";
            }
            return PartialView(qlstMenu);
        }

        public ActionResult Sliders()
        {
            List<Tbl_Sliders> qlstSlider = db.Tbl_Sliders.Where(b => b.IsActive && b.IsDeleted == false && b.IsShowHomePage)
                .OrderByDescending(b => b.CreateDate).ToList();

            return PartialView(qlstSlider);
        }

        public ActionResult ActiveBusinessPlans()
        {
            int takeNewActivePlans = 3;
            List<Tbl_BussinessPlans> qlstActivePlans = db.Tbl_BussinessPlans.Where(b => b.IsActive && b.IsDeleted == false)
                .OrderByDescending(b => b.BussinessPlanID).Take(takeNewActivePlans).ToList();
            List<BusinessPlansItemViewModel> lstPlans = new List<BusinessPlansItemViewModel>();
            foreach (var item in qlstActivePlans)
            {
                int qRemainingDay = planService.calculateRemainDay(item);
                string qRemainingText = qRemainingDay + " روز";
                if (qRemainingDay == -1)
                    qRemainingText = "پایان";
                string AmountRequiredRoRaiseCapital = planService.GetEnglishNumber(item.AmountRequiredRoRaiseCapital);
                int qPercentageComplate = planService.GetPercentage(long.Parse(AmountRequiredRoRaiseCapital), planService.GetRaisedPrice(db, item.BussinessPlanID));
                int qInvestorCount = planService.GetPlanInvestorCount(db, item.BussinessPlanID);

                lstPlans.Add(new BusinessPlansItemViewModel()
                {
                    BussinessPlanID = item.BussinessPlanID,
                    Title = item.Title,
                    ShortDescription = item.ShortDescription,
                    ImageName = item.ImageNameInListPalns,
                    RemainingTime = qRemainingDay,
                    RemainingTimeText = qRemainingText,
                    PercentageComplate = qPercentageComplate,
                    widthPercentage = qPercentageComplate + "%",
                    InvestorCount = qInvestorCount,
                    AmountRequiredRoRaiseCapital = item.AmountRequiredRoRaiseCapital,
                    MarketTarget = item.MarketTarget,
                    CodeOTC = item.CodeOTC,
                    PercentageReturnInvestment = item.PercentageReturnInvestment.Value
                });
            }
            return PartialView(lstPlans);
        }

        public ActionResult IntroductionCompany()
        {
            Tbl_Settings tbl_Settings = db.Tbl_Settings.FirstOrDefault();
            IntroductionCompanyViewModel introductionCompany = new IntroductionCompanyViewModel()
            {
                IntroductionCompanyTitle = tbl_Settings.IntroductionCompanyTitle,
                IntroductionCompanyDescription = tbl_Settings.IntroductionCompanyDescription
            };

            ViewBag.ShareHolders = db.Tbl_ShareHoldersCompany.Where(s => s.IsDelete != true && s.IsActive).ToList();

            return PartialView(introductionCompany);
        }

        public ActionResult SingleIntroductionCompany()
        {
            Tbl_Settings tbl_Settings = db.Tbl_Settings.FirstOrDefault();
            IntroductionCompanyViewModel introductionCompany = new IntroductionCompanyViewModel()
            {
                IntroductionCompanyTitle = tbl_Settings.IntroductionCompanyTitle,
                IntroductionCompanyDescription = tbl_Settings.IntroductionCompanyDescription,
                IntroductionCompanyFullText = tbl_Settings.IntroductionCompanyFullText,
            };
            return View(introductionCompany);
        }

        /// <summary>
        /// نمایش اخبار در صفحه اصلی
        /// </summary>
        /// <returns></returns>
        public ActionResult BlogList()
        {
            List<BlogItemViewModel> lstBlog = new List<BlogItemViewModel>();
            List<Tbl_Blog> qlstBlog = db.Tbl_Blog.Where(b => b.IsActive && b.IsDeleted == false && b.ShowMainPage)
                .OrderByDescending(b => b.CreateDate).ToList();
            foreach (var item in qlstBlog)
            {
                BlogItemViewModel blog = new BlogItemViewModel()
                {
                    BlogID = item.BlogID,
                    Title = item.Title,
                    Description = item.Description,
                    BlogUrl = ConfigurationManager.AppSettings["ThisDomain"] + "/" + item.BlogID
                    + "/" + item.TitleUrl,
                    ImageUrl = ConfigurationManager.AppSettings["ThisDomain"] + "/Images/BlogImages/Image/" + item.ImageName,
                    ImageAlt = item.ImageAlt
                };
                lstBlog.Add(blog);
            }
            return PartialView(lstBlog);
        }

        public ActionResult Footer()
        {
            return PartialView(db.Tbl_Settings.FirstOrDefault());
        }

        [Route("AboutUs")]
        public ActionResult AboutUs()
        {
            string qAboutUs = db.Tbl_Settings.FirstOrDefault().AboutUs;
            FooterViewModel footerViewModel = new FooterViewModel()
            {
                AboutUs = qAboutUs
            };
            return View(footerViewModel);
        }

        [Route("Permits")]
        public ActionResult Permits()
        {
            string qPermits = db.Tbl_Settings.FirstOrDefault().Permits;
            FooterViewModel footerViewModel = new FooterViewModel()
            {
                Permits = qPermits
            };
            return View(footerViewModel);
        }

        [Route("ContactUs")]
        public ActionResult ContactUs()
        {
            string qContactUs = db.Tbl_Settings.FirstOrDefault().ContactUs;
            FooterViewModel footerViewModel = new FooterViewModel()
            {
                ContactUs = qContactUs
            };
            return View(footerViewModel);
        }

        [Route("InvestmentProcess")]
        public ActionResult InvestmentProcess()
        {
            string qInvestmentProcess = db.Tbl_Settings.FirstOrDefault().InvestmentProcess;
            FooterViewModel footerViewModel = new FooterViewModel()
            {
                InvestmentProcess = qInvestmentProcess
            };
            return View(footerViewModel);
        }

        [Route("CapitalApplicationProcess")]
        public ActionResult CapitalApplicationProcess()
        {
            string qCapitalApplicationProcess = db.Tbl_Settings.FirstOrDefault().CapitalApplicationProcess;
            FooterViewModel footerViewModel = new FooterViewModel()
            {
                CapitalApplicationProcess = qCapitalApplicationProcess
            };
            return View(footerViewModel);
        }

        [Route("CampaignProcess")]
        public ActionResult CampaignProcess()
        {
            string qCampaignProcess = db.Tbl_Settings.FirstOrDefault().CampaignProcess;
            FooterViewModel footerViewModel = new FooterViewModel()
            {
                CampaignProcess = qCampaignProcess
            };
            return View(footerViewModel);
        }

        [Route("SiteTermsConditions")]
        public ActionResult SiteTermsConditions()
        {
            string qSiteTermsConditions = db.Tbl_Settings.FirstOrDefault().SiteTermsConditions;
            FooterViewModel footerViewModel = new FooterViewModel()
            {
                SiteTermsConditions = qSiteTermsConditions
            };
            return View(footerViewModel);
        }

        [Route("Privacy")]
        public ActionResult Privacy()
        {
            string qPrivacy = db.Tbl_Settings.FirstOrDefault().Privacy;
            FooterViewModel footerViewModel = new FooterViewModel()
            {
                Privacy = qPrivacy
            };
            return View(footerViewModel);
        }

        [Route("RiskAlertStatement")]
        public ActionResult RiskAlertStatement()
        {
            Tbl_Settings qSettings = db.Tbl_Settings.FirstOrDefault();
            FooterViewModel footerViewModel = new FooterViewModel()
            {
                RiskAlertStatement = qSettings.RiskAlertStatement,
                RiskAlertStatementFullText = qSettings.RiskAlertStatementFullText
            };
            return View(footerViewModel);
        }
    }
}