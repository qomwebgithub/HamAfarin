using DataLayer;
using HamAfarin.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ViewModels;

namespace HamAfarin.Areas.UserPanel.Controllers
{
    public class WithdrawalController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: UserPanel/Withdrawal
        public ActionResult Index()
        {
            int userId = UserSetAuthCookie.GetUserID(User.Identity.Name);
            var withdrawal = db.Tbl_WalletWithdrawalRequest.Where(u => u.User_Id == userId);
            return View(withdrawal.OrderByDescending(w => w.CreateDate));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tbl_WalletWithdrawalRequest withdrawalRequest)
        {
            if (ModelState.IsValid)
            {
                int userId = UserSetAuthCookie.GetUserID(User.Identity.Name);
                withdrawalRequest.CreateDate = DateTime.Now;
                withdrawalRequest.User_Id = userId;
                withdrawalRequest.WithdrawalStatus = 0;

                db.Tbl_WalletWithdrawalRequest.Add(withdrawalRequest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
                return View(withdrawalRequest);
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