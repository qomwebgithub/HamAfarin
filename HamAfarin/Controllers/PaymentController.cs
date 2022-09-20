using DataLayer;
using HamAfarin;
using HamAfarin.Classes;
using HamAfarin.Classes.Dto;
using HamAfarin.Classes.Interface;
using HamAfarin.ZarinPal;
using KooyWebApp_MVC.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Parbad;
using Parbad.Builder;
using Parbad.Gateway.Pasargad;
using Parbad.Gateway.Saman;
using Parbad.Gateway.ZarinPal;
using Parbad.Mvc;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ViewModels;
using HamAfarin.PaymentVerifySaman;

namespace Hamafarin.Controllers
{
    public class PaymentController : Controller
    {
        HamAfarinDBEntities db = new HamAfarinDBEntities();
        HamafarinPaymentDBEntities _paymentDb = new HamafarinPaymentDBEntities();
        PlanService planService = new PlanService();
        UserService userService = new UserService();
        ShaparakMessageEncoding ShaparakMessage = new ShaparakMessageEncoding();
        SmsService oSms = new SmsService();
        string zpSecret = "bfb039ca-b62b-434a-a7fc-ad2b53c981b0";

        // GET: Payment
        [Authorize]
        public ActionResult SelectPaymentType(int id)
        {
            int UserID = UserSetAuthCookie.GetUserID(User.Identity.Name);
            Tbl_UserProfiles qUserProfiles = db.Tbl_UserProfiles.FirstOrDefault(p => p.User_id == UserID);
            if (qUserProfiles != null && qUserProfiles.IsActive)
            {
                Tbl_BussinessPlans qBussinessPlans = db.Tbl_BussinessPlans.FirstOrDefault(p => p.BussinessPlanID == id && p.IsActive && p.IsDeleted == false);

                if (qBussinessPlans == null)
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);

                //if (qBussinessPlans.IsActive == false ||
                //    qBussinessPlans.InvestmentStartDate > DateTime.Now ||
                //    qBussinessPlans.InvestmentExpireDate < DateTime.Now)
                //{
                //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //}

                ViewBag.MinimumAmountInvest = qBussinessPlans.MinimumAmountInvest;
                ViewBag.UserProfileIsActive = true;

                SelectPaymentTypeViewModel selectPayment = new SelectPaymentTypeViewModel();
                selectPayment.BusinessPlanID = qBussinessPlans.BussinessPlanID;
                selectPayment.CompanyName = qBussinessPlans.CompanyName;
                selectPayment.BussinessName = qBussinessPlans.Title;
                selectPayment.FinancialDuration_id = qBussinessPlans.Tbl_BussinessPlan_FinancialDuration.FinancialDurationTitle;
                selectPayment.CodeOTC = qBussinessPlans.CodeOTC;
                selectPayment.ImageNameWarranty = qBussinessPlans.ImageNameWarranty;
                selectPayment.PercentageReturnInvestment = qBussinessPlans.PercentageReturnInvestment;
                selectPayment.PercentageReturnInvestment = qBussinessPlans.PercentageReturnInvestment;
                selectPayment.AmountRequiredRoRaiseCapital = qBussinessPlans.AmountRequiredRoRaiseCapital;
                int myUserId = UserSetAuthCookie.GetUserID(User.Identity.Name);
                Tbl_UserProfiles tbl_UserProfiles = db.Tbl_UserProfiles.FirstOrDefault(u => u.User_id == myUserId);
                selectPayment.InvestorFullName = tbl_UserProfiles.FirstName + " " + tbl_UserProfiles.LastName;
                selectPayment.InvestorSejamId = "";
                selectPayment.IsOnline = true;
                selectPayment.SiteTermsConditions = db.Tbl_Settings.FirstOrDefault().SiteTermsConditions;
                selectPayment.Privacy = db.Tbl_Settings.FirstOrDefault().Privacy;
                selectPayment.InvestorMobile = tbl_UserProfiles.MobileNumber;
                selectPayment.InvestorNationalCode = tbl_UserProfiles.NationalCode;
                selectPayment.ContractFileName = qBussinessPlans.ContractFileName;
                selectPayment.BusinessPlanRisks = qBussinessPlans.BusinessPlanRisks;
                // کل سرمایه گذاری شما
                selectPayment.TotalInvestment = planService.TotalUserInvestmentForPlan(db, id, myUserId);
                // حداکثر امکان سرمایه گذاری شما
                selectPayment.MinimumAmountInvest = Convert.ToInt64(qBussinessPlans.MinimumAmountInvest);
                // حداقل مبلغ برای سرمایه گذاری
                selectPayment.MinimumAmountInvest = Convert.ToInt64(qBussinessPlans.MinimumAmountInvest);
                // حداکثر مبلغ برای سرمایه گذاری
                selectPayment.MaximumInvestment = Convert.ToInt64((Convert.ToInt64(qBussinessPlans.AmountRequiredRoRaiseCapital) / 100) * qBussinessPlans.MaximumInvestmentPercentage);
                // حداکثر امکان سرمایه گذاری شما
                selectPayment.CanInvestment = selectPayment.MaximumInvestment - selectPayment.TotalInvestment;

                return View(selectPayment);

            }
            else
            {
                if (qUserProfiles == null)
                    ViewBag.UserProfileIsActive = null;
                else
                    ViewBag.UserProfileIsActive = false;
            }
            return View();
        }

        public ActionResult DargahSelect()
        {
            List<Tbl_Dargah> qlstDargah = db.Tbl_Dargah.Where(d => d.IsActive).ToList();
            return PartialView(qlstDargah);
        }


