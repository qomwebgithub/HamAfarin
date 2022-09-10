using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using HamAfarin;
using ViewModels;
using PagedList;
using Common;

namespace Hamafarin.Controllers
{
    public class BusinessPlansController : Controller
    {
        HamAfarinDBEntities db = new HamAfarinDBEntities();
        PlanService planService = new PlanService();


        // GET: BusinessPlans
        public ActionResult Index(int page = 1)
        {

            ViewBag.id = page;

            //اجازه ی سرمایه گذاری
            ViewBag.ActivePlanCount = db.Tbl_BussinessPlans.Where(b => b.IsActive && b.IsPublish && b.IsDeleted == false).Count();
            //ViewBag.PastPlanCount = db.Tbl_BussinessPlans.Where(b => b.IsActive && b.IsDeleted == false && b.InvestmentStartDate > DateTime.Now).Count();
            ViewBag.FuturePlanCount = db.Tbl_FutureBusinessPlan.Where(f => f.IsDeleted == false && f.IsActive == true).Count();
            // برای دریافت حداکثر مبلغ سرمایه گذاری برای استفاده در فیلتر قیمت
            ViewBag.MaximumPrice = db.Tbl_BussinessPlans.Where(p => p.IsActive && p.IsDeleted == false && p.IsPublish && p.InvestmentStartDate <= DateTime.Now).OrderByDescending(p => p.MinimumAmountInvest).Select(p => p.MinimumAmountInvest).FirstOrDefault();
            // برای دریافت حداکثر مبلغ سرمایه گذاری برای استفاده در فیلتر قیمت
            ViewBag.MinimumPrice = db.Tbl_BussinessPlans.Where(p => p.IsActive && p.IsDeleted == false && p.IsPublish && p.InvestmentStartDate <= DateTime.Now).Min(p => p.MinimumAmountInvest).FirstOrDefault();

            return View();
        }

        public ActionResult SingleBusinessPlan(int id)
        {
            Tbl_BussinessPlans qActivePlans = db.Tbl_BussinessPlans.FirstOrDefault(b => b.BussinessPlanID == id && b.IsActive && b.IsPublish && b.IsDeleted == false);

            // تعداد روز های باقیمانده تا شروع
            int daysUntilStart = planService.CalculateRemainDay(qActivePlans.InvestmentStartDate);

            // تعداد روز های باقیمانده
            int qRemainingDay = planService.CalculateRemainDay(qActivePlans.InvestmentExpireDate);
            string qRemainingText = qRemainingDay + " روز";
            if (qRemainingDay == -1)
                qRemainingText = "پایان";

            // تبدیل اعداد به انگلیسی
            string AmountRequiredRoRaiseCapital = planService.GetEnglishNumber(qActivePlans.AmountRequiredRoRaiseCapital);
            // مبلغ سرمایه گذاری planService
            long intRaisedPrice = planService.GetRaisedPrice(db, qActivePlans.BussinessPlanID);
            // مقدار درصد سرمایه گذاری شده
            int qPercentageComplate = planService.GetPercentageInvestmentPlan(AmountRequiredRoRaiseCapital, intRaisedPrice);
            // تعداد سرمایه گذاران
            int qInvestorCount = planService.GetPlanInvestorCount(db, qActivePlans.BussinessPlanID);
            BusinessPlanSingleViewModel businessPlansItemViewModel = new BusinessPlanSingleViewModel()
            {
                BussinessPlanID = qActivePlans.BussinessPlanID,
                Title = qActivePlans.Title,
                CompanyName = qActivePlans.CompanyName,
                ImageName = qActivePlans.ImageNameInSinglePlan,
                MobileImageName = qActivePlans.ImageNameInListPalns,
                DaysUntilStart = daysUntilStart,
                RemainingTime = qRemainingDay,
                RemainingTimeText = qRemainingText,
                PercentageComplate = qPercentageComplate,
                PriceComplated = intRaisedPrice,
                WidthPercentage = qPercentageComplate + "%",
                InvestorCount = qInvestorCount,
                MarketTarget = qActivePlans.MarketTarget,
                BusinessPlanFeatures = qActivePlans.BusinessPlanFeatures,
                BusinessPlanRisks = qActivePlans.BusinessPlanRisks,
                FinancialDuration = qActivePlans.Tbl_BussinessPlan_FinancialDuration?.FinancialDurationTitle,
                Location = qActivePlans.Location,
                ImageNameWarranty = qActivePlans.ImageNameWarranty,
                PercentageReturnInvestment = qActivePlans.PercentageReturnInvestment.ToString(),
                AmountRequiredRoRaiseCapital = qActivePlans.AmountRequiredRoRaiseCapital,
                BussinessInstagramAddress = qActivePlans.BussinessInstagramAddress,
                BussinessAparatAddress = qActivePlans.BussinessAparatAddress,
                BussinessWebSiteAddress = qActivePlans.BussinessWebSiteAddress,
                BussinessLinkAddress = "",
                VideoFileName = qActivePlans.IntroductionIdeaVideoFileName,
                FinancialInformationText = qActivePlans.FinancialInformationText,
                InvestorsText = qActivePlans.InvestorsText,
                ProgressReportText = qActivePlans.ProgressReportText,
                IsOverflowInvestment = qActivePlans.IsOverflowInvestment,
                IsSuccessBussinessPlan = qActivePlans.IsSuccessBussinessPlan,
                MetaTitle = qActivePlans.MetaTitle,
                MetaDescription = qActivePlans.MetaDescription,
                Canonical = qActivePlans.Canonical,
                ImageAlt = qActivePlans.ImageAlt,
                SeoKey = qActivePlans.SeoKey,
            };

            //اجازه ی سرمایه گذاری
            businessPlansItemViewModel.IsAcceptInvestment = planService.IsAcceptInvestmentPlan(db, qActivePlans.BussinessPlanID);
            return View(businessPlansItemViewModel);
        }

