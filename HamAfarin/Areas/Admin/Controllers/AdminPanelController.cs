using DataLayer;
using HamAfarin.Classes;
using HamAfarin.Classes.Dto;
using HamAfarin.Classes.Interface;
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

        //public ActionResult TestSaman()
        //{
        //    var bankService = new SamanBankService();

        //    var bankDto = new BankDto()
        //    {
        //        Amount = 1000000,
        //        AfterPaymentRedirectAddress = "https://www.hamafarin.ir/",
        //        InvoiceNumber = Guid.NewGuid().ToString(),
        //    };

        //    (bool IsSuccess, string Result) request = bankService.RequestToken(bankDto);
        //    if (request.IsSuccess)
        //    {
        //        var token = bankService.GetToken(request.Result);
        //        return Redirect(bankService.GetRedirectUrl(token));
        //    }

        //    return View();
        //}

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