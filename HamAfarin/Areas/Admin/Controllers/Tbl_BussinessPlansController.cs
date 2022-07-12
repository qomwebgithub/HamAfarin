using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using ClosedXML.Excel;
using DataLayer;
using HamAfarin;
using InsertShowImage;
using KooyWebApp_MVC.Classes;
using Newtonsoft.Json;
using ViewModels;
using ViewModels.Api;

namespace Hamafarin.Areas.Admin.Controllers
{
    public class Tbl_BussinessPlansController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();
        UserService userService = new UserService();
        SMS oSms = new SMS();

        // GET: Admin/Tbl_BussinessPlans
        public ActionResult Index()
        {
            var tbl_BussinessPlans = db.Tbl_BussinessPlans.Include(t => t.Tbl_BussinessPlan_BussenessFields)
                .Include(t => t.Tbl_BussinessPlan_FinancialDuration).Include(t => t.Tbl_CompanyType)
                .Include(t => t.Tbl_MonetaryUnits).Include(t => t.Tbl_Users)
                .Where(t => t.IsDeleted == false);
            return View(tbl_BussinessPlans.OrderByDescending(c => c.CreateDate).ToList());
        }

        public FileResult ExcelReport(int id)
        {
            var investorsProfileList = db.Tbl_BusinessPlanPayment
                .Where(b => b.IsDelete == false &&
                    b.BusinessPlan_id == id &&
                    b.IsConfirmedFromFaraboors)
                .GroupBy(c => c.PaymentUser_id)
                .Select(g => new
                {
                    UserID = (int)g.Key,
                    Profile = g.Select(a => a.Tbl_Users.Tbl_UserProfiles.FirstOrDefault()).FirstOrDefault(),
                    PersonLegal = g.Select(a => a.Tbl_Users.Tbl_PersonLegal.FirstOrDefault()).FirstOrDefault(),
                    TotalPaymentPrice = (long)g.Sum(b => b.PaymentPrice),
                })
                .ToList();

            List<string> lstColumnsName = new List<string> { "نام", "نام خانوادگی", "کد ملی", "موبایل", "ش حساب شبا", "کل سرمایه گذاری" };

            DataTable dt = new DataTable("Grid");
            foreach (var item in lstColumnsName)
            {
                dt.Columns.Add(item);
            }

            foreach (var item in investorsProfileList)
            {
                dt.Rows.Add(
                    item.PersonLegal == null ? item.Profile.FirstName : "",
                    item.PersonLegal == null ? item.Profile.LastName : item.PersonLegal.CompanyName,
                    item.PersonLegal == null ? item.Profile.NationalCode : item.PersonLegal.NationalId,
                    item.Profile.MobileNumber,
                    item.Profile.AccountSheba,
                    item.TotalPaymentPrice
                );
            }

            string title = db.Tbl_BussinessPlans
                .Where(b => b.BussinessPlanID == id)
                .Select(b => b.Title)
                .FirstOrDefault();

            using (XLWorkbook wb = new XLWorkbook()) //Install ClosedXml from Nuget for XLWorkbook  
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream()) //using System.IO;  
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"فهرست سرمایه گذاران  {title} ({DateTime.Now.ToString("yyyy-MM-dd")}).xlsx");
                }
            }
        }

        // GET: Admin/Tbl_BussinessPlans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_BussinessPlans tbl_BussinessPlans = db.Tbl_BussinessPlans.Find(id);
            if (tbl_BussinessPlans == null)
            {
                return HttpNotFound();
            }
            return View(tbl_BussinessPlans);
        }

        public ActionResult FbCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> FbCreate(string faraboorsProjectId)
        {
            if (string.IsNullOrWhiteSpace(faraboorsProjectId))
            {
                ViewBag.Error = "لطفا آیدی را وارد کنید";
                return View();
            }

            FaraboorsClass fb = new FaraboorsClass();

            (bool Success, string Message) result = await fb.GetProjectInfoAsync(faraboorsProjectId);

            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }

            ProjectInfoDto dto = JsonConvert.DeserializeObject<ProjectInfoDto>(result.Message);

            Tbl_BussinessPlans bussinessPlan = new Tbl_BussinessPlans();

            bussinessPlan.User_id = UserSetAuthCookie.GetUserID(User.Identity.Name);
            bussinessPlan.IsActive = false;
            bussinessPlan.IsDeleted = false;

            bussinessPlan.FaraboorsProjectId = dto.TraceCode;
            bussinessPlan.CreateDate = dto.CreationDate;
            bussinessPlan.Title = dto.PersianName;
            bussinessPlan.CodeOTC = dto.PersoanApprovedSymbol; //نام این فیلد گفته بودند ویرایش شود

            //فیلد های زیر اضافه شود
            //bp. = dto.EnglishApprovedSymbol;
            //bp. = dto.IndustryGroupDescription;
            //bp. = dto.SubIndustryGroupDescription;

            bussinessPlan.AmountRequiredRoRaiseCapital = dto.TotalPrice.ToString();
            bussinessPlan.MinimumAmountInvest = dto.RealPersonMinimumAvailabePrice.ToString();
            bussinessPlan.InvestmentStartDate = dto.ApprovedUnderwritingStartDate;
            bussinessPlan.InvestmentExpireDate = dto.ApprovedUnderwritingEndDate;

            //فیلد زیر اضافه شود
            //bp. = dto.ProjectStatusDescription;

            ProjectOwnerCompany ownerDto = dto.ProjectOwnerCompany.FirstOrDefault();
            if (ownerDto != null)
            {
                bussinessPlan.CompanyNationalCertificateCode = ownerDto.NationalID.ToString();
                bussinessPlan.CompanyName = ownerDto.Name;
                bussinessPlan.CompanyType_id = ownerDto.CompanyTypeID;
                bussinessPlan.CompanyRegisterCode = ownerDto.RegistrationNumber;
                bussinessPlan.CompanyRegisterDate = ownerDto.RegistrationDate;
                bussinessPlan.CompanyEconomicCode = ownerDto.EconomicID;
                bussinessPlan.CompanyRegisterAddress = ownerDto.Address;
                bussinessPlan.CompanyPostalCode = ownerDto.PostalCode;
            }

            //1 = مدیرعامل
            ListOfProjectBoardMember ceoDto = dto.ListOfProjectBoardMembers.FirstOrDefault(m => m.OrganizationPostID == 1);
            if (ceoDto != null)
            {
                bussinessPlan.CompanyAgentFullName = ceoDto.FirstName + " " + ceoDto.LastName;
                bussinessPlan.CompanyAgentPhoneNumber = ceoDto.MobileNumber;
                bussinessPlan.CompanyAgentEmail = ceoDto.EmailAddress;
                bussinessPlan.CompanyAgentRole = ceoDto.OrganizationPostDescription;
            }

            db.Tbl_BussinessPlans.Add(bussinessPlan);
            db.SaveChanges();
            return RedirectToAction(actionName: "Index");

        }

        // GET: Admin/Tbl_BussinessPlans/Create
        public ActionResult Create()
        {
            ViewBag.BussinessField_id = new SelectList(db.Tbl_BussinessPlan_BussenessFields, "BussinessFieldID", "BussinessFieldTitle");
            ViewBag.FinancialDuration_id = new SelectList(db.Tbl_BussinessPlan_FinancialDuration, "FinancialDurationID", "FinancialDurationTitle");
            ViewBag.BussinessPlanStatus_id = new SelectList(db.Tbl_BussinessPlan_Status, "BussinessPlanStatusID", "StatusTitle");
            ViewBag.CompanyType_id = new SelectList(db.Tbl_CompanyType, "CompanyTypeID", "CompanyTypeTitle");
            ViewBag.MonetaryUnit_id = new SelectList(db.Tbl_MonetaryUnits, "MonetaryUnitID", "MonetaryUnitTitle");
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName");
            return View(new AdminCreateEditBusinessPlan()
            {
                IsActive = true,
                MonetaryUnit_id = 2
            });
        }

        // POST: Admin/Tbl_BussinessPlans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AdminCreateEditBusinessPlan upsertBusinessPlan)
        {
            if (ModelState.IsValid == false)
                return View(upsertBusinessPlan);

            ////////////****************/////////////////////////////
            // تبدیل تاریخ تولد از 
            // string
            // به
            // datetime
            //upsertBusinessPlan.InvestmentStartDate = StringExtensions.StringToDate(upsertBusinessPlan.strInvestmentStartDate);
            //upsertBusinessPlan.InvestmentExpireDate = StringExtensions.StringToDate(upsertBusinessPlan.strInvestmentExpireDate);
            //upsertBusinessPlan.CompanyRegisterDate = StringExtensions.StringToDate(upsertBusinessPlan.strCompanyRegisterDate);
            //upsertBusinessPlan.PreviousInvestorDate = StringExtensions.StringToDate(upsertBusinessPlan.strPreviousInvestorDate);
            //upsertBusinessPlan.PreviousInvestorExpireDate = StringExtensions.StringToDate(upsertBusinessPlan.strPreviousInvestorExpireDate);
            ////////////****************/////////////////////////////

            upsertBusinessPlan.ImageNameWarranty =
                SaveNewImage(upsertBusinessPlan.ImageWarrantyFile, upsertBusinessPlan.ImageNameWarranty,
                    "/Resources/BusinessPlans/Warranty/");

            upsertBusinessPlan.ImageNameInSinglePlan = SaveNewImage(
                upsertBusinessPlan.ImageInSinglePlanFile, upsertBusinessPlan.ImageNameInSinglePlan,
                "/Images/BusinessPlans/Image/", "/Images/BusinessPlans/Thumb/");

            upsertBusinessPlan.ImageNameInListPalns = SaveNewImage(
                upsertBusinessPlan.ImageInListPalnsFile, upsertBusinessPlan.ImageNameInListPalns,
                "/Images/BusinessPlans/Image/", "/Images/BusinessPlans/Thumb/");

            upsertBusinessPlan.BussinessLogoImageName = SaveNewImage(
                upsertBusinessPlan.ImageLogoFile, upsertBusinessPlan.BussinessLogoImageName,
                "/Images/BusinessPlans/Logo/Image/", "/Images/BusinessPlans/Logo/Thumb/");


            upsertBusinessPlan.CompanyIntroductionLetterFileName = SaveNewFile(
                upsertBusinessPlan.LetterFile, upsertBusinessPlan.CompanyIntroductionLetterFileName,
                "/Resources/BusinessPlans/Letter/");

            upsertBusinessPlan.BussinessModelFileName = SaveNewFile(
                upsertBusinessPlan.ModelFile, upsertBusinessPlan.BussinessModelFileName,
                "/Resources/BusinessPlans/Model/");

            upsertBusinessPlan.SlideShowPresentationFileName = SaveNewFile(
                upsertBusinessPlan.SlideFile, upsertBusinessPlan.SlideShowPresentationFileName,
                "/Resources/BusinessPlans/Slide/");

            upsertBusinessPlan.DocumentsAndReportsFileName = SaveNewFile(
                upsertBusinessPlan.ReportFile, upsertBusinessPlan.DocumentsAndReportsFileName,
                "/Resources/BusinessPlans/Report/");

            upsertBusinessPlan.IntroductionIdeaVideoFileName = SaveNewFile(
                upsertBusinessPlan.IdeaFile, upsertBusinessPlan.IntroductionIdeaVideoFileName,
                "/Resources/BusinessPlans/Idea/");

            upsertBusinessPlan.ContractFileName = SaveNewFile(
                upsertBusinessPlan.ContractFile, upsertBusinessPlan.ContractFileName,
                "/Resources/BusinessPlans/Contract/");


            upsertBusinessPlan.CreateDate = DateTime.Now;
            upsertBusinessPlan.User_id = UserSetAuthCookie.GetUserID(User.Identity.Name);

            // حذف فاصله های اضافی از اعداد
            upsertBusinessPlan.AmountRequiredRoRaiseCapital = upsertBusinessPlan.AmountRequiredRoRaiseCapital.Trim();
            upsertBusinessPlan.MinimumAmountInvest = upsertBusinessPlan.MinimumAmountInvest.Trim();

            //مپ کردن مدل طرح کاربری به طرح اصلی
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AdminCreateEditBusinessPlan, Tbl_BussinessPlans>();
            });
            IMapper iMapper = config.CreateMapper();
            var tblBussinessPlans = iMapper.Map<AdminCreateEditBusinessPlan, Tbl_BussinessPlans>(upsertBusinessPlan);
            if (tblBussinessPlans.TitleUrl == null)
            {
                tblBussinessPlans.TitleUrl = tblBussinessPlans.Title.Trim().Replace(" ", "-");
            }
            else
            {
                tblBussinessPlans.TitleUrl = tblBussinessPlans.TitleUrl.Trim().Replace(" ", "-");
            }
            tblBussinessPlans.IsSuccessBussinessPlan = false;
            db.Tbl_BussinessPlans.Add(tblBussinessPlans);
            db.SaveChanges();
            // کنترل گالری عکس
            if (upsertBusinessPlan.GalleryPlanFiles != null && upsertBusinessPlan.GalleryPlanFiles.Any())
            {
                foreach (HttpPostedFileBase item in upsertBusinessPlan.GalleryPlanFiles)
                {
                    string imageName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                    item.SaveAs(Server.MapPath("/Images/BusinessPlans/Image/" + imageName));

                    ImageResizer img = new ImageResizer(500);
                    img.Resize(Server.MapPath("/Images/BusinessPlans/Image/" + imageName),
                        Server.MapPath("/Images/BusinessPlans/Thumb/" + imageName));
                    db.Tbl_BusinessPlanGallery.Add(new Tbl_BusinessPlanGallery()
                    {
                        BusinessPlan_id = tblBussinessPlans.BussinessPlanID,
                        ImageName = imageName,
                        IsActive = true,
                        IsDelete = false
                    });
                }
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Admin/Tbl_BussinessPlans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Tbl_BussinessPlans tbl_BussinessPlans = db.Tbl_BussinessPlans.Find(id);

            if (tbl_BussinessPlans == null)
                return HttpNotFound();

            ViewBag.BussinessField_id = new SelectList(db.Tbl_BussinessPlan_BussenessFields, "BussinessFieldID", "BussinessFieldTitle", tbl_BussinessPlans.BussinessField_id);
            ViewBag.FinancialDuration_id = new SelectList(db.Tbl_BussinessPlan_FinancialDuration, "FinancialDurationID", "FinancialDurationTitle", tbl_BussinessPlans.FinancialDuration_id);
            ViewBag.CompanyType_id = new SelectList(db.Tbl_CompanyType, "CompanyTypeID", "CompanyTypeTitle", tbl_BussinessPlans.CompanyType_id);
            ViewBag.MonetaryUnit_id = new SelectList(db.Tbl_MonetaryUnits, "MonetaryUnitID", "MonetaryUnitTitle", tbl_BussinessPlans.MonetaryUnit_id);
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_BussinessPlans.User_id);
            //مپ کردن مدل طرح کاربری به طرح اصلی
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Tbl_BussinessPlans, AdminCreateEditBusinessPlan>();
            });
            IMapper iMapper = config.CreateMapper();
            var adminCreateEditBusinessPlan = iMapper.Map<Tbl_BussinessPlans, AdminCreateEditBusinessPlan>(tbl_BussinessPlans);
            List<Tbl_BusinessPlanGallery> qGallery = db.Tbl_BusinessPlanGallery.Where(g => g.BusinessPlan_id == adminCreateEditBusinessPlan.BussinessPlanID).ToList();
            adminCreateEditBusinessPlan.GalleryPlan = qGallery;
            ViewBag.Video = adminCreateEditBusinessPlan.IntroductionIdeaVideoFileName;
            return View(adminCreateEditBusinessPlan);
        }

        // POST: Admin/Tbl_BussinessPlans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AdminCreateEditBusinessPlan upsertBusinessPlan)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.BussinessField_id = new SelectList(
                db.Tbl_BussinessPlan_BussenessFields, "BussinessFieldID",
                "BussinessFieldTitle", upsertBusinessPlan.BussinessField_id);

                ViewBag.FinancialDuration_id = new SelectList(
                    db.Tbl_BussinessPlan_FinancialDuration, "FinancialDurationID",
                    "FinancialDurationTitle", upsertBusinessPlan.FinancialDuration_id);

                ViewBag.CompanyType_id = new SelectList(
                    db.Tbl_CompanyType, "CompanyTypeID", "CompanyTypeTitle",
                    upsertBusinessPlan.CompanyType_id);

                ViewBag.MonetaryUnit_id = new SelectList(
                    db.Tbl_MonetaryUnits, "MonetaryUnitID", "MonetaryUnitTitle",
                    upsertBusinessPlan.MonetaryUnit_id);

                ViewBag.User_id = new SelectList(
                    db.Tbl_Users, "UserID", "UserName", upsertBusinessPlan.User_id);

                ViewBag.Video = upsertBusinessPlan.IntroductionIdeaVideoFileName;

                return View(upsertBusinessPlan);
            }

            upsertBusinessPlan.ImageNameWarranty =
                SaveNewImage(upsertBusinessPlan.ImageWarrantyFile, upsertBusinessPlan.ImageNameWarranty,
                    "/Resources/BusinessPlans/Warranty/");

            upsertBusinessPlan.ImageNameInSinglePlan = SaveNewImage(
                upsertBusinessPlan.ImageInSinglePlanFile, upsertBusinessPlan.ImageNameInSinglePlan,
                "/Images/BusinessPlans/Image/", "/Images/BusinessPlans/Thumb/");

            upsertBusinessPlan.ImageNameInListPalns = SaveNewImage(
                upsertBusinessPlan.ImageInListPalnsFile, upsertBusinessPlan.ImageNameInListPalns,
                "/Images/BusinessPlans/Image/", "/Images/BusinessPlans/Thumb/");

            upsertBusinessPlan.BussinessLogoImageName = SaveNewImage(
                upsertBusinessPlan.ImageLogoFile, upsertBusinessPlan.BussinessLogoImageName,
                "/Images/BusinessPlans/Logo/Image/", "/Images/BusinessPlans/Logo/Thumb/");


            upsertBusinessPlan.CompanyIntroductionLetterFileName = SaveNewFile(
                upsertBusinessPlan.LetterFile, upsertBusinessPlan.CompanyIntroductionLetterFileName,
                "/Resources/BusinessPlans/Letter/");

            upsertBusinessPlan.BussinessModelFileName = SaveNewFile(
                upsertBusinessPlan.ModelFile, upsertBusinessPlan.BussinessModelFileName,
                "/Resources/BusinessPlans/Model/");

            upsertBusinessPlan.SlideShowPresentationFileName = SaveNewFile(
                upsertBusinessPlan.SlideFile, upsertBusinessPlan.SlideShowPresentationFileName,
                "/Resources/BusinessPlans/Slide/");

            upsertBusinessPlan.DocumentsAndReportsFileName = SaveNewFile(
                upsertBusinessPlan.ReportFile, upsertBusinessPlan.DocumentsAndReportsFileName,
                "/Resources/BusinessPlans/Report/");

            upsertBusinessPlan.IntroductionIdeaVideoFileName = SaveNewFile(
                upsertBusinessPlan.IdeaFile, upsertBusinessPlan.IntroductionIdeaVideoFileName,
                "/Resources/BusinessPlans/Idea/");

            upsertBusinessPlan.ContractFileName = SaveNewFile(
                upsertBusinessPlan.ContractFile, upsertBusinessPlan.ContractFileName,
                "/Resources/BusinessPlans/Contract/");

            // حذف فاصله های اضافی از اعداد
            upsertBusinessPlan.AmountRequiredRoRaiseCapital =
                upsertBusinessPlan.AmountRequiredRoRaiseCapital.Trim();

            upsertBusinessPlan.MinimumAmountInvest = upsertBusinessPlan.MinimumAmountInvest.Trim();

            //مپ کردن مدل طرح کاربری به طرح اصلی
            MapperConfiguration config = new MapperConfiguration(cfg =>
            { cfg.CreateMap<AdminCreateEditBusinessPlan, Tbl_BussinessPlans>(); });
            IMapper iMapper = config.CreateMapper();

            Tbl_BussinessPlans tbl_BussinessPlans =
                iMapper.Map<AdminCreateEditBusinessPlan, Tbl_BussinessPlans>(upsertBusinessPlan);

            if (tbl_BussinessPlans.TitleUrl == null)
                tbl_BussinessPlans.TitleUrl = tbl_BussinessPlans.Title.Trim().Replace(" ", "-");

            db.Entry(tbl_BussinessPlans).State = EntityState.Modified;
            db.SaveChanges();

            // کنترل گالری عکس
            if (upsertBusinessPlan.GalleryPlanFiles != null && upsertBusinessPlan.GalleryPlanFiles.Any())
            {
                foreach (HttpPostedFileBase item in upsertBusinessPlan.GalleryPlanFiles)
                {
                    string imageName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                    item.SaveAs(Server.MapPath("/Images/BusinessPlans/Image/" + imageName));

                    ImageResizer img = new ImageResizer(500);
                    img.Resize(Server.MapPath("/Images/BusinessPlans/Image/" + imageName),
                        Server.MapPath("/Images/BusinessPlans/Thumb/" + imageName));

                    db.Tbl_BusinessPlanGallery.Add(new Tbl_BusinessPlanGallery()
                    {
                        BusinessPlan_id = tbl_BussinessPlans.BussinessPlanID,
                        ImageName = imageName,
                        IsActive = true,
                        IsDelete = false
                    });
                }
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public void DoSuccessPlan(int id)
        {
            if (UserSetAuthCookie.GetRoleID(User.Identity.Name) == 1)
            {
                Tbl_BussinessPlans qPlan = db.Tbl_BussinessPlans.Find(id);
                qPlan.IsSuccessBussinessPlan = true;
                db.SaveChanges();
            }
        }

        // GET: Admin/Tbl_BussinessPlans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_BussinessPlans tbl_BussinessPlans = db.Tbl_BussinessPlans.Find(id);
            if (tbl_BussinessPlans == null)
            {
                return HttpNotFound();
            }
            return View(tbl_BussinessPlans);
        }

        // POST: Admin/Tbl_BussinessPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_BussinessPlans tbl_BussinessPlans = db.Tbl_BussinessPlans.Find(id);
            tbl_BussinessPlans.IsDeleted = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> SendSMSCertificateReady(int id)
        {
            List<string> lstMobile = await db.Tbl_BusinessPlanPayment
                .Include(p => p.Tbl_Users)
                .Where(p => p.BusinessPlan_id == id && p.IsConfirmedFromFaraboors && p.IsDelete == false)
                .Select(p => p.Tbl_Users.MobileNumber)
                .Distinct()
                .ToListAsync();
            var mobileNumbers = String.Join(",", lstMobile);
            // 6 = صدور گواهی شراکت
            Tbl_Sms qSms = await db.Tbl_Sms.FindAsync(6);
            string message = qSms.Message;
            if (message.Contains("@T"))
            {
                Tbl_BussinessPlans qBussinessPlan = await db.Tbl_BussinessPlans.FirstOrDefaultAsync(b => b.BussinessPlanID == id);
                message = message.Replace("@T", qBussinessPlan.Title);
            }
            (bool Success, string Message) smsResult = await oSms.SendSmsAsync(mobileNumbers, message);

            return Json(new { success = smsResult.Success, message = smsResult.Message });
        }

        /// <summary>
        /// حذف تصویر از گالری
        /// </summary>
        /// <param name="id">شناسه تصویر در گالری</param>
        public void DeleteImageGallery(int id)
        {
            Tbl_BusinessPlanGallery qGallery = db.Tbl_BusinessPlanGallery.Find(id);
            System.IO.File.Delete(Server.MapPath("/Images/BusinessPlans/Image/" + qGallery.ImageName));
            System.IO.File.Delete(Server.MapPath("/Images/BusinessPlans/Thumb/" + qGallery.ImageName));
            db.Tbl_BusinessPlanGallery.Remove(qGallery);
            db.SaveChanges();
        }

        /// <summary>
        /// حذف ویدیو
        /// </summary>
        /// <param name="id">شناسه ویدیو</param>
        public void DeleteVideo(int id)
        {
            Tbl_BussinessPlans qBussinessPlans = db.Tbl_BussinessPlans.Find(id);
            System.IO.File.Delete(Server.MapPath("/Resources/BusinessPlans/Idea/" + qBussinessPlans.IntroductionIdeaVideoFileName));
            qBussinessPlans.IntroductionIdeaVideoFileName = null;
            db.Entry(qBussinessPlans).State = EntityState.Modified;
            db.SaveChanges();
        }

        private string SaveNewFile(HttpPostedFileBase file, string oldFileName, string path)
        {
            if (file == null)
                return oldFileName;

            if (oldFileName != null)
                System.IO.File.Delete(Server.MapPath(path + oldFileName));

            string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            file.SaveAs(Server.MapPath(path + newFileName));
            return newFileName;
        }

        private string SaveNewImage(HttpPostedFileBase image, string oldImageName, string path)
        {
            if (image == null || image.IsImage() == false)
                return oldImageName;

            if (oldImageName != null && oldImageName != "no-photo.jpg")
                System.IO.File.Delete(Server.MapPath(path + oldImageName));

            string newImageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            image.SaveAs(Server.MapPath(path + newImageName));
            return newImageName;
        }

        private string SaveNewImage(HttpPostedFileBase image, string oldImageName, string path, string thumbpath, int thumbSize = 500)
        {
            if (image == null || image.IsImage() == false)
                return oldImageName;

            if (oldImageName != null && oldImageName != "no-photo.jpg")
            {
                System.IO.File.Delete(Server.MapPath(path + oldImageName));
                System.IO.File.Delete(Server.MapPath(thumbpath + oldImageName));
            }
            string newImageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            image.SaveAs(Server.MapPath(path + newImageName));

            ImageResizer img = new ImageResizer(thumbSize);
            img.Resize(Server.MapPath(path + newImageName),
                Server.MapPath(thumbpath + newImageName));

            return newImageName;
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
