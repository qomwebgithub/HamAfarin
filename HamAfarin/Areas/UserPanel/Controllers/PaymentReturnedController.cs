using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using Hamafarin;
using ViewModels;

namespace HamAfarin.Areas.UserPanel.Controllers
{
    public class PaymentReturnedController : Controller
    {
        HamAfarinDBEntities db = new HamAfarinDBEntities();
        // GET: UserPanel/PaymentReturned
        /// <summary>
        /// درخواست استرداد پرداخت
        /// </summary>
        /// <param name="paymentId">شناسه پرداخت</param>
        /// <param name="planId">شناسه طرح</param>
        /// <returns></returns>
        public ActionResult DoReturnedPayment(int paymentId, int planId)
        {
            Tbl_BusinessPlanPayment qPayment = db.Tbl_BusinessPlanPayment.Find(paymentId);
            PaymentReturnedViewModel paymentReturned = new PaymentReturnedViewModel()
            {
                BusinessPlan_id = planId,
                PaymentId = qPayment.PaymentID,
            };
            return PartialView(paymentReturned);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DoReturnedPayment(PaymentReturnedViewModel paymentReturned)
        {
            var qBusinessPlanPayment = db.Tbl_BusinessPlanPayment.FirstOrDefault(p=> p.PaymentID == paymentReturned.PaymentId);
            if (ModelState.IsValid && qBusinessPlanPayment.PaymentStatus != 0)
            {
                Tbl_PaymentReturned tbl_PaymentReturned = new Tbl_PaymentReturned()
                {
                    BusinessPlan_id = paymentReturned.BusinessPlan_id,
                    Payment_id = paymentReturned.PaymentId,
                    CreateDate = DateTime.Now,
                    User_id = UserSetAuthCookie.GetUserID(User.Identity.Name),
                    ReasonText = paymentReturned.ReasonText,
                    IsActive = true,
                    IsDelete = false,
                    IsConfirm = false,
                };
                qBusinessPlanPayment.PaymentStatus = 0;
                db.Entry(qBusinessPlanPayment).State = EntityState.Modified;
                db.Tbl_PaymentReturned.Add(tbl_PaymentReturned);
                db.SaveChanges();
                return PartialView();

            }
            ViewBag.returnIsSuccess = true;
            return PartialView();
        }
    }
}