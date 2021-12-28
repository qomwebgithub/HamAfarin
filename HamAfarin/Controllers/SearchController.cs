using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using ViewModels;

namespace HamAfarin.Controllers
{
    public class SearchController : Controller
    {
        HamAfarinDBEntities db = new HamAfarinDBEntities();
        PlanService planService = new PlanService();
        // GET: Search
        public ActionResult Index(String q)
        {
            List<Tbl_BussinessPlans> qListSearch = new List<Tbl_BussinessPlans>();
            qListSearch.AddRange(db.Tbl_BussinessPlans.Where(p => p.Title.Contains(q) && p.IsActive && p.IsDeleted == false).ToList());
            List<BusinessPlansItemViewModel> listSearch = new List<BusinessPlansItemViewModel>();
            foreach (var item in qListSearch)
            {
                int qRemainingDay = planService.CalculateRemainDay(item.InvestmentExpireDate);
                string qRemainingText = qRemainingDay + " روز";
                if (qRemainingDay == -1)
                    qRemainingText = "پایان";
                string AmountRequiredRoRaiseCapital = planService.GetEnglishNumber(item.AmountRequiredRoRaiseCapital);
                int qPercentageComplate = planService.GetPercentage(long.Parse(AmountRequiredRoRaiseCapital), planService.GetRaisedPrice(db, item.BussinessPlanID));
                int qInvestorCount = planService.GetPlanInvestorCount(db, item.BussinessPlanID);

                listSearch.Add(new BusinessPlansItemViewModel()
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
            return PartialView(listSearch);
        }
    }
}