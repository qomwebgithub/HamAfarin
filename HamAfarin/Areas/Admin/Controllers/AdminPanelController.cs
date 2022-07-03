using DataLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ViewModels.Api;

namespace HamAfarin.Areas.Admin.Controllers
{
    public class AdminPanelController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: Admin/AdminPanel
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _Menu()
        {
            return PartialView();
        }

        //public async Task<ActionResult> Test()
        //{
        //    var plan = await db.Tbl_BussinessPlans.FirstOrDefaultAsync(b => !b.IsDeleted);

        //    var fara = new FaraboorsClass();
        //    var json = await fara.GetProjectInfoAsync(plan.FaraboorsProjectId);

        //    try
        //    {
        //        var project = JsonConvert.DeserializeObject<ProjectInfoJsonModel>(json.Message);
        //        return Json(project, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {
        //        return Json(json.Message, JsonRequestBehavior.AllowGet);
        //    }
        //}
    }
}