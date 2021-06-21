using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HamAfarin.Controllers
{
    public class FutureBusinessPlanController : Controller
    {
        HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: FutureBusinessPlan
        public ActionResult HomeFuturePlans()
        {
            return PartialView(db.Tbl_FutureBusinessPlan.Where(f => f.IsDeleted == false && f.IsActive == true).ToList());
        }

        public ActionResult FuturePlans()
        {
            return PartialView(db.Tbl_FutureBusinessPlan.Where(f => f.IsDeleted == false && f.IsActive == true).ToList());
        }


        [Route("FuturePlan/{id}")]
        public ActionResult SingleFuturePlan(int id)
        {
            return View(db.Tbl_FutureBusinessPlan.Find(id));
        }
    }
}