using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HamAfarin.Controllers
{
    public class InvestmentProcessController : Controller
    {
        HamAfarinDBEntities db = new HamAfarinDBEntities();
        // GET: InvestmentProcess
        public ActionResult Index()
        {
            List<Tbl_InvestmentProcess> qlstInvestmentProcesses = db.Tbl_InvestmentProcess.Where(i => i.IsActive == true && i.IsDeleted == false).OrderBy(i => i.Sort).ToList();
            return PartialView(qlstInvestmentProcesses);
        }



        /// <summary>
        /// مزایای سرمایه گذاری
        /// </summary>
        /// <returns></returns>
        public ActionResult InvestmentBenefits()
        {


            return PartialView();
        }




        /// <summary>
        /// مزایای تأمین سرمایه در هم آفرین
        /// </summary>
        /// <returns></returns>
        public ActionResult BenefitsOfFinancingInHamAfarin()
        {
            return PartialView();
        }
    }
}