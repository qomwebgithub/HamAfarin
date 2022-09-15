using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels;
using Common;
using AutoMapper;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using System.Globalization;

namespace HamAfarin.Areas.Admin.Controllers
{
    public class Tbl_DepositToInvestorsController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();
        SmsService oSms = new SmsService();

        // GET: UserPanel/DepositToInvestors
        public ActionResult Index()
        {
            List<Tbl_DepositToInvestors> listTbl_DepositToInvestors = db.Tbl_DepositToInvestors
                .Where(d => d.IsDelete == false)
                .OrderByDescending(d => d.CreateDate)
                .ToList();

            return View(listTbl_DepositToInvestors);
        }

        public FileResult ExcelReport(int id)
        {
            List<InvestorViewModel> depositProfileList = (
                from u in db.Tbl_Users
                join UserProfiles in db.Tbl_UserProfiles
                    on u.UserID equals UserProfiles.User_id into UserGroup
                from p in UserGroup.DefaultIfEmpty()
                join PersonLegal in db.Tbl_PersonLegal
                    on u.UserID equals PersonLegal.User_id into LegalGroup
                from l in LegalGroup.DefaultIfEmpty()
                join DepositToInvestorsDetails in db.Tbl_DepositToInvestorsDetails
                    on p.User_id equals DepositToInvestorsDetails.InvestorUser_id into InvestorGroup
                from i in InvestorGroup.DefaultIfEmpty()
                where i.Deposit_id == id && i.IsDelete == false
                select new InvestorViewModel
                {
                    UserID = u.UserID,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    CompanyName = l.CompanyName,
                    NationalId = p.NationalCode,
                    CompanyId = l.NationalId,
                    MobileNumber = p.MobileNumber,
                    Sheba = p.AccountSheba,
                    DepositAmount = (long)i.DepositAmount,
                    TotalPaymentPrice = (long)i.InvestmentAmount,
                }
            ).ToList();

            Tbl_DepositToInvestors deposit = db.Tbl_DepositToInvestors.FirstOrDefault(d => d.DepositID == id);

            List<string> lstColumnsName = new List<string>
            {
                "شماره سپرده ی مبدا",
                "شماره مشتری",
                "مبلغ",
                "کد بانک مقصد",
                "نام و نام خانوادگی ذینفع",
                "شرح تراکنش(بابت)",
                "شبای مقصد",
                "تاریخ ارسال",
                "شناسه واریز(اختیاری)",
                "نام",
                "نام خانوادگی",
                "کد ملی",
                "موبایل",
                "تاریخ واریز",
                "نوع واریز",
                "درصد واریز",
                "کل مبلغ سرمایه گذاری"
            };

            DataTable dt = new DataTable("جزییات واریز");

            foreach (var item in lstColumnsName)
            {
                dt.Columns.Add(item);
            }

            foreach (var item in depositProfileList)
            {
                dt.Rows.Add(
                    null,
                    null,
                    item.DepositAmount * 10,
                    item.Sheba.Substring(5, 2),
                    item.CompanyName ?? item.FirstName + " " + item.LastName,
                    null,
                    item.Sheba.Replace("IR", ""),
                    null,
                    null,
                    item.CompanyName == null ? item.FirstName : null,
                    item.CompanyName ?? item.LastName,
                    item.CompanyId ?? item.NationalId,
                    item.MobileNumber,
                    deposit.DepositDate.Value.ToString("yyyy/MM/dd"),
                    deposit.Tbl_DepositTypes.DepositTypeName,
                    deposit.YieldPercent,
                    item.TotalPaymentPrice * 10
                );
            }

            using (XLWorkbook wb = new XLWorkbook()) //Install ClosedXml from Nuget for XLWorkbook  
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream()) //using System.IO;  
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"فهرست واریزی برای  {deposit.Tbl_BussinessPlans.Title} ({DateTime.Now.ToString("yyyy-MM-dd")}).xlsx");
                }
            }

        }

        public ActionResult Details(int id)
        {
            #region OldQueryBeforeDataBaseView
            //جوین چهار جدول
            //var listInvestorsViewModel = from u in db.Tbl_Users
            //                             join UserProfiles in db.Tbl_UserProfiles
            //                                  on u.UserID equals UserProfiles.User_id into UserGroup
            //                             from p in UserGroup.DefaultIfEmpty()
            //                             join PersonLegal in db.Tbl_PersonLegal
            //                                  on u.UserID equals PersonLegal.User_id into LegalGroup
            //                             from l in LegalGroup.DefaultIfEmpty()
            //                             join DepositToInvestorsDetails in db.Tbl_DepositToInvestorsDetails
            //                                  on p.User_id equals DepositToInvestorsDetails.InvestorUser_id into InvestorGroup
            //                             from i in InvestorGroup.DefaultIfEmpty()
            //                             where i.Deposit_id == id
            //                             select new InvestorViewModel
            //                             {
            //                                 UserID = u.UserID,
            //                                 FirstName = p.FirstName,
            //                                 LastName = p.LastName,
            //                                 CompanyName = l.CompanyName,
            //                                 MobileNumber = p.MobileNumber,
            //                                 Shaba = p.AccountSheba,
            //                                 DepositAmount = (long)i.DepositAmount
            //                             };
            #endregion

            List<View_DepositToInvestorsDetails> listInvestorsViewModel = db.View_DepositToInvestorsDetails.Where(x => x.Deposit_id == id).ToList();
            return View(listInvestorsViewModel);
        }

        public ActionResult Create()
        {
            DepositToInvestorsViewModel depositToInvestors = new DepositToInvestorsViewModel();

            depositToInvestors.PlansList = db.Tbl_BussinessPlans
                .Where(p => p.IsActive && p.IsDeleted == false)
                .OrderByDescending(p => p.CreateDate)
                .Select(p => new PlanViewModel { PlanID = p.BussinessPlanID, PlanName = p.Title })
                .ToList();

            depositToInvestors.DepositTypesList = db.Tbl_DepositTypes
                .Select(p => new DepositTypeViewModel { TypeID = p.DepositTypeID, TypeName = p.DepositTypeName })
                .ToList();

            return View(depositToInvestors);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DepositToInvestorsViewModel depositToInvestors)
        {
            depositToInvestors.DepositDate = StringExtensions.StringToDate(depositToInvestors.StringDepositDate);
            depositToInvestors.PlansList = db.Tbl_BussinessPlans
                .Where(p => p.IsActive && p.IsDeleted == false)
                .Select(p => new PlanViewModel { PlanID = p.BussinessPlanID, PlanName = p.Title })
                .ToList();
            depositToInvestors.DepositTypesList = db.Tbl_DepositTypes
                .Select(p => new DepositTypeViewModel { TypeID = p.DepositTypeID, TypeName = p.DepositTypeName })
                .ToList();

            if (ModelState.IsValid == false)
            {
                return View(depositToInvestors);
            }

            depositToInvestors.YieldPercent = decimal.Parse(depositToInvestors.StringYieldPercent, CultureInfo.InvariantCulture);

            if (depositToInvestors.YieldPercent <= 0)
            {
                ModelState.AddModelError("StringYieldPercent", "درصد واریز باید بزرگتر از 0 باشد");
                return View(depositToInvestors);
            }

            int userIdentity = UserSetAuthCookie.GetUserID(User.Identity.Name);
            DateTime dateTimeNow = DateTime.Now;

            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DepositToInvestorsViewModel, Tbl_DepositToInvestors>();
            });
            IMapper iMapper = config.CreateMapper();
            Tbl_DepositToInvestors tblDepositToInvestors = iMapper.Map<DepositToInvestorsViewModel, Tbl_DepositToInvestors>(depositToInvestors);
            tblDepositToInvestors.IsDelete = false;
            tblDepositToInvestors.IsPaid = false;
            tblDepositToInvestors.CreateUser_id = userIdentity;
            tblDepositToInvestors.CreateDate = dateTimeNow;
            if (tblDepositToInvestors.IsPaid)
                tblDepositToInvestors.PaidDate = dateTimeNow;

            db.Tbl_DepositToInvestors.Add(tblDepositToInvestors);
            db.SaveChanges();

            int depositID = tblDepositToInvestors.DepositID;

            //در صورت به خطا خوردن مرحله دوم ذخیره، اطلاعات مرحله اول پاک شود
            try
            {
                List<InvestorViewModel> listInvestorsViewModel = db.Tbl_BusinessPlanPayment
                    .Where(b => b.IsDelete == false &&
                        b.BusinessPlan_id == depositToInvestors.Plan_id &&
                        b.IsConfirmedFromFaraboors)
                    .GroupBy(c => c.PaymentUser_id)
                    .Select(g => new InvestorViewModel
                    {
                        UserID = (int)g.Key,
                        FirstPaymentDate = (DateTime)g.Select(b => b.PaidDateTime).FirstOrDefault(),
                        TotalPaymentPrice = (long)g.Sum(b => b.PaymentPrice),
                        DepositAmount = (long)(depositToInvestors.YieldPercent / 100 * (decimal)g.Sum(b => b.PaymentPrice)),
                    })
                    .OrderBy(g => g.FirstPaymentDate)
                    .ToList();

                List<Tbl_DepositToInvestorsDetails> listTbl_DepositToInvestorsDetails = new List<Tbl_DepositToInvestorsDetails>();
                foreach (InvestorViewModel investor in listInvestorsViewModel)
                {
                    listTbl_DepositToInvestorsDetails.Add(new Tbl_DepositToInvestorsDetails()
                    {
                        IsDelete = false,
                        Deposit_id = tblDepositToInvestors.DepositID,
                        InvestorUser_id = investor.UserID,
                        InvestmentAmount = investor.TotalPaymentPrice,
                        DepositAmount = investor.DepositAmount,
                        CreateDate = dateTimeNow,
                    });
                }

                long? totalDepositAmount = listTbl_DepositToInvestorsDetails.Select(d => d.DepositAmount).Sum();
                tblDepositToInvestors.TotalDeposit = totalDepositAmount;
                db.Tbl_DepositToInvestorsDetails.AddRange(listTbl_DepositToInvestorsDetails);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                db.Tbl_DepositToInvestors.Remove(db.Tbl_DepositToInvestors.Find(depositID));
                db.SaveChanges();
                throw e;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ActionResult> DepositCalculation(int id, decimal percent = 0)
        {
            List<InvestorViewModel> listInvestorsViewModel = await db.Tbl_BusinessPlanPayment.AsNoTracking()
                .Where(b => b.IsDelete == false &&
                    b.BusinessPlan_id == id &&
                    b.IsConfirmedFromFaraboors)
                .GroupBy(c => c.PaymentUser_id)
                .Select(g => new InvestorViewModel
                {
                    UserID = (int)g.Key,
                    FirstName = g.Select(a => a.Tbl_Users.Tbl_UserProfiles.Select(u => u.FirstName).FirstOrDefault()).FirstOrDefault(),
                    LastName = g.Select(a => a.Tbl_Users.Tbl_UserProfiles.Select(u => u.LastName).FirstOrDefault()).FirstOrDefault(),
                    NationalId = g.Select(a => a.Tbl_Users.Tbl_UserProfiles.Select(u => u.NationalCode).FirstOrDefault()).FirstOrDefault(),
                    CompanyName = g.Select(a => a.Tbl_Users.Tbl_PersonLegal.Select(u => u.CompanyName).FirstOrDefault()).FirstOrDefault(),
                    CompanyId = g.Select(a => a.Tbl_Users.Tbl_PersonLegal.Select(u => u.NationalId).FirstOrDefault()).FirstOrDefault(),
                    MobileNumber = g.Select(a => a.Tbl_Users.Tbl_UserProfiles.Select(u => u.MobileNumber).FirstOrDefault()).FirstOrDefault(),
                    Sheba = g.Select(a => a.Tbl_Users.Tbl_UserProfiles.Select(u => u.AccountSheba).FirstOrDefault()).FirstOrDefault(),
                    FirstPaymentDate = (DateTime)g.Select(b => b.PaidDateTime).FirstOrDefault(),
                    TotalPaymentPrice = (long)g.Sum(b => b.PaymentPrice),
                    DepositAmount = (long)(percent / 100 * (decimal)g.Sum(b => b.PaymentPrice))
                })
                .OrderBy(g => g.FirstPaymentDate)
                .ToListAsync();

            return Json(listInvestorsViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> ConfirmDeposit(int id)
        {
            List<string> mobileNumbersList = await db.Tbl_DepositToInvestorsDetails
                .Join(db.Tbl_Users,
                    d => d.InvestorUser_id,
                    u => u.UserID,
                    (deposit, user) => new { deposit, user })
                .Join(db.Tbl_UserProfiles,
                    c => c.user.UserID,
                    p => p.User_id,
                    (combinedEntry, profile) => new
                    {
                        Deposit_id = combinedEntry.deposit.Deposit_id,
                        IsDelete = combinedEntry.deposit.IsDelete,
                        Mobile = profile.MobileNumber
                    })
                .Where(f => f.Deposit_id == id && f.IsDelete == false)
                .Select(f => f.Mobile)
                .Distinct()
                .ToListAsync();

            string mobileNumbers = String.Join(",", mobileNumbersList);

            // 9 = واریز
            Tbl_Sms qSms = await db.Tbl_Sms.FindAsync(9);
            string message = qSms.Message;

            Tbl_DepositToInvestors qTbl_DepositToInvestors = await db.Tbl_DepositToInvestors.FirstOrDefaultAsync(d => d.IsDelete == false && d.DepositID == id);

            //متن داینامیک
            if (message.Contains("@T"))
                message = message.Replace("@T", qTbl_DepositToInvestors.Tbl_BussinessPlans.Title);

            #region SmsActive
            (bool Success, string Message) smsResult = await oSms.SendSmsAsync(mobileNumbers, message);

            if (smsResult.Success)
            {
                qTbl_DepositToInvestors.IsPaid = true;
                qTbl_DepositToInvestors.DepositDate = DateTime.Now;
                await db.SaveChangesAsync();
            }

            return Json(new { success = smsResult.Success, message = smsResult.Success ? "عملیات با موفقیت انجام شد" : smsResult.Message });
            #endregion
            #region SmsDisabled
            //qTbl_DepositToInvestors.IsPaid = true;
            //qTbl_DepositToInvestors.DepositDate = DateTime.Now;
            //await db.SaveChangesAsync();
            //return Json(new { success = true, message = "عملیات با موفقیت انجام شد" });
            #endregion
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}