        public ActionResult RiskAlertStatementInPlan()
        {
            Tbl_Settings qSettings = db.Tbl_Settings.FirstOrDefault();
            FooterViewModel footerViewModel = new FooterViewModel()
            {
                RiskAlertStatement = qSettings.RiskAlertStatement,
                RiskAlertStatementFullText = qSettings.RiskAlertStatementFullText,
                RiskAlertStatementAvtive = qSettings.RiskAlertStatementAvtive
            };
            return PartialView(footerViewModel);
        }
        public ActionResult ActivePlans(string searchText, long lowestPrice, long highestPrice, int page = 1)
        {
            if (searchText.HasValue() == false)
                searchText = "";

            if (highestPrice == 0)
            {
                long qhighestPrice = Convert.ToInt64(db.Tbl_BussinessPlans.Where(p => p.IsActive && p.IsPublish && p.IsDeleted == false && p.InvestmentStartDate <= DateTime.Now).OrderByDescending(p => p.MinimumAmountInvest).Select(p => p.MinimumAmountInvest).FirstOrDefault());
                highestPrice = qhighestPrice;
            }

            List<Tbl_BussinessPlans> qlstActivePlans = db.Tbl_BussinessPlans.AsEnumerable()
                .Where(b => b.IsActive &&
                    b.IsPublish &&
                    b.IsDeleted == false &&
                    (Convert.ToInt64(b.MinimumAmountInvest)) >= lowestPrice &&
                    (Convert.ToInt64(b.MinimumAmountInvest)) <= highestPrice &&
                    (b.Title.Contains(searchText) || b.BusinessPlanFeatures.Contains(searchText)))
                .OrderByDescending(b => b.InvestmentStartDate).ToList();


            List<BusinessPlansItemViewModel> lstPlans = new List<BusinessPlansItemViewModel>();
            foreach (var item in qlstActivePlans)
            {
                // تعداد روز های باقیمانده تا شروع
                int daysUntilStart = planService.CalculateRemainDay(item.InvestmentStartDate);

                // تعداد روز های باقیمانده تا پایان
                int daysUntilEnd = planService.CalculateRemainDay(item.InvestmentExpireDate);
                string daysUntilEndText = daysUntilEnd + " روز";
                if (daysUntilEnd == -1)
                    daysUntilEndText = "پایان";
                if (item.IsSuccessBussinessPlan)
                {
                    daysUntilEnd = -1;
                }
                string AmountRequiredRoRaiseCapital = planService.GetEnglishNumber(item.AmountRequiredRoRaiseCapital);
                int qPercentageComplate = planService.GetPercentage(long.Parse(AmountRequiredRoRaiseCapital), planService.GetRaisedPrice(db, item.BussinessPlanID));
                int qInvestorCount = planService.GetPlanInvestorCount(db, item.BussinessPlanID);

                lstPlans.Add(new BusinessPlansItemViewModel()
                {
                    BussinessPlanID = item.BussinessPlanID,
                    Title = item.Title,
                    ShortDescription = item.ShortDescription,
                    ImageName = item.ImageNameInListPalns,
                    DaysUntilStart = daysUntilStart,
                    RemainingTime = daysUntilEnd,
                    RemainingTimeText = daysUntilEndText,
                    PercentageComplate = qPercentageComplate,
                    widthPercentage = qPercentageComplate + "%",
                    InvestorCount = qInvestorCount,
                    AmountRequiredRoRaiseCapital = item.AmountRequiredRoRaiseCapital,
                    MarketTarget = item.MarketTarget,
                    CodeOTC = item.CodeOTC,
                    PercentageReturnInvestment = item.PercentageReturnInvestment.Value,
                    PlanInFarabourseUrl = item.PlanInFarabourseUrl
                });
            }
            IPagedList PagedList = lstPlans.ToPagedList(page, 6);
            ViewBag.Count = lstPlans.Count();
            return PartialView(PagedList);
        }

