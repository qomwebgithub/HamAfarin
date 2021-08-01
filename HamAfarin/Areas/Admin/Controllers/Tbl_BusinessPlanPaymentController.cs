﻿using System;
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
using Common;
using DataLayer;
using Hamafarin;
using InsertShowImage;
using KooyWebApp_MVC.Classes;
using ViewModels;

namespace HamAfarin.Areas.Admin.Controllers
{
    public class Tbl_BusinessPlanPaymentController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();
        UserService userService = new UserService();
        SMS oSms = new SMS();

        // GET: Admin/Tbl_BusinessPlanPayment
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                var tbl_BusinessPlanPayment = db.Tbl_BusinessPlanPayment.Include(t => t.Tbl_PaymentType).Include(t => t.Tbl_Users).Include(t => t.Tbl_Users1).Include(t => t.Tbl_BussinessPlans);
                return View(tbl_BusinessPlanPayment.OrderByDescending(c => c.PaidDateTime).ToList());
            }
            else
            {
                var tbl_BusinessPlanPayment = db.Tbl_BusinessPlanPayment.Where(p => p.BusinessPlan_id == id).Include(t => t.Tbl_PaymentType).Include(t => t.Tbl_Users).Include(t => t.Tbl_Users1).Include(t => t.Tbl_BussinessPlans);
                return View(tbl_BusinessPlanPayment.OrderByDescending(c => c.PaidDateTime).ToList());
            }
        }


        // GET: Admin/Tbl_BusinessPlanPayment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_BusinessPlanPayment tbl_BusinessPlanPayment = db.Tbl_BusinessPlanPayment.Find(id);
            if (tbl_BusinessPlanPayment == null)
            {
                return HttpNotFound();
            }
            return View(tbl_BusinessPlanPayment);
        }

        // GET: Admin/Tbl_BusinessPlanPayment/Create
        public ActionResult Create()
        {
            createDefaultValues();
            return View();
        }

        private void createDefaultValues()
        {
            ViewBag.PaymentType_id = new SelectList(db.Tbl_PaymentType, "PaymentTypeID", "PaymentTitle");
            ViewBag.CreateUser_id = new SelectList(db.Tbl_Users, "UserID", "UserName");
            ViewBag.PaymentUser_id = new SelectList(db.Tbl_Users, "UserID", "UserName");
            ViewBag.BusinessPlan_id = new SelectList(db.Tbl_BussinessPlans, "BussinessPlanID", "Title");
            List<PaymentStatusViewModel> statusList = new List<PaymentStatusViewModel>()
            {
                new PaymentStatusViewModel(){key = 1,value= "موفق"},
                new PaymentStatusViewModel(){key = 2,value= "در انتظار"},
                new PaymentStatusViewModel(){key = 3,value= "برگشت خورده"},
                new PaymentStatusViewModel(){key = 0,value= "انصراف"}
            };
            ViewBag.PaymentStatus_id = new SelectList(statusList, "key", "value", 1);
        }

        // POST: Admin/Tbl_BusinessPlanPayment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AdminCreateEditPaymentViewModel createPaymentModel,HttpPostedFileBase imgPaymentImageNameUploaded)
        {
            if (imgPaymentImageNameUploaded != null && imgPaymentImageNameUploaded.IsImage())
            {
                createPaymentModel.PaymentImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgPaymentImageNameUploaded.FileName);
                imgPaymentImageNameUploaded.SaveAs(Server.MapPath("/Images/PaymentImages/" + createPaymentModel.PaymentImageName));

            }
            else
            {
                createDefaultValues();
                ModelState.AddModelError("PaymentImageName", "رسید بانکی را انتخاب کنید");
                return View(createPaymentModel);
            }
            if (string.IsNullOrEmpty(createPaymentModel.strPaidDateTime) == false)
            {
                createPaymentModel.PaidDateTime = StringExtensions.StringToDate(createPaymentModel.strPaidDateTime);
                ModelState.Remove("PaidDateTime");
            }
            if (string.IsNullOrEmpty(createPaymentModel.strAdminCheckDate) == false)
            {
                createPaymentModel.AdminCheckDate = StringExtensions.StringToDate(createPaymentModel.strAdminCheckDate);
            }

            if (ModelState.IsValid)
            {
                Tbl_BusinessPlanPayment tbl_BusinessPlanPayment = new Tbl_BusinessPlanPayment()
                {
                    IsDelete = false,
                    IsPaid = createPaymentModel.IsPaid,
                    IsConfirmedFromAdmin = false,
                    //IsConfirmedFromAdmin = createPaymentModel.IsConfirmedFromAdmin,
                    IsReturned = false,
                    BusinessPlan_id = createPaymentModel.BusinessPlan_id,
                    InvoiceNumber = createPaymentModel.InvoiceNumber,
                    TransactionPaymentCode = createPaymentModel.TransactionPaymentCode,
                    PaidDateTime = createPaymentModel.PaidDateTime,
                    CreateDate = DateTime.Now,
                    CreateUser_id = UserSetAuthCookie.GetUserID(User.Identity.Name),
                    PaymentUser_id = createPaymentModel.PaymentUser_id,
                    PaymentPrice = createPaymentModel.PaymentPrice,
                    PaymentType_id = 3,
                    PaymentImageName = createPaymentModel.PaymentImageName,
                    PaymentStatus = createPaymentModel.PaymentStatus,
                    AdminCheckDate = createPaymentModel.AdminCheckDate,
                };
                db.Tbl_BusinessPlanPayment.Add(tbl_BusinessPlanPayment);
                db.SaveChanges();
                return RedirectToAction("Details/" + tbl_BusinessPlanPayment.PaymentID);
            }

            ViewBag.PaymentType_id = new SelectList(db.Tbl_PaymentType, "PaymentTypeID", "PaymentTitle", createPaymentModel.PaymentType_id);
            ViewBag.CreateUser_id = new SelectList(db.Tbl_Users, "UserID", "UserName", createPaymentModel.CreateUser_id);
            ViewBag.PaymentUser_id = new SelectList(db.Tbl_Users, "UserID", "UserName", createPaymentModel.PaymentUser_id);
            ViewBag.BusinessPlan_id = new SelectList(db.Tbl_BussinessPlans, "BussinessPlanID", "Title", createPaymentModel.BusinessPlan_id);
            List<PaymentStatusViewModel> statusList = new List<PaymentStatusViewModel>()
            {
                new PaymentStatusViewModel(){key = 1,value= "موفق"},
                new PaymentStatusViewModel(){key = 2,value= "در انتظار"},
                new PaymentStatusViewModel(){key = 3,value= "برگشت خورده"},
                new PaymentStatusViewModel(){key = 0,value= "انصراف"}
            };
            ViewBag.PaymentStatus_id = new SelectList(statusList, "key", "value", createPaymentModel.PaymentStatus);

            return View(createPaymentModel);
        }

        // GET: Admin/Tbl_BusinessPlanPayment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_BusinessPlanPayment tbl_BusinessPlanPayment = db.Tbl_BusinessPlanPayment.Find(id);
            if (tbl_BusinessPlanPayment == null)
            {
                return HttpNotFound();
            }
            ViewBag.PaymentType_id = new SelectList(db.Tbl_PaymentType, "PaymentTypeID", "PaymentTitle", tbl_BusinessPlanPayment.PaymentType_id);
            ViewBag.CreateUser_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_BusinessPlanPayment.CreateUser_id);
            ViewBag.PaymentUser_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_BusinessPlanPayment.PaymentUser_id);
            ViewBag.BusinessPlan_id = new SelectList(db.Tbl_BussinessPlans, "BussinessPlanID", "Title", tbl_BusinessPlanPayment.BusinessPlan_id);

            //مپ کردن مدل پرداخت کاربری به مدل
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Tbl_BusinessPlanPayment, AdminCreateEditPaymentViewModel>();
            });
            IMapper iMapper = config.CreateMapper();
            //var adminCreateEditPayment = iMapper.Map<Tbl_BusinessPlanPayment, AdminCreateEditPaymentViewModel>(tbl_BusinessPlanPayment);
            var adminCreateEditPayment = new AdminCreateEditPaymentViewModel()
            {
                PaymentID = tbl_BusinessPlanPayment.PaymentID,
                BusinessPlan_id = tbl_BusinessPlanPayment.BusinessPlan_id,
                IsPaid = tbl_BusinessPlanPayment.IsPaid,
                IsConfirmedFromAdmin = tbl_BusinessPlanPayment.IsConfirmedFromAdmin,
                PaidDateTime = tbl_BusinessPlanPayment.PaidDateTime,
                InvoiceNumber = tbl_BusinessPlanPayment.InvoiceNumber,
                CreateDate = tbl_BusinessPlanPayment.CreateDate,
                CreateUser_id = tbl_BusinessPlanPayment.CreateUser_id,
                PaymentUser_id = tbl_BusinessPlanPayment.PaymentUser_id,
                TransactionPaymentCode = tbl_BusinessPlanPayment.TransactionPaymentCode,
                PaymentPrice = tbl_BusinessPlanPayment.PaymentPrice,
                PaymentType_id = tbl_BusinessPlanPayment.PaymentType_id.Value,
                PaymentImageName = tbl_BusinessPlanPayment.PaymentImageName,
            };
            if (tbl_BusinessPlanPayment.PaymentType_id == 2)
            {
                Tbl_PaymentOnlineDetils qOnlineDetails = db.Tbl_PaymentOnlineDetils.FirstOrDefault(d => d.Payment_id == tbl_BusinessPlanPayment.PaymentID);
                AdminOnlineDetilsPaymentViewModel adminOnlineDetails = new AdminOnlineDetilsPaymentViewModel()
                {
                    PaymentDetilsID = qOnlineDetails.PaymentDetilsID,
                    IsFinally = qOnlineDetails.IsFinally,
                    ShaparakToken = qOnlineDetails.ShaparakToken,
                    TransactionReferenceID = qOnlineDetails.TransactionReferenceID,
                    ShaparakCheckTransactionResult = qOnlineDetails.ShaparakCheckTransactionResult,
                    ShaparakVerifyPayment = qOnlineDetails.ShaparakVerifyPayment,
                    CreateDate = qOnlineDetails.CreateDate,
                    FinallyDate = qOnlineDetails.FinallyDate,
                };
                adminCreateEditPayment.OnlineDetails = adminOnlineDetails;
            }

            (string Date, string Time) dateTime = DateTimeFormating(tbl_BusinessPlanPayment.PaidDateTime);
            ViewBag.PaymentDate = dateTime.Date;
            ViewBag.PaymentTime = dateTime.Time;

            return View(adminCreateEditPayment);
        }

        // POST: Admin/Tbl_BusinessPlanPayment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tbl_BusinessPlanPayment tbl_BusinessPlanPayment)
        {
            if (ModelState.IsValid)
            {
                if (tbl_BusinessPlanPayment.IsConfirmedFromAdmin)
                {
                    tbl_BusinessPlanPayment.PaymentStatus = (int)PaymentStatusType.SUCCESS;
                    tbl_BusinessPlanPayment.AdminCheckDate = DateTime.Now;


                    // 5 = تایید سرمایه گذاری توسط ادمین
                    Tbl_Sms qSms = db.Tbl_Sms.Find(5);
                    string massage = qSms.Message;
                    if (qSms.Message.Contains("@T"))
                    {
                        Tbl_BussinessPlans qBussinessPlan = db.Tbl_BussinessPlans.FirstOrDefault(b => b.BussinessPlanID == tbl_BusinessPlanPayment.BusinessPlan_id);
                        massage = qSms.Message.Replace("@T", qBussinessPlan.Title);
                    }

                    Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.UserID == tbl_BusinessPlanPayment.Tbl_BussinessPlans.User_id);
                    (bool Success, string Message) result = oSms.AdpSendSms(qUser.MobileNumber, massage);
                }
                db.Entry(tbl_BusinessPlanPayment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PaymentType_id = new SelectList(db.Tbl_PaymentType, "PaymentTypeID", "PaymentTitle", tbl_BusinessPlanPayment.PaymentType_id);
            ViewBag.CreateUser_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_BusinessPlanPayment.CreateUser_id);
            ViewBag.PaymentUser_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_BusinessPlanPayment.PaymentUser_id);
            ViewBag.BusinessPlan_id = new SelectList(db.Tbl_BussinessPlans, "BussinessPlanID", "Title", tbl_BusinessPlanPayment.BusinessPlan_id);

            return View(tbl_BusinessPlanPayment);
        }

        // GET: Admin/Tbl_BusinessPlanPayment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_BusinessPlanPayment tbl_BusinessPlanPayment = db.Tbl_BusinessPlanPayment.Find(id);
            if (tbl_BusinessPlanPayment == null)
            {
                return HttpNotFound();
            }
            return View(tbl_BusinessPlanPayment);
        }

        // POST: Admin/Tbl_BusinessPlanPayment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_BusinessPlanPayment tbl_BusinessPlanPayment = db.Tbl_BusinessPlanPayment.Find(id);
            tbl_BusinessPlanPayment.IsDelete = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: Admin/Tbl_BusinessPlanPayment/SubmittedPayments
        public ActionResult SubmittedPayments()
        {
            var tbl_BusinessPlanPayment = db.Tbl_BusinessPlanPayment.Where(p => p.IsPaid && p.IsConfirmedFromAdmin)
                .Include(t => t.Tbl_PaymentType).Include(t => t.Tbl_Users).Include(t => t.Tbl_Users1).Include(t => t.Tbl_BussinessPlans);
            return View(tbl_BusinessPlanPayment.ToList());
        }
        /// <summary>
        /// سرمایه گذاری های تایید نشده
        /// </summary>
        /// <returns></returns>
        // GET: Admin/Tbl_BusinessPlanPayment/UnSubmittedPayments
        public ActionResult UnSubmittedPayments()
        {
            var tbl_BusinessPlanPayment = db.Tbl_BusinessPlanPayment.Where(p => p.IsPaid && p.IsConfirmedFromAdmin == false)
                .Include(t => t.Tbl_PaymentType).Include(t => t.Tbl_Users).Include(t => t.Tbl_Users1).Include(t => t.Tbl_BussinessPlans);
            return View(tbl_BusinessPlanPayment.ToList());
        }

        // GET: Admin/Tbl_BusinessPlanPayment/DraftsPayments
        public ActionResult DraftsPayments()
        {
            var tbl_BusinessPlanPayment = db.Tbl_BusinessPlanPayment.Where(p => p.IsPaid == false)
                .Include(t => t.Tbl_PaymentType).Include(t => t.Tbl_Users).Include(t => t.Tbl_Users1).Include(t => t.Tbl_BussinessPlans);
            return View(tbl_BusinessPlanPayment.ToList());
        }

        [HttpPost]
        public async Task<ActionResult> ConfirmedFromAdmin(int id, string payDate)
        {
            Tbl_BusinessPlanPayment tbl_BusinessPlanPayment = await db.Tbl_BusinessPlanPayment.FindAsync(id);
            tbl_BusinessPlanPayment.IsConfirmedFromAdmin = true;
            db.Entry(tbl_BusinessPlanPayment).State = EntityState.Modified;
            await db.SaveChangesAsync();
            FaraboorsClass faraboors = new FaraboorsClass();
            (bool Success, string Message) result = await faraboors.ProjectFinancingProviderAsync(id, payDate);

            return Json(new { success = result.Success, message = result.Message });
        }

        private (string date,string time) DateTimeFormating(DateTime? dateTime)
        {
            (string Date, string Time) dateTimeFixed;

            string[] lstPaidDateTime = dateTime.ToString().Split(' ');

            dateTimeFixed.Date = lstPaidDateTime[0];

            if (lstPaidDateTime[2].Contains("ب.ظ"))
            {
                string[] lstTime = lstPaidDateTime[1].Split(':');

                List<int> lstNumbers = new List<int>();
                foreach (string num in lstTime)
                {
                    lstNumbers.Add(int.Parse(num));
                }

                TimeSpan time = new TimeSpan(lstNumbers[0], lstNumbers[1], lstNumbers[2]);
                time = time.Add(new TimeSpan(12, 0, 0));

                dateTimeFixed.Time = time.ToString();

            }
            else
            {
                dateTimeFixed.Time = lstPaidDateTime[2];
            }

            return dateTimeFixed;
        }
    }
}