        [HttpPost]
        public async Task<ActionResult> SelectPaymentType(SelectPaymentTypeViewModel selectPaymentTypeViewModel, HttpPostedFileBase imgPaymentImageNameUploaded)
        {
            ViewBag.UserProfileIsActive = true;
            if (selectPaymentTypeViewModel.IsOnline)
            {
                ModelState.Remove("OfflinePaymentPrice");
                ModelState.Remove("PaymentImageName");
                ModelState.Remove("TransactionPaymentCode");
                Tbl_Dargah qDargah = db.Tbl_Dargah.FirstOrDefault(p => p.ID == selectPaymentTypeViewModel.Dargah && p.IsActive);
                if (qDargah == null)
                {
                    return View("SelectPaymentType/" + selectPaymentTypeViewModel.BusinessPlanID);
                }
                if (ModelState.IsValid)
                {
                    Tbl_BussinessPlans qBussinessPlans = db.Tbl_BussinessPlans.FirstOrDefault(p => p.BussinessPlanID == selectPaymentTypeViewModel.BusinessPlanID && p.IsActive && p.IsDeleted == false);

                    // حداقل مبلغ برای سرمایه گذاری
                    ViewBag.MinimumAmountInvest = qBussinessPlans.MinimumAmountInvest;
                    long MaximumInvestment = Convert.ToInt64((Convert.ToInt64(qBussinessPlans.AmountRequiredRoRaiseCapital) / 100) * qBussinessPlans.MaximumInvestmentPercentage);
                    // حداکثر مبلغ برای سرمایه گذاری
                    ViewBag.MaximumInvestment = MaximumInvestment;

                    int currentUserId = UserSetAuthCookie.GetUserID(User.Identity.Name);
                    bool userIslegal = db.Tbl_Users.Where(x => x.UserID == currentUserId).Select(x => x.IsLegal).FirstOrDefault();
                    Tbl_Users qUser = db.Tbl_Users.First(u => u.UserID == currentUserId);
                    Tbl_UserProfiles qUserProfile = db.Tbl_UserProfiles.FirstOrDefault(p => p.User_id == qUser.UserID);
                    string email = "";

                    if (qUserProfile != null)
                    {
                        email = qUserProfile.Email;
                    }
                    PaymentPriceValidation paymentPriceValidation = planService.PaymentPriceValidation(db, selectPaymentTypeViewModel.BusinessPlanID,
                        selectPaymentTypeViewModel.OnlinePaymentPrice, currentUserId, userIslegal);
                    if (paymentPriceValidation.Validation)
                    {
                        Tbl_BusinessPlanPayment tbl_BusinessPlanPayment = new Tbl_BusinessPlanPayment()
                        {
                            BusinessPlan_id = selectPaymentTypeViewModel.BusinessPlanID,
                            IsPaid = false,
                            IsConfirmedFromAdmin = false,
                            PaidDateTime = DateTime.Now,
                            CreateDate = DateTime.Now,
                            CreateUser_id = currentUserId,
                            PaymentUser_id = currentUserId,
                            PaymentPrice = selectPaymentTypeViewModel.OnlinePaymentPrice,
                            PaymentType_id = 2,
                            AdminCheckDate = null,
                            IsDelete = false,
                            PaymentStatus = 2,
                            IsReturned = false,
                            PaymentImageName = null,
                            InvoiceNumber = Generator.InvoiceNumber()
                        };
                        db.Tbl_BusinessPlanPayment.Add(tbl_BusinessPlanPayment);
                        db.SaveChanges();

                        Tbl_PaymentOnlineDetils tbl_PaymentOnline = new Tbl_PaymentOnlineDetils()
                        {
                            CreateDate = tbl_BusinessPlanPayment.CreateDate,
                            FinallyDate = null,
                            IsDelete = false,
                            IsFinally = false,
                            PaymentDetilsID = Guid.NewGuid().ToString(),
                            Payment_id = tbl_BusinessPlanPayment.PaymentID,
                            Dargah_id = selectPaymentTypeViewModel.Dargah
                        };
                        db.Tbl_PaymentOnlineDetils.Add(tbl_PaymentOnline);
                        decimal dclAmount = Convert.ToDecimal(tbl_BusinessPlanPayment.PaymentPrice) * 10;

                        string redirectAddress = "https://www.hamafarin.ir/Payment/VerifyPayment/" + tbl_PaymentOnline.PaymentDetilsID;



                        if (selectPaymentTypeViewModel.Dargah == 2)
                        {
                            #region Zarinpal
                            //زرین پال
                            int amount = (int)tbl_BusinessPlanPayment.PaymentPrice;
                            ServicePointManager.Expect100Continue = false;
                            var zp = new PaymentGatewayImplementationServicePortTypeClient();
                            string authority;
                            int Status = zp.PaymentRequest(zpSecret, amount,
                                $" طرح {selectPaymentTypeViewModel.BussinessName}",
                                qUserProfile.Email ?? "", qUser.MobileNumber, redirectAddress, out authority);

                            if (Status == 100)
                            {
                                tbl_PaymentOnline.ShaparakMessageGetToken = authority;
                                db.SaveChanges();
                                return Redirect($"https://www.zarinpal.com/pg/StartPay/{authority}");
                            }
                            #endregion

                        }
                        else
                        {
                            #region ParbadPayment
                            //string dargah;
                            //switch (selectPaymentTypeViewModel.Dargah)
                            //{
                            //    case 1: dargah = "Pasargad"; break;
                            //    case 2: dargah = "ZarinPal"; break;
                            //    default: dargah = "Saman"; break;
                            //};

                            //IOnlinePayment onlinePayment = OnlinePayment();
                            //long trackingNumber = long.Parse(tbl_BusinessPlanPayment.InvoiceNumber);

                            //IPaymentRequestResult result = await onlinePayment.RequestAsync(invoice =>
                            //    invoice
                            //        .SetTrackingNumber(trackingNumber)
                            //        .SetAmount(dclAmount)
                            //        .SetCallbackUrl(redirectAddress)
                            //        .SetGateway(dargah)
                            ////.SetZarinPalData(new ZarinPalInvoice(
                            ////    "test", "test@gmail.com", "09191155021"))
                            //);

                            //if (result.IsSucceed)
                            //{
                            //    var payment = await _paymentDb.Payment.Where(p => p.TrackingNumber == result.TrackingNumber).SingleOrDefaultAsync();
                            //    tbl_PaymentOnline.ShaparakMessageGetToken = payment.Token;
                            //    await db.SaveChangesAsync();

                            //    return result.GatewayTransporter.TransportToGateway();
                            //}
                            #endregion
                            #region BankPayment
                            IBankService bankService;
                            switch (selectPaymentTypeViewModel.Dargah)
                            {
                                //case 1: bankService = new PasargadBankService(); break;
                                //case 2: bankService = new ZarinPalBankService(); break;
                                default: bankService = new SamanBankService(); break;
                            };
                            string strUserNationalCode = UserSetAuthCookie.GetUserName(User.Identity.Name);

                            var bankDto = new BankInvoice()
                            {
                                UserEmail = email,
                                UserMobile = qUser.MobileNumber,
                                Description = $" طرح {selectPaymentTypeViewModel.BussinessName}",
                                Amount = (long)dclAmount,
                                AfterPaymentRedirectAddress = redirectAddress,
                                InvoiceDate = tbl_BusinessPlanPayment.CreateDate.Value,
                                InvoiceNumber = tbl_BusinessPlanPayment.InvoiceNumber,
                                TimeStamp = tbl_BusinessPlanPayment.CreateDate.Value,
                                PlanTitleAndCodeOTC = tbl_BusinessPlanPayment.Tbl_BussinessPlans.Title + " - " + tbl_BusinessPlanPayment.Tbl_BussinessPlans.CodeOTC,
                                UserNationalCode = strUserNationalCode
                            };

                            var request = await bankService.RequestAsync(bankDto);
                            tbl_PaymentOnline.ShaparakMessageGetToken = request.Result;

                            if (request.IsSuccess)
                            {
                                tbl_PaymentOnline.ShaparakToken = request.Token;
                                db.SaveChanges();
                                ViewBag.Token = request.Token;
                                return View();
                                // return Redirect(request.RedirectUrl);
                            }
                            #endregion
                        }


                        ViewBag.Massage = "فعلا ارتباط با درگاه مقدور نمی باشد";
                        db.SaveChanges();
                        return View(selectPaymentTypeViewModel);
                    }
                    else
                    {
                        // سرمایه گذاری من در این طرح
                        ViewBag.TotalMyInvestmentInPlan = planService.TotalUserInvestmentForPlan(db, selectPaymentTypeViewModel.BusinessPlanID
                            , currentUserId);
                        ModelState.AddModelError("OnlinePaymentPrice", paymentPriceValidation.Error.ToString());
                        return View(selectPaymentTypeViewModel);
                    }
                }
            }
            else
            {
                ModelState.Remove("OnlinePaymentPrice");
                if (ModelState.IsValid)
                {
                    Tbl_BussinessPlans qBussinessPlans = db.Tbl_BussinessPlans.FirstOrDefault(p => p.BussinessPlanID == selectPaymentTypeViewModel.BusinessPlanID && p.IsActive && p.IsDeleted == false);

                    // حداقل مبلغ برای سرمایه گذاری
                    ViewBag.MinimumAmountInvest = qBussinessPlans.MinimumAmountInvest;
                    long MaximumInvestment = Convert.ToInt64((Convert.ToInt64(qBussinessPlans.AmountRequiredRoRaiseCapital) / 100) * qBussinessPlans.MaximumInvestmentPercentage);
                    // حداکثر مبلغ برای سرمایه گذاری
                    ViewBag.MaximumInvestment = MaximumInvestment;

                    int currentUserId = UserSetAuthCookie.GetUserID(User.Identity.Name);
                    bool userIslegal = db.Tbl_Users.Where(x => x.UserID == currentUserId).Select(x => x.IsLegal).FirstOrDefault();

                    PaymentPriceValidation paymentPriceValidation = planService.PaymentPriceValidation(db, selectPaymentTypeViewModel.BusinessPlanID, selectPaymentTypeViewModel.OfflinePaymentPrice
                        , currentUserId, userIslegal);

                    if (paymentPriceValidation.Validation)
                    {
                        if (imgPaymentImageNameUploaded != null && imgPaymentImageNameUploaded.IsImage())
                        {
                            selectPaymentTypeViewModel.PaymentImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgPaymentImageNameUploaded.FileName);
                            imgPaymentImageNameUploaded.SaveAs(Server.MapPath("/Images/PaymentImages/" + selectPaymentTypeViewModel.PaymentImageName));

                        }
                        else
                        {
                            ModelState.AddModelError("PaymentImageName", "رسید بانکی را انتخاب کنید");
                            return View(selectPaymentTypeViewModel);
                        }
                        UserService userService = new UserService();

                        Tbl_BusinessPlanPayment tbl_BusinessPlanPayment = new Tbl_BusinessPlanPayment()
                        {
                            BusinessPlan_id = selectPaymentTypeViewModel.BusinessPlanID,
                            IsPaid = false,
                            IsConfirmedFromAdmin = false,
                            PaidDateTime = DateTime.Now,
                            CreateDate = DateTime.Now,
                            CreateUser_id = currentUserId,
                            PaymentUser_id = currentUserId,
                            TransactionPaymentCode = selectPaymentTypeViewModel.TransactionPaymentCode,
                            PaymentPrice = selectPaymentTypeViewModel.OfflinePaymentPrice,
                            PaymentType_id = 3,
                            PaymentImageName = selectPaymentTypeViewModel.PaymentImageName,
                            AdminCheckDate = null,
                            IsDelete = false,
                            PaymentStatus = 2
                        };
                        db.Tbl_BusinessPlanPayment.Add(tbl_BusinessPlanPayment);
                        db.SaveChanges();
                        ViewBag.IsPaymentSuccess = true;
                        return View("OfflinePayment");
                    }
                    else
                    {
                        // سرمایه گذاری من در این طرح
                        ViewBag.TotalMyInvestmentInPlan = planService.TotalUserInvestmentForPlan(db, selectPaymentTypeViewModel.BusinessPlanID
                            , currentUserId);
                        ModelState.AddModelError("OfflinePaymentPrice", paymentPriceValidation.Error.ToString());
                    }
                    if (imgPaymentImageNameUploaded != null) selectPaymentTypeViewModel.PaymentImageName = imgPaymentImageNameUploaded.FileName;
                    return View(selectPaymentTypeViewModel);
                }
            }
            return View("SelectPaymentType/" + selectPaymentTypeViewModel.BusinessPlanID);
        }

        //[HttpPost]
        //public ActionResult SelectPaymentType(SelectPaymentTypeViewModel selectPaymentTypeViewModel, HttpPostedFileBase imgPaymentImageNameUploaded)
        //{
        //    ViewBag.UserProfileIsActive = true;
        //    if (selectPaymentTypeViewModel.IsOnline)
        //    {
        //        ModelState.Remove("OfflinePaymentPrice");
        //        ModelState.Remove("PaymentImageName");
        //        ModelState.Remove("TransactionPaymentCode");
        //        Tbl_Dargah qDargah = db.Tbl_Dargah.FirstOrDefault(p => p.ID == selectPaymentTypeViewModel.Dargah && p.IsActive);
        //        if (qDargah == null)
        //        {
        //            return View("SelectPaymentType/" + selectPaymentTypeViewModel.BusinessPlanID);
        //        }
        //        if (ModelState.IsValid)
        //        {
        //            Tbl_BussinessPlans qBussinessPlans = db.Tbl_BussinessPlans.FirstOrDefault(p => p.BussinessPlanID == selectPaymentTypeViewModel.BusinessPlanID && p.IsActive && p.IsDeleted == false);

        //            // حداقل مبلغ برای سرمایه گذاری
        //            ViewBag.MinimumAmountInvest = qBussinessPlans.MinimumAmountInvest;
        //            long MaximumInvestment = Convert.ToInt64((Convert.ToInt64(qBussinessPlans.AmountRequiredRoRaiseCapital) / 100) * qBussinessPlans.MaximumInvestmentPercentage);
        //            // حداکثر مبلغ برای سرمایه گذاری
        //            ViewBag.MaximumInvestment = MaximumInvestment;

        //            int currentUserId = UserSetAuthCookie.GetUserID(User.Identity.Name);
        //            bool userIslegal = db.Tbl_Users.Where(x => x.UserID == currentUserId).Select(x => x.IsLegal).FirstOrDefault();
        //            Tbl_Users qUser = db.Tbl_Users.First(u => u.UserID == currentUserId);
        //            Tbl_UserProfiles qUserProfile = db.Tbl_UserProfiles.FirstOrDefault(p => p.User_id == qUser.UserID);
        //            string Email = "";

        //            if (qUserProfile != null)
        //            {
        //                Email = qUserProfile.Email;
        //            }
        //            PaymentPriceValidation paymentPriceValidation = planService.PaymentPriceValidation(db, selectPaymentTypeViewModel.BusinessPlanID,
        //                selectPaymentTypeViewModel.OnlinePaymentPrice, currentUserId, userIslegal);
        //            if (paymentPriceValidation.Validation)
        //            {
        //                Tbl_BusinessPlanPayment tbl_BusinessPlanPayment = new Tbl_BusinessPlanPayment()
        //                {
        //                    BusinessPlan_id = selectPaymentTypeViewModel.BusinessPlanID,
        //                    IsPaid = false,
        //                    IsConfirmedFromAdmin = false,
        //                    PaidDateTime = DateTime.Now,
        //                    CreateDate = DateTime.Now,
        //                    CreateUser_id = currentUserId,
        //                    PaymentUser_id = currentUserId,
        //                    PaymentPrice = selectPaymentTypeViewModel.OnlinePaymentPrice,
        //                    PaymentType_id = 2,
        //                    AdminCheckDate = null,
        //                    IsDelete = false,
        //                    PaymentStatus = 2,
        //                    IsReturned = false,
        //                    PaymentImageName = null,
        //                    InvoiceNumber = Generator.InvoiceNumber()
        //                };
        //                db.Tbl_BusinessPlanPayment.Add(tbl_BusinessPlanPayment);
        //                db.SaveChanges();

        //                Tbl_PaymentOnlineDetils tbl_PaymentOnline = new Tbl_PaymentOnlineDetils()
        //                {
        //                    CreateDate = tbl_BusinessPlanPayment.CreateDate,
        //                    FinallyDate = null,
        //                    IsDelete = false,
        //                    IsFinally = false,
        //                    PaymentDetilsID = Guid.NewGuid().ToString(),
        //                    Payment_id = tbl_BusinessPlanPayment.PaymentID,
        //                    Dargah_id = selectPaymentTypeViewModel.Dargah
        //                };
        //                db.Tbl_PaymentOnlineDetils.Add(tbl_PaymentOnline);
        //                decimal dclAmount = Convert.ToDecimal(tbl_BusinessPlanPayment.PaymentPrice) * 10;

        //                string redirectAddress = "https://www.hamafarin.ir/Payment/VerifyPayment/" + tbl_PaymentOnline.PaymentDetilsID;
        //                if (selectPaymentTypeViewModel.Dargah == 1)
        //                {
        //                    #region Pasargad
        //                    string timeStamp = tbl_BusinessPlanPayment.CreateDate.Value.ToString("yyyy/MM/dd HH:mm:ss");
        //                    string amount = dclAmount.ToString();
        //                    string invoiceNumber = tbl_BusinessPlanPayment.InvoiceNumber;
        //                    string ActionResult = "1003";
        //                    string invoiceDate = tbl_BusinessPlanPayment.CreateDate.Value.ToString("yyyy/MM/dd");
        //                    AppSettingsReader appSetting = new AppSettingsReader();
        //                    string merchantCode = "4650168";  //کد پذیرنده
        //                    string terminalCode = "1837060"; //کد ترمینال  

        //                    DataPost dp = new DataPost();
        //                    dp.InvoiceNumber = invoiceNumber;
        //                    dp.InvoiceDate = invoiceDate;
        //                    dp.MerchantCode = merchantCode;
        //                    dp.TerminalCode = terminalCode;
        //                    dp.Amount = amount;
        //                    dp.RedirectAddress = redirectAddress;
        //                    dp.Action = ActionResult;
        //                    dp.TimeStamp = timeStamp;
        //                    string output = JsonConvert.SerializeObject(dp);
        //                    string sign = GetSign(output);

        //                    byte[] textArray = Encoding.UTF8.GetBytes(output);
        //                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://pep.shaparak.ir/Api/v1/Payment/GetToken");
        //                    request.Method = "POST";
        //                    request.ContentType = "Application/Json";
        //                    request.ContentLength = textArray.Length;
        //                    request.Headers.Add("Sign", sign);

        //                    request.GetRequestStream().Write(textArray, 0, textArray.Length);
        //                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        //                    StreamReader reader = new StreamReader(response.GetResponseStream());
        //                    string result = reader.ReadToEnd();
        //                    tbl_PaymentOnline.ShaparakMessageGetToken = result;

        //                    if (result.Contains("Token"))
        //                    {
        //                        var res = result.Split(':', ',');
        //                        string token = res[1];
        //                        token = token.Replace("\"", "");

        //                        tbl_PaymentOnline.ShaparakToken = token;
        //                        db.SaveChanges();

        //                        Response.Redirect("https://pep.shaparak.ir/payment.aspx?n=" + token);
        //                        return View();
        //                    }

        //                    #endregion

        //                }
        //                else if (selectPaymentTypeViewModel.Dargah == 2)
        //                {
        //                    #region Zarinpal
        //                    //زرین پال
        //                    int amount = (int)tbl_BusinessPlanPayment.PaymentPrice;
        //                    ServicePointManager.Expect100Continue = false;
        //                    var zp = new PaymentGatewayImplementationServicePortTypeClient();
        //                    string authority;
        //                    int Status = zp.PaymentRequest(zpSecret, amount,
        //                        $" طرح {selectPaymentTypeViewModel.BussinessName}",
        //                        Email, qUser.MobileNumber, redirectAddress, out authority);

        //                    if (Status == 100)
        //                    {
        //                        tbl_PaymentOnline.ShaparakMessageGetToken = authority;
        //                        db.SaveChanges();
        //                        return Redirect($"https://www.zarinpal.com/pg/StartPay/{authority}");
        //                    }
        //                    #endregion

        //                }


        //                ViewBag.Massage = "فعلا ارتباط با درگاه مقدور نمی باشد";
        //                db.SaveChanges();
        //                return View(selectPaymentTypeViewModel);
        //            }
        //            else
        //            {
        //                // سرمایه گذاری من در این طرح
        //                ViewBag.TotalMyInvestmentInPlan = planService.TotalUserInvestmentForPlan(db, selectPaymentTypeViewModel.BusinessPlanID
        //                    , currentUserId);
        //                ModelState.AddModelError("OnlinePaymentPrice", paymentPriceValidation.Error.ToString());
        //                return View(selectPaymentTypeViewModel);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        ModelState.Remove("OnlinePaymentPrice");
        //        if (ModelState.IsValid)
        //        {
        //            Tbl_BussinessPlans qBussinessPlans = db.Tbl_BussinessPlans.FirstOrDefault(p => p.BussinessPlanID == selectPaymentTypeViewModel.BusinessPlanID && p.IsActive && p.IsDeleted == false);

        //            // حداقل مبلغ برای سرمایه گذاری
        //            ViewBag.MinimumAmountInvest = qBussinessPlans.MinimumAmountInvest;
        //            long MaximumInvestment = Convert.ToInt64((Convert.ToInt64(qBussinessPlans.AmountRequiredRoRaiseCapital) / 100) * qBussinessPlans.MaximumInvestmentPercentage);
        //            // حداکثر مبلغ برای سرمایه گذاری
        //            ViewBag.MaximumInvestment = MaximumInvestment;

        //            int currentUserId = UserSetAuthCookie.GetUserID(User.Identity.Name);
        //            bool userIslegal = db.Tbl_Users.Where(x => x.UserID == currentUserId).Select(x => x.IsLegal).FirstOrDefault();

        //            PaymentPriceValidation paymentPriceValidation = planService.PaymentPriceValidation(db, selectPaymentTypeViewModel.BusinessPlanID, selectPaymentTypeViewModel.OfflinePaymentPrice
        //                , currentUserId, userIslegal);

        //            if (paymentPriceValidation.Validation)
        //            {
        //                if (imgPaymentImageNameUploaded != null && imgPaymentImageNameUploaded.IsImage())
        //                {
        //                    selectPaymentTypeViewModel.PaymentImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgPaymentImageNameUploaded.FileName);
        //                    imgPaymentImageNameUploaded.SaveAs(Server.MapPath("/Images/PaymentImages/" + selectPaymentTypeViewModel.PaymentImageName));

        //                }
        //                else
        //                {
        //                    ModelState.AddModelError("PaymentImageName", "رسید بانکی را انتخاب کنید");
        //                    return View(selectPaymentTypeViewModel);
        //                }
        //                UserService userService = new UserService();

        //                Tbl_BusinessPlanPayment tbl_BusinessPlanPayment = new Tbl_BusinessPlanPayment()
        //                {
        //                    BusinessPlan_id = selectPaymentTypeViewModel.BusinessPlanID,
        //                    IsPaid = false,
        //                    IsConfirmedFromAdmin = false,
        //                    PaidDateTime = DateTime.Now,
        //                    CreateDate = DateTime.Now,
        //                    CreateUser_id = currentUserId,
        //                    PaymentUser_id = currentUserId,
        //                    TransactionPaymentCode = selectPaymentTypeViewModel.TransactionPaymentCode,
        //                    PaymentPrice = selectPaymentTypeViewModel.OfflinePaymentPrice,
        //                    PaymentType_id = 3,
        //                    PaymentImageName = selectPaymentTypeViewModel.PaymentImageName,
        //                    AdminCheckDate = null,
        //                    IsDelete = false,
        //                    PaymentStatus = 2
        //                };
        //                db.Tbl_BusinessPlanPayment.Add(tbl_BusinessPlanPayment);
        //                db.SaveChanges();
        //                ViewBag.IsPaymentSuccess = true;
        //                return View("OfflinePayment");
        //            }
        //            else
        //            {
        //                // سرمایه گذاری من در این طرح
        //                ViewBag.TotalMyInvestmentInPlan = planService.TotalUserInvestmentForPlan(db, selectPaymentTypeViewModel.BusinessPlanID
        //                    , currentUserId);
        //                ModelState.AddModelError("OfflinePaymentPrice", paymentPriceValidation.Error.ToString());
        //            }
        //            if (imgPaymentImageNameUploaded != null) selectPaymentTypeViewModel.PaymentImageName = imgPaymentImageNameUploaded.FileName;
        //            return View(selectPaymentTypeViewModel);
        //        }
        //    }
        //    return View("SelectPaymentType/" + selectPaymentTypeViewModel.BusinessPlanID);
        //}

        //[HttpGet]
        public async Task<ActionResult> VerifyPayment(string id)
        {
            //تست نوع درخواست
            //string methodType = HttpContext.Request.HttpMethod;
            //await oSms.SendSmsAsync("09033802138", methodType);

            Tbl_PaymentOnlineDetils qPaymentOnline = db.Tbl_PaymentOnlineDetils.FirstOrDefault(p => p.PaymentDetilsID == id);

            try
            {
                if (qPaymentOnline == null)
                    return View();

                Tbl_BusinessPlanPayment qBusinessPlanPayment = await db.Tbl_BusinessPlanPayment.FirstOrDefaultAsync(p => p.IsDelete == false && p.PaymentID == qPaymentOnline.Payment_id);

                long raisedPrice = planService.GetRaisedPrice(db, qBusinessPlanPayment.Tbl_BussinessPlans.BussinessPlanID) + qBusinessPlanPayment.PaymentPrice.Value;
                long totalPrice = long.Parse(qBusinessPlanPayment.Tbl_BussinessPlans.AmountRequiredRoRaiseCapital);
                bool isRefund = qBusinessPlanPayment.Tbl_BussinessPlans.IsOverflowInvestment == false && totalPrice < raisedPrice;

                #region OldCode
                //if (qPaymentOnline.Dargah_id == 1)
                //{
                //    IBankService bankService = new PasargadBankService();
                //    #region Pasargad
                //    #region NewCode
                //    //درگاه پاسارگاد
                //    //var requestDto = bankService.DeserializeBankRequest(Request.QueryString);
                //    //qPaymentOnline.TransactionReferenceID = requestDto.TransactionReferenceID;

                //    //// دریافت اطلاعات از درگاه
                //    //var checkTransaction = bankService.CheckTransaction(requestDto.TransactionReferenceID);
                //    //qPaymentOnline.ShaparakCheckTransactionResult = checkTransaction.Result;

                //    //if (!checkTransaction.IsSuccess)
                //    //{
                //    //    ViewBag.IsSuccess = false;
                //    //    ViewBag.Result = "عملیات ناموفق";
                //    //    return View();
                //    //}

                //    //requestDto.Amount = bankService.GetTransactionAmount(checkTransaction.Result);

                //    //long raisedPrice = planService.GetRaisedPrice(db, qBusinessPlanPayment.Tbl_BussinessPlans.BussinessPlanID) + qBusinessPlanPayment.PaymentPrice.Value;
                //    //long totalPrice = long.Parse(qBusinessPlanPayment.Tbl_BussinessPlans.AmountRequiredRoRaiseCapital);

                //    //if (qBusinessPlanPayment.Tbl_BussinessPlans.IsOverflowInvestment == false && totalPrice < raisedPrice)
                //    //{
                //    //    // برگشت وجه
                //    //    bankService.RefundPayment(requestDto);
                //    //    ViewBag.Result = "برگشت وجه";
                //    //    return View();
                //    //}

                //    //// تایید پرداخت به درگاه
                //    //var ShaparakRefNumber = bankService.ConfirmPayment(requestDto);

                //    //if (ShaparakRefNumber.IsSuccess)
                //    //{
                //    //    qPaymentOnline.ShaparakVerifyPayment = ShaparakRefNumber.Result;
                //    //    qPaymentOnline.IsFinally = true;
                //    //    qBusinessPlanPayment.TransactionPaymentCode = requestDto.TransactionReferenceID;
                //    //    qBusinessPlanPayment.IsPaid = true;
                //    //    qPaymentOnline.FinallyDate = DateTime.Now;
                //    //    db.SaveChanges();
                //    //    ViewBag.IsSuccess = true;
                //    //    ViewBag.TransactionReferenceID = requestDto.TransactionReferenceID;
                //    //    // 4 = سرمایه گذاری
                //    //    Tbl_Sms qSms = db.Tbl_Sms.Find(4);
                //    //    Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.UserID == qPaymentOnline.Tbl_BusinessPlanPayment.Tbl_BussinessPlans.User_id);
                //    //    (bool Success, string Message) result = oSms.SendSms(qUser.MobileNumber, qSms.Message);

                //    //    return RedirectToAction("SinglePaymentBusinessPlan", "UserPaymentBusinessPlan", new { area = "UserPanel", id = qPaymentOnline.Payment_id, notify = true });
                //    //}
                //    #endregion

                //    string invoiceNumber = Request.QueryString["iN"];
                //    string invoiceDate = Request.QueryString["iD"];
                //    string TransactionReferenceID = Request.QueryString["tref"];
                //    qPaymentOnline.TransactionReferenceID = TransactionReferenceID;

                //    // دریافت اطلاعات از درگاه
                //    string strResult = CheckTransactionResult(TransactionReferenceID);

                //    qPaymentOnline.ShaparakCheckTransactionResult = strResult;

                //    if (!strResult.Contains("ReferenceNumber"))
                //    {
                //        ViewBag.IsSuccess = false;
                //        ViewBag.Result = "عملیات ناموفق";
                //        return View();
                //    }

                //    var res = strResult.Split(':', ',');
                //    string[] pay = res[21].Split('.');

                //    if (isRefund)
                //    {
                //        // برگشت وجه
                //        string refundResult = RefundPayment(pay[0], invoiceNumber, invoiceDate);
                //        ViewBag.Result = "برگشت وجه";
                //        return View();
                //    }

                //    // تایید پرداخت به درگاه
                //    string ShaparakRefNumber = ConfirmPayment(pay[0], invoiceNumber, invoiceDate);

                //    var shaparakbum = ShaparakRefNumber.Split(':', ',');
                //    if (ShaparakRefNumber.Contains("ShaparakRefNumber"))
                //    {
                //        qPaymentOnline.ShaparakVerifyPayment = ShaparakRefNumber;
                //        qPaymentOnline.IsFinally = true;
                //        qBusinessPlanPayment.TransactionPaymentCode = TransactionReferenceID;
                //        qBusinessPlanPayment.IsPaid = true;
                //        qPaymentOnline.FinallyDate = DateTime.Now;
                //        db.SaveChanges();
                //        ViewBag.IsSuccess = true;
                //        ViewBag.TransactionReferenceID = TransactionReferenceID;

                //        // 4 = سرمایه گذاری
                //        Tbl_Sms qSms = db.Tbl_Sms.Find(4);
                //        Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.UserID == qPaymentOnline.Tbl_BusinessPlanPayment.Tbl_BussinessPlans.User_id);
                //        (bool Success, string Message) result = oSms.SendSms(qUser.MobileNumber, qSms.Message);

                //        return RedirectToAction("SinglePaymentBusinessPlan", "UserPaymentBusinessPlan", new { area = "UserPanel", id = qPaymentOnline.Payment_id, notify = true });
                //    }
                //    #endregion

                //}
                #endregion
                if (qPaymentOnline.Dargah_id == 2)
                {
                    // زرین پال
                    var status = Request.QueryString["Status"];
                    var authority = Request.QueryString["Authority"];
                    qPaymentOnline.FinallyDate = DateTime.Now;
                    qPaymentOnline.ShaparakToken = status;
                    qPaymentOnline.ShaparakVerifyPayment = authority;
                    db.SaveChanges();

                    //if (Request.QueryString["Status"] != "" && Request.QueryString["Status"] != null && Request.QueryString["Authority"] != "" && Request.QueryString["Authority"] != null)
                    //{
                    //    if (Request.QueryString["Status"].ToString().Equals("OK"))
                    //    {

                    int amount = (int)qBusinessPlanPayment.PaymentPrice * 10;
                    string RefID;

                    string url = "https://api.zarinpal.com/pg/v4/payment/verify.json?merchant_id=" +
                        zpSecret + "&amount=" + amount + "&authority=" + authority;

                    var client = new RestClient(url);
                    Method method = Method.Post;
                    var request = new RestRequest("", method);

                    request.AddHeader("accept", "application/json");
                    request.AddHeader("content-type", "application/json");

                    var response = client.ExecuteAsync(request);

                    JObject jodata = JObject.Parse(response.Result.Content);
                    string data = jodata["data"].ToString();

                    JObject jo = JObject.Parse(response.Result.Content);
                    string errors = jo["errors"].ToString();

                    if (data != "[]")
                    {
                        RefID = jodata["data"]["ref_id"].ToString();
                        qPaymentOnline.ShaparakVerifyPayment = authority;
                        qPaymentOnline.IsFinally = true;
                        qBusinessPlanPayment.TransactionPaymentCode = RefID.ToString();
                        qBusinessPlanPayment.IsPaid = true;
                        qPaymentOnline.FinallyDate = DateTime.Now;
                        qPaymentOnline.TransactionReferenceID = RefID.ToString();
                        qPaymentOnline.ShaparakCheckTransactionResult = data.ToString();
                        db.SaveChanges();
                        ViewBag.IsSuccess = true;
                        ViewBag.TransactionReferenceID = RefID;

                        // 4 = سرمایه گذاری
                        Tbl_Sms qSms = db.Tbl_Sms.Find(4);
                        Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.UserID == qPaymentOnline.Tbl_BusinessPlanPayment.Tbl_BussinessPlans.User_id);
                        oSms.SendSms(qUser.MobileNumber, qSms.Message);

                        return RedirectToAction("SinglePaymentBusinessPlan", "UserPaymentBusinessPlan", new { area = "UserPanel", id = qPaymentOnline.Payment_id, notify = true });
                    }
                    else if (errors != "[]")
                    {
                        string errorscode = jo["errors"]["code"].ToString();
                        qPaymentOnline.ShaparakVerifyPayment = errorscode;
                        db.SaveChanges();
                        ViewBag.IsSuccess = false;
                        ViewBag.Result = "عملیات ناموفق";
                        return View();
                    }

                    #region OldCode
                    //ServicePointManager.Expect100Continue = false;
                    //var zp = new PaymentGatewayImplementationServicePortTypeClient();

                    //int Status = zp.PaymentVerification(zpSecret, authority, amount, out RefID);
                    //qPaymentOnline.FinallyDate = DateTime.Now;
                    //qPaymentOnline.ShaparakCheckTransactionResult = Status.ToString();
                    //db.SaveChanges();
                    //if (Status == 100)
                    //{
                    //    qPaymentOnline.ShaparakVerifyPayment = authority;
                    //    qPaymentOnline.IsFinally = true;
                    //    qBusinessPlanPayment.TransactionPaymentCode = RefID.ToString();
                    //    qBusinessPlanPayment.IsPaid = true;
                    //    qPaymentOnline.FinallyDate = DateTime.Now;
                    //    db.SaveChanges();
                    //    ViewBag.IsSuccess = true;
                    //    ViewBag.TransactionReferenceID = RefID;

                    //    // 4 = سرمایه گذاری
                    //    Tbl_Sms qSms = db.Tbl_Sms.Find(4);
                    //    Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.UserID == qPaymentOnline.Tbl_BusinessPlanPayment.Tbl_BussinessPlans.User_id);
                    //    (bool Success, string Message) result = oSms.SendSms(qUser.MobileNumber, qSms.Message);

                    //    return RedirectToAction("SinglePaymentBusinessPlan", "UserPaymentBusinessPlan", new { area = "UserPanel", id = qPaymentOnline.Payment_id, notify = true });
                    //}
                    //else
                    //{
                    //    ViewBag.IsSuccess = false;
                    //    ViewBag.Result = "عملیات ناموفق";
                    //    return View();
                    //}
                    //    }
                    //    else
                    //    {
                    //        ViewBag.IsSuccess = false;
                    //        ViewBag.Result = "عملیات ناموفق";
                    //        return View();
                    //    }
                    //}
                    //else
                    //{
                    //    ViewBag.IsSuccess = false;
                    //    ViewBag.Result = "عملیات ناموفق";
                    //    return View();
                    //}
                    #endregion
                }
                else
                {
                    #region Parbad
                    //IOnlinePayment onlinePayment = OnlinePayment();
                    //IPaymentFetchResult invoice = await onlinePayment.FetchAsync();

                    //qPaymentOnline.ShaparakCheckTransactionResult = System.Text.Json.JsonSerializer.Serialize(invoice);

                    //// Check if the invoice is new or it's already processed before.
                    //if (invoice.Status != PaymentFetchResultStatus.ReadyForVerifying)
                    //{
                    //    ViewBag.IsSuccess = false;
                    //    ViewBag.Result = "عملیات ناموفق";
                    //    await db.SaveChangesAsync();
                    //    return View();
                    //}

                    //// This is an example of cancelling an invoice when you think that the payment process must be stopped.
                    //if (isRefund)
                    //{
                    //    var cancelResult = await onlinePayment.CancelAsync(invoice, cancellationReason: "متاسفانه ظرفیت طرح تکمیل شده است.");
                    //    ViewBag.Result = "برگشت وجه";
                    //    await db.SaveChangesAsync();
                    //    return View();
                    //}

                    //var verifyResult = await onlinePayment.VerifyAsync(invoice);

                    //var payment = await _paymentDb.Payment.Where(p => p.TrackingNumber == verifyResult.TrackingNumber).SingleOrDefaultAsync();

                    //qPaymentOnline.ShaparakVerifyPayment = payment.TransactionCode;
                    //qPaymentOnline.IsFinally = true;
                    //qBusinessPlanPayment.TransactionPaymentCode = payment.TransactionCode;
                    //qBusinessPlanPayment.IsPaid = true;
                    //qPaymentOnline.FinallyDate = DateTime.Now;
                    //await db.SaveChangesAsync();

                    //ViewBag.IsSuccess = true;
                    //ViewBag.TransactionReferenceID = payment.TransactionCode;

                    //Tbl_Sms qSms = await db.Tbl_Sms.FindAsync(4);
                    //Tbl_Users qUser = await db.Tbl_Users.FirstOrDefaultAsync(u => u.UserID == qPaymentOnline.Tbl_BusinessPlanPayment.Tbl_BussinessPlans.User_id);
                    //(bool Success, string Message) result = await oSms.SendSmsAsync(qUser.MobileNumber, qSms.Message);

                    //return RedirectToAction("SinglePaymentBusinessPlan", "UserPaymentBusinessPlan", new { area = "UserPanel", id = qPaymentOnline.Payment_id, notify = true });
                    #endregion

                    #region BankPayment
                    IBankService bankService;
                    switch (qPaymentOnline.Dargah_id)
                    {
                        //case 1: bankService = new PasargadBankService(); break;
                        //case 2: bankService = new ZarinPalBankService(); break;
                        default: bankService = new SamanBankService(); break;
                    };

                    //  BankCallbackResult callbackResult = bankService.Fetch(Request);

                    BankCallbackResult callbackResult = new BankCallbackResult();
                    callbackResult.TraceNo = Request.QueryString["TraceNo"];
                    callbackResult.SecurePan = Request.QueryString["SecurePan"];
                    callbackResult.MID = Request.QueryString["MID"];
                    callbackResult.Rrn = Request.QueryString["Rrn"];
                    callbackResult.State = Request.QueryString["State"];
                    callbackResult.ReferenceId = Request.QueryString["ResNum"];
                    callbackResult.TransactionId = Request.QueryString["RefNum"];

                    if (callbackResult.State == "OK")
                    {
                        qPaymentOnline.ShaparakCheckTransactionResult = callbackResult.TransactionId;
                        db.SaveChanges();
                    }
                    else
                    {
                        qPaymentOnline.ShaparakCheckTransactionResult = callbackResult.State;
                        db.SaveChanges();
                        return View();
                    }

                    //var isNotUniqueRefId = db.Tbl_PaymentOnlineDetils
                    //    .Any(p => p.TransactionReferenceID == callbackResult.TransactionId &&
                    //    p.Dargah_id == qPaymentOnline.Dargah_id && p.IsFinally);

                    //if (!callbackResult.IsSucceed || isNotUniqueRefId)
                    //{
                    //    ViewBag.IsSuccess = false;
                    //    ViewBag.Result = "عملیات ناموفق";
                    //    await db.SaveChangesAsync();
                    //    return View();
                    //}

                    //// This is an example of cancelling an invoice when you think that the payment process must be stopped.
                    //if (isRefund)
                    //{
                    //    var cancelResult = await onlinePayment.CancelAsync(invoice, cancellationReason: "متاسفانه ظرفیت طرح تکمیل شده است.");
                    //    ViewBag.Result = "برگشت وجه";
                    //    await db.SaveChangesAsync();
                    //    return View();
                    //}

                    // PaymentVerifyResult verifyResult = await bankService.VerifyAsync(callbackResult);
                    string _terminalId = "12949549";


                    PaymentIFBindingSoapClient payment = new PaymentIFBindingSoapClient();
                    // این مقدار در صورتی که مثبت باشد، مبلغ انتقالی (مبلغ کسر شده از کارت مشتری) را نشان میدهد و
                    //در صورتی که منفی باشد، معرف کد خطاست
                    double PaymentPrice = payment.verifyTransaction(callbackResult.TransactionId, _terminalId);

                    if (PaymentPrice < 0)
                    {
                        qPaymentOnline.ShaparakVerifyPayment = PaymentPrice.ToString();
                        db.SaveChanges();
                        return View();
                    }

                    qPaymentOnline.ShaparakVerifyPayment = PaymentPrice.ToString();
                    qPaymentOnline.IsFinally = true;
                    qBusinessPlanPayment.TransactionPaymentCode = callbackResult.ReferenceId;
                    qBusinessPlanPayment.IsPaid = true;
                    qPaymentOnline.FinallyDate = DateTime.Now;
                    await db.SaveChangesAsync();

                    ViewBag.IsSuccess = true;
                    ViewBag.TransactionReferenceID = callbackResult.TransactionId;

                    Tbl_Sms qSms = await db.Tbl_Sms.FindAsync(4);
                    // Tbl_Users qUser = await db.Tbl_Users.FirstOrDefaultAsync(u => u.UserID == qPaymentOnline.Tbl_BusinessPlanPayment.Tbl_BussinessPlans.User_id);
                    oSms.SendSms(UserSetAuthCookie.GetMobileNumber(User.Identity.Name), qSms.Message);

                    return RedirectToAction("SinglePaymentBusinessPlan", "UserPaymentBusinessPlan", new { area = "UserPanel", id = qPaymentOnline.Payment_id, notify = true });

                    #endregion
                }
            }
            catch (Exception e)
            {
                qPaymentOnline.ShaparakVerifyPayment = e.ToString();
                db.SaveChanges();
            }
            return View();
        }

        //[HttpPost]
        //[ActionName("VerifyPayment")]
        //public async Task<ActionResult> VerifyPaymentPost(string id)
        //{
        //    string methodType = HttpContext.Request.HttpMethod;
        //    await oSms.SendSmsAsync("09033802138", methodType);

        //    return Content("ok");
        //}

        [OutputCache(Duration = 86400)]
        public ActionResult InvestmentSummary()
        {
            List<Tbl_BusinessPlanPayment> qPayments = db.Tbl_BusinessPlanPayment.Where(p => p.IsConfirmedFromAdmin && p.IsPaid && p.IsDelete == false).ToList();
            int qActiveUsers = db.Tbl_Users.Where(p => p.IsActive && p.IsDeleted == false).Count();
            long qAmountCapitalRaised = qPayments.Sum(p => p.PaymentPrice).Value;
            //  int qInvestmentCountPerson = qPayments.Select(p => p.PaymentUser_id).Distinct().Count();
            int qInvestmentSuccessPlanCount = db.Tbl_BussinessPlans.Where(p => p.IsSuccessBussinessPlan && p.IsDeleted == false && p.IsActive).Count();
            long qTotalDepositToInvestors = db.Tbl_DepositToInvestorsDetails.Where(p => p.IsDelete == false && p.Tbl_DepositToInvestors.IsPaid && p.Tbl_DepositToInvestors.IsDelete == false).Sum(p => p.DepositAmount.Value);
            // بدست اوردن کل سود واریزی با یک رقم اعشار, میلیارد تومن
            double doubleTotalDepositToInvestors = Math.Round(qTotalDepositToInvestors / (double)1000000000, 1);

            int qCountDepositToInvestors = db.Tbl_DepositToInvestors.Count(p => p.IsDelete == false && p.IsPaid);
            InvestmentSummaryViewModel investmentSummaryViewModel = new InvestmentSummaryViewModel()
            {
                AmountCapitalRaised = Math.Round(qAmountCapitalRaised / (double)1000000000, 1),
                ActiveUsers = qActiveUsers,
                // InvestmentCountPerson = qInvestmentCountPerson,
                InvestmentSuccessPlanCount = qInvestmentSuccessPlanCount,
                TotalDepositToInvestors = doubleTotalDepositToInvestors,
                CountDepositToInvestors = qCountDepositToInvestors
            };
            return PartialView(investmentSummaryViewModel);
        }

        private string GetSign(string data)
        {
            var rsa = new RSACryptoServiceProvider();
            var secretKey = "<RSAKeyValue><Modulus>uXkZ3gR907p+1ygpEhNCrP0dSKiSBba4V/uBopMMWfg+z5bMhzJ759D5mLXo81aQboa30Djj6CQNGx+bd7wZYlx0z3WHZi1c9UH9lwIFvGnJ/9RpD+Blr06U6EHhe/mCw6Jsg2UausqX7bhkQyzWma7EbBgc+ieyd72ba9Fe/7U=</Modulus><Exponent>AQAB</Exponent><P>5v4scbAyu1Bq+LhEzPoilqx0RxgzUtz7i3F6463QPYBD3CmZVpwgQZXrQ4bE0XesSUl/BkcIrN/mywCbIJv0Zw==</P><Q>zY1fn8kPSkHAQnFVrfn1cqo3QE4uAJfJiv6boxvUUM2mWfK8ujvOkrUjTUZ/J+O2RzdIv0VCBGeWUeKEruo5gw==</Q><DP>Hiu0wmSxO6YVUsc+tUc2nVeJGIAgtAIJGP2Jf5OET4QhWPBWBun9jJN4VymTK4jmB+yBmuBMUcgs7Pb3TBsSoQ==</DP><DQ>Yoy+ZQBjuUlu4SwvVPs7h58+YDFbcuNTOLW7buc/0wHWGNf9ThiwgLwh0cHT4w8U7G4ADdwpu6zicB33WVlo+w==</DQ><InverseQ>e6u/g2rKgb6U3At6o20nIas7x1iIAGMPDvVJBu22lw/t0u4HPROjkavo/P+SOgWG9ziS5vfprGD8spgwixpXNQ==</InverseQ><D>AD/BYSLwaFBfyzoqk/Oiq0jLuUVArPFJ3hRgYC+CXLyQmQbCz4upzu3g5+uWnH0JRJy5snXhGHaz7c1lEAwYnKCdF5IS0sma5BqOPlCQHYBjp0FTN5jI2gepRP7TUV67e29BzE3IqS1zfPk4oq8XA0PvI1FZEGNAUYXVuYRnHiE=</D></RSAKeyValue>";
            rsa.FromXmlString(secretKey);
            byte[] signMain = rsa.SignData(Encoding.UTF8.GetBytes(data), new
            SHA1CryptoServiceProvider());
            string sign = Convert.ToBase64String(signMain);
            return sign;
        }

        //private string CheckTransactionResult(string TransactionReferenceID)
        //{
        //    PasargadBankRequestTokenDto dp = new PasargadBankRequestTokenDto();
        //    dp.TransactionReferenceID = TransactionReferenceID;
        //    string output = JsonConvert.SerializeObject(dp);
        //    string sign = GetSign(output);

        //    byte[] textArray = Encoding.UTF8.GetBytes(output);
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://pep.shaparak.ir/Api/v1/Payment/CheckTransactionResult");
        //    request.Method = "POST";
        //    request.ContentType = "Application/Json";
        //    request.ContentLength = textArray.Length;
        //    request.Headers.Add("Sign", sign);
        //    request.GetRequestStream().Write(textArray, 0, textArray.Length);
        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //    StreamReader reader = new StreamReader(response.GetResponseStream());
        //    string result = reader.ReadToEnd();
        //    return result;
        //}

        //private string ConfirmPayment(string amount, string invoiceNumber, string invoiceDate)
        //{
        //    string merchantCode = "4650168"; //کد پذیرنده
        //    string terminalCode = "1837060"; //کد ترمینال  
        //    string timeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

        //    PasargadBankRequestTokenDto dp = new PasargadBankRequestTokenDto();
        //    dp.InvoiceNumber = invoiceNumber;
        //    dp.InvoiceDate = invoiceDate;
        //    dp.MerchantCode = merchantCode;
        //    dp.TerminalCode = terminalCode;
        //    dp.Amount = amount.ToString();
        //    dp.TimeStamp = timeStamp;
        //    string output = JsonConvert.SerializeObject(dp);
        //    string sign = GetSign(output);

        //    byte[] textArray = Encoding.UTF8.GetBytes(output);
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://pep.shaparak.ir/Api/v1/Payment/VerifyPayment");
        //    request.Method = "POST";
        //    request.ContentType = "Application/Json";
        //    request.ContentLength = textArray.Length;
        //    request.Headers.Add("Sign", sign);

        //    request.GetRequestStream().Write(textArray, 0, textArray.Length);
        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        //    StreamReader reader = new StreamReader(response.GetResponseStream());
        //    string result = reader.ReadToEnd();
        //    return result;
        //}

        //private string RefundPayment(string amount, string invoiceNumber, string invoiceDate)
        //{
        //    string merchantCode = "4650168"; //کد پذیرنده
        //    string terminalCode = "1837060"; //کد ترمینال  
        //    string timeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

        //    PasargadBankRequestTokenDto dp = new PasargadBankRequestTokenDto();
        //    dp.InvoiceNumber = invoiceNumber;
        //    dp.InvoiceDate = invoiceDate;
        //    dp.MerchantCode = merchantCode;
        //    dp.TerminalCode = terminalCode;
        //    dp.Amount = amount.ToString();
        //    dp.TimeStamp = timeStamp;
        //    string output = JsonConvert.SerializeObject(dp);
        //    string sign = GetSign(output);

        //    byte[] textArray = Encoding.UTF8.GetBytes(output);
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://pep.shaparak.ir/Api/v1/Payment/RefundPayment");
        //    request.Method = "POST";
        //    request.ContentType = "Application/Json";
        //    request.ContentLength = textArray.Length;
        //    request.Headers.Add("Sign", sign);

        //    request.GetRequestStream().Write(textArray, 0, textArray.Length);
        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        //    StreamReader reader = new StreamReader(response.GetResponseStream());
        //    string result = reader.ReadToEnd();
        //    return result;
        //}

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
                        .AddGateway<PasargadGateway>()
                        .WithAccounts(source =>
                        {
                            source.AddInMemory(account =>
                            {
                                account.MerchantCode = "4650168";
                                account.TerminalCode = "1837060";
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

        #region Test Code

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

            IPaymentRequestResult result = await onlinePayment.RequestAsync(invoice =>
                invoice
                    .SetTrackingNumber(new Random().Next(1, 1000))
                    .SetAmount(10000)
                    .SetCallbackUrl("https://hamafarin.ir/Payment/TestVerify")
                    .SetGateway("Saman")
            //.SetZarinPalData(new ZarinPalInvoice(
            //    "test", "test@gmail.com", "09191155021"))
            );

            if (!result.IsSucceed)
                throw new Exception();

            return result.GatewayTransporter.TransportToGateway();
        }

        //[HttpGet, HttpPost]
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

        //public ActionResult TestReturn1()
        //{
        //    DateTime now = DateTime.Now;

        //    string token = "";
        //    string timeStamp = now.ToString("yyyy/MM/dd HH:mm:ss");
        //    string amount = "10000";
        //    string invoiceNumber = "HAfarin-000904-2";
        //    string actionResult = "1003";
        //    string invoiceDate = now.ToString("yyyy/MM/dd");
        //    string merchantCode = "4650168";  //کد پذیرنده
        //    string terminalCode = "1837060"; //کد ترمینال  
        //    string redirectAddress = "https://www.hamafarin.ir/Payment/TestReturn2";

        //    DataPost dp = new DataPost();
        //    dp.InvoiceNumber = invoiceNumber;
        //    dp.InvoiceDate = invoiceDate;
        //    dp.MerchantCode = merchantCode;
        //    dp.TerminalCode = terminalCode;
        //    dp.Amount = amount;
        //    dp.RedirectAddress = redirectAddress;
        //    dp.Action = actionResult;
        //    dp.TimeStamp = timeStamp;
        //    string output = JsonConvert.SerializeObject(dp);
        //    string sign = GetSign(output);

        //    byte[] textArray = Encoding.UTF8.GetBytes(output);
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://pep.shaparak.ir/Api/v1/Payment/GetToken");
        //    request.Method = "POST";
        //    request.ContentType = "Application/Json";
        //    request.ContentLength = textArray.Length;
        //    request.Headers.Add("Sign", sign);

        //    request.GetRequestStream().Write(textArray, 0, textArray.Length);
        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        //    StreamReader reader = new StreamReader(response.GetResponseStream());
        //    string result = reader.ReadToEnd();

        //    //Tbl_PaymentOnlineDetils tbl_PaymentOnline = new Tbl_PaymentOnlineDetils()
        //    //{
        //    //    CreateDate = now,
        //    //    FinallyDate = null,
        //    //    IsDelete = false,
        //    //    IsFinally = false,
        //    //    PaymentDetilsID = Guid.NewGuid().ToString(),
        //    //    Payment_id = null,
        //    //};
        //    //db.Tbl_PaymentOnlineDetils.Add(tbl_PaymentOnline);

        //    //tbl_PaymentOnline.ShaparakMessageGetToken = result;

        //    if (result.Contains("Token"))
        //    {
        //        var res = result.Split(':', ',');

        //        token = res[1];

        //        token = token.Replace("\"", "");

        //        //tbl_PaymentOnline.ShaparakToken = token;
        //        //db.SaveChanges();

        //        Response.Redirect("https://pep.shaparak.ir/payment.aspx?n=" + token);
        //    }
        //    else
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    //return View();
        //    return new HttpStatusCodeResult(HttpStatusCode.OK);
        //}

        //public ActionResult TestReturn2()
        //{
        //    string invoiceNumber = Request.QueryString["iN"];
        //    string invoiceDate = Request.QueryString["iD"];
        //    string TransactionReferenceID = Request.QueryString["tref"];

        //    // دریافت اطلاعات از درگاه
        //    string strResult = ReadPaymentResult(TransactionReferenceID);

        //    var res = strResult.Split(':', ',');
        //    string[] pay = res[21].Split('.');

        //    // برگشت وجه
        //    string refundResult = RefundPayment(pay[0], invoiceNumber, invoiceDate);
        //    ViewBag.Result = "برگشت وجه";

        //    return new HttpStatusCodeResult(HttpStatusCode.OK);
        //    //return View();
        //}

        #endregion
    }
}