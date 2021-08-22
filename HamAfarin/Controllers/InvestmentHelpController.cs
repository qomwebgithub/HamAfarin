using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HamAfarin.Controllers
{
    public class InvestmentHelpController : Controller
    {
        HamAfarinDBEntities db = new HamAfarinDBEntities();
        // GET: InvestmentHelp
        public ActionResult ShortInvestmentHelp()
        {
            return PartialView(db.Tbl_InvestmentHelp.FirstOrDefault());
        }
        public ActionResult FullInvestmentHelp()
        {
            return View(db.Tbl_InvestmentHelp.FirstOrDefault());
        }

        /// <summary>
        /// صفحه ی سرمایه گذاری
        /// </summary>
        /// <returns></returns>
        public ActionResult InverstmentPage()
        {
            Tbl_Settings tbl_Settings = db.Tbl_Settings.FirstOrDefault();
            ViewBag.Image = tbl_Settings.InverstmentPageBanner;
            return View(db.Tbl_InvestmentHelp.FirstOrDefault());
        }



        /// <summary>
        /// صفحه متقاضی سرمایه
        /// </summary>
        /// <returns></returns>
        public ActionResult FinancingPage()
        {
            Tbl_CapitalApplicantHelp qCapial = db.Tbl_CapitalApplicantHelp.FirstOrDefault();
            Tbl_Settings tbl_Settings = db.Tbl_Settings.FirstOrDefault();
            ViewBag.Image = tbl_Settings.FinancingPageBanner;
            return View(qCapial);
        }

    }
}