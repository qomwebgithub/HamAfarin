using DataLayer;
using HamAfarin.Classes;
using Parbad;
using Parbad.Builder;
using Parbad.Gateway.Mellat;
using Parbad.Gateway.ParbadVirtual;
using Parbad.Gateway.Pasargad;
using Parbad.Gateway.Saman;
using Parbad.Gateway.ZarinPal;
using Parbad.Mvc;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HamAfarin.Areas.Admin.Controllers
{
    public class AdminPanelController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();
        private HamafarinPaymentDBEntities paymentDb = new HamafarinPaymentDBEntities();

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
