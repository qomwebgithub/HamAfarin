using DataLayer;
using HamAfarin.Classes;
using HamAfarin.Classes.Dto;
using HamAfarin.Classes.Interface;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Parbad;
using Parbad.Abstraction;
using Parbad.Builder;
using Parbad.Gateway.Mellat;
using Parbad.Gateway.ParbadVirtual;
using Parbad.Gateway.Pasargad;
using Parbad.Gateway.Saman;
using Parbad.Gateway.ZarinPal;
using Parbad.GatewayBuilders;
using Parbad.Mvc;
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

        public async Task<ActionResult> TestPay()
        {
            var onlinePayment = OnlinePayment();
            //Tbl_Dargah qDargah = db.Tbl_Dargah.FirstOrDefault(p => p.IsActive);

            //string gatewayName;
            //switch (qDargah.ID)
            //{
            //    case 1: gatewayName = "Pasargad"; break;
            //    case 2: gatewayName = "ZarinPal"; break;
            //    default: gatewayName = "Saman"; break;
            //};

            //Invoice invoice = new Invoice()
            //{
            //    GatewayName = "ParbadVirtual",
            //    TrackingNumber = new Random().Next(1,1000),
            //    Amount = 10000,
            //    CallbackUrl = new CallbackUrl("http://localhost:4677/Admin/AdminPanel/TestVerify"),
            //    //CallbackUrl = Url.Action("TestVerify", "AdminPanel", null, Request.Url.Scheme)
            //};

            IPaymentRequestResult result = await onlinePayment.RequestAsync(invoice =>
                invoice
                    .SetTrackingNumber(new Random().Next(1, 1000))
                    .SetAmount(10000)
                    .SetCallbackUrl("https://hamafarin.ir/Admin/AdminPanel/TestVerify")
                    .SetGateway("Saman")
                    //.SetZarinPalData(new ZarinPalInvoice(
                    //    "test", "test@gmail.com", "09191155021"))
            );

            if (!result.IsSucceed)
                throw new Exception();

            return result.GatewayTransporter.TransportToGateway();
        }

        [HttpGet, HttpPost]
        public async Task<ActionResult> TestVerify()
        {
            var onlinePayment = OnlinePayment();
            var invoice = await onlinePayment.FetchAsync();

            // Check if the invoice is new or it's already processed before.
            if (invoice.Status != PaymentFetchResultStatus.ReadyForVerifying)
            {
                // You can also see if the invoice is already verified before.
                var isAlreadyVerified = invoice.IsAlreadyVerified;
                return Content("پرداخت نا موفق");
            }

            // This is an example of cancelling an invoice when you think that the payment process must be stopped.
            //if (!Is_There_Still_Product_In_Shop(invoice.TrackingNumber))
            if (false)
            {
                var cancelResult = await onlinePayment.CancelAsync(invoice, cancellationReason: "Sorry, We have no more products to sell.");

                return View("CancelResult", cancelResult);
            }

            var verifyResult = await onlinePayment.VerifyAsync(invoice);

            //return View(verifyResult);
            return Content("پرداخت موفق.");
        }
        private IOnlinePayment OnlinePayment()
        {
            return ParbadBuilder.CreateDefaultBuilder()
                .ConfigureGateways(gateways =>
                {
                    gateways
                        .AddGateway<SamanGateway>()
                        .WithAccounts(source =>
                        {
                            source.AddInMemory(account =>
                            {
                                account.MerchantId = "12949549";
                                account.Password = "1837060";
                            });
                        });

                    gateways
                        .AddGateway<PasargadGateway>()
                        .WithAccounts(source =>
                        {
                            source.AddInMemory(account =>
                            {
                                account.MerchantCode = "4650168";
                                account.TerminalCode = "1837060";
                                account.PrivateKey = "<RSAKeyValue><Modulus>uXkZ3gR907p+1ygpEhNCrP0dSKiSBba4V/uBopMMWfg+z5bMhzJ759D5mLXo81aQboa30Djj6CQNGx+bd7wZYlx0z3WHZi1c9UH9lwIFvGnJ/9RpD+Blr06U6EHhe/mCw6Jsg2UausqX7bhkQyzWma7EbBgc+ieyd72ba9Fe/7U=</Modulus><Exponent>AQAB</Exponent><P>5v4scbAyu1Bq+LhEzPoilqx0RxgzUtz7i3F6463QPYBD3CmZVpwgQZXrQ4bE0XesSUl/BkcIrN/mywCbIJv0Zw==</P><Q>zY1fn8kPSkHAQnFVrfn1cqo3QE4uAJfJiv6boxvUUM2mWfK8ujvOkrUjTUZ/J+O2RzdIv0VCBGeWUeKEruo5gw==</Q><DP>Hiu0wmSxO6YVUsc+tUc2nVeJGIAgtAIJGP2Jf5OET4QhWPBWBun9jJN4VymTK4jmB+yBmuBMUcgs7Pb3TBsSoQ==</DP><DQ>Yoy+ZQBjuUlu4SwvVPs7h58+YDFbcuNTOLW7buc/0wHWGNf9ThiwgLwh0cHT4w8U7G4ADdwpu6zicB33WVlo+w==</DQ><InverseQ>e6u/g2rKgb6U3At6o20nIas7x1iIAGMPDvVJBu22lw/t0u4HPROjkavo/P+SOgWG9ziS5vfprGD8spgwixpXNQ==</InverseQ><D>AD/BYSLwaFBfyzoqk/Oiq0jLuUVArPFJ3hRgYC+CXLyQmQbCz4upzu3g5+uWnH0JRJy5snXhGHaz7c1lEAwYnKCdF5IS0sma5BqOPlCQHYBjp0FTN5jI2gepRP7TUV67e29BzE3IqS1zfPk4oq8XA0PvI1FZEGNAUYXVuYRnHiE=</D></RSAKeyValue>";
                            });
                        });

                    gateways
                        .AddGateway<ZarinPalGateway>()
                        .WithAccounts(source =>
                        {
                            source.AddInMemory(account =>
                            {
                                account.MerchantId = "";
                                account.AuthorizationToken = "bfb039ca-b62b-434a-a7fc-ad2b53c981b0";
                                account.IsSandbox = false;
                            });
                        });

                    //gateways.AddParbadVirtual()
                    //    .WithOptions(options => options.GatewayPath = "/MyVirtualGateway");
                })
                .ConfigureHttpContext(builder => builder.UseOwinFromCurrentHttpContext())
                .ConfigureStorage(builder => builder.AddStorage(new PaymentStorage()))
                .Build().OnlinePayment;
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