        public ActionResult SearchActivePlans(string searchText = "", int page = 1)
        {

            List<Tbl_BussinessPlans> qlstActivePlans = db.Tbl_BussinessPlans.Where(b => b.IsActive && b.IsPublish && b.IsDeleted == false && b.InvestmentStartDate <= DateTime.Now && b.Title.Contains(searchText) || b.BusinessPlanFeatures.Contains(searchText))
            .OrderByDescending(b => b.BussinessPlanID).ToList();


            List<BusinessPlansItemViewModel> lstPlans = new List<BusinessPlansItemViewModel>();
            foreach (var item in qlstActivePlans)
            {
                int qRemainingDay = planService.CalculateRemainDay(item.InvestmentExpireDate);
                string qRemainingText = qRemainingDay + " روز";
                if (qRemainingDay == -1)
                    qRemainingText = "پایان";
                if (item.IsSuccessBussinessPlan)
                {
                    qRemainingDay = -1;
                }
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
            IPagedList PagedList = lstPlans.ToPagedList(page, 6);
            //ViewBag.Count = lstPlans.Count();
            return PartialView("ActivePlans", lstPlans);
        }

        public ActionResult PastPlans(int page = 1)
        {
            //int takeNewActivePlans = 3;
            List<Tbl_BussinessPlans> qlstActivePlans = db.Tbl_BussinessPlans.Where(b => b.IsActive && b.IsPublish && b.IsDeleted == false && b.InvestmentStartDate > DateTime.Now)
                .OrderByDescending(b => b.BussinessPlanID).ToList();

            List<BusinessPlansItemViewModel> lstPlans = new List<BusinessPlansItemViewModel>();
            foreach (var item in qlstActivePlans)
            {
                int qRemainingDay = planService.CalculateRemainDay(item.InvestmentExpireDate);
                string qRemainingText = qRemainingDay + " روز";
                if (qRemainingDay == -1)
                    qRemainingText = "پایان";
                if (item.IsSuccessBussinessPlan)
                {
                    qRemainingDay = -1;
                }
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

        /// <summary>
        /// دریافت گالری طرح
        /// </summary>
        /// <param name="id">شناسه طرح</param>
        /// <returns>لیست گالری طرح</returns>
        [HttpGet]
        public ActionResult GalleryPlan(int id)
        {
            List<Tbl_BusinessPlanGallery> qGallery = db.Tbl_BusinessPlanGallery.Where(g => g.BusinessPlan_id == id).ToList();
            return PartialView(qGallery);
        }

        //public ActionResult Search(string searchText) 
        //{
        //    ViewBag.SearchResult = db.Tbl_BussinessPlans.Where(b => (b.Title));
        //    return View();
        //}

    }

}