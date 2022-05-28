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
using Common;
using DataLayer;
using Hamafarin;
using InsertShowImage;
using KooyWebApp_MVC.Classes;
using ViewModels;

namespace HamAfarin.Areas.Admin.Controllers
{
    public class Tbl_SmsController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();
        SMS oSms = new SMS();

        // GET: Admin/Tbl_Sms
        public async Task<ActionResult> Index()
        {
            List<Tbl_Sms> tbl_Sms = await db.Tbl_Sms.OrderByDescending(s => s.ID).ToListAsync();
            return View(tbl_Sms);
        }

        // GET: Admin/Tbl_Sms/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Sms tbl_Sms = await db.Tbl_Sms.FindAsync(id);
            if (tbl_Sms == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Sms);
        }

        // Post: Admin/Tbl_Sms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Tbl_Sms tbl_Sms)
        {
            if (ModelState.IsValid)
            {
                tbl_Sms.Message = tbl_Sms.Message.ToUpper();
                tbl_Sms.EditDate = DateTime.Now;
                db.Entry(tbl_Sms).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tbl_Sms);
        }

        public ActionResult PartialSendSms()
        {
            string Message = "";
            Tbl_Sms qSmsNewPlan = db.Tbl_Sms.FirstOrDefault(p => p.ID == 8);
            if (qSmsNewPlan != null)
            {
                Message = qSmsNewPlan.Message;
            }
            return PartialView(new AdminSendSmsviewModel()
            {
                Message = Message
            });
        }

        /// <summary>
        /// ارسال اس ام اس به کاربران
        /// </summary>
        /// <param name="MobileNumber">شماره موبایل کاربر (اختیاری است) اگر وارد نکرده بود به همه ی کاربران مورد نظر اس ام اس ارسال میشود</param>
        /// <param name="Message">متن پیام</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SendSms(AdminSendSmsviewModel sendSms)
        {
            try
            {
                // اگر متن پیام خالی بود پیام ارسال نمیکنیم
                if (string.IsNullOrEmpty(sendSms.Message) == false)
                {
                    SendSmsAsync(sendSms);
                }

            }
            catch (Exception e)
            {
                //return Json(new { success = false }, JsonRequestBehavior.AllowGet);

            }
            return Redirect("/Admin/Tbl_Users");
        }

        public async Task SendSmsAsync(AdminSendSmsviewModel sendSms)
        {
            await Task.Run(() =>
            {

                // اگر متن پیام خالی بود پیام ارسال نمیکنیم
                if (string.IsNullOrEmpty(sendSms.Message) == false)
                {
                    // اگر شماره موبایل پر شده بود فقط به همان شماره پیام میفرستیم در غیر اینصورت به همه ی کاربران موردنظر اس ام اس ارسال میکنیم
                    if (string.IsNullOrEmpty(sendSms.MobileNumber) == false)
                    {
                        sendSms.MobileNumber = StringExtensions.Fa2En(sendSms.MobileNumber);
                        (bool Success, string Message) result = oSms.SendSms(sendSms.MobileNumber, sendSms.Message);
                    }
                    else
                    {
                        string common = "";
                        List<Tbl_Users> qlstProducts = db.Tbl_Users.Where(p => p.IsDeleted == false && p.IsActive).ToList();
                        foreach (var item in qlstProducts)
                        {
                            sendSms.MobileNumber += common + item.MobileNumber;
                            common = ",";
                        }

                        (bool Success, string Message) result = oSms.SendSms(sendSms.MobileNumber, sendSms.Message);
                        //// شماره موبایل ها را در لیستی از استرینگ ذخیره میکنیم
                        //foreach (var item in lstGetOrdersForSendSms)
                        //{
                        //    lsttMobileNumber.Add(StringExtensions.Fa2En(item.Mobile));
                        //}
                        //if (lsttMobileNumber != null)
                        //{
                        //    if (lsttMobileNumber.Count > 0)
                        //    {
                        //        oSms.SendSMS(lsttMobileNumber, sendSms.Message);
                        //        // ذخیره اس ام اس ارسالی
                        //        foreach (var item in lsttMobileNumber)
                        //        {
                        //            Tbl_SendSmsForCustomers tbl_SendSms = new Tbl_SendSmsForCustomers()
                        //            {
                        //                ID = Guid.NewGuid().ToString(),
                        //                CreateDate = DateTime.Now,
                        //                IsSend = true,
                        //                Message = sendSms.Message,
                        //                MobileNumber = item
                        //            };
                        //            db.Tbl_SendSmsForCustomers.Add(tbl_SendSms);
                        //        }
                        //        db.SaveChanges();
                        //    }

                        //}
                    }
                    //return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }


            });
        }
    }
}