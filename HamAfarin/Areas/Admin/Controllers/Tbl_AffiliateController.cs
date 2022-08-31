﻿using DataLayer;
using System.Linq;
using System.Web.Mvc;
using ViewModels;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System;

namespace HamAfarin.Areas.Admin.Controllers
{
    public class Tbl_AffiliateController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: UserPanel/DepositToInvestors
        public async Task<ActionResult> Index()
        {
            var fullJoin = await
                (from api in db.Tbl_ApiToken.Where(d=>d.IsDelete ==false)
                 join userProfiles in db.Tbl_UserProfiles on api.User_Id equals userProfiles.User_id into UserProfileGroup
                 from upg in UserProfileGroup.DefaultIfEmpty()
                 join affiliate in db.Tbl_Affiliate on api.ID equals affiliate.Token_Id into AffiliateGroup
                 from ag in AffiliateGroup.DefaultIfEmpty()
                 join businessPlanPayment in db.Tbl_BusinessPlanPayment
                     on ag.User_Id equals businessPlanPayment.PaymentUser_id into PaymentGroup
                 from pg in PaymentGroup.Where(g => g.IsConfirmedFromFaraboors && g.IsDelete == false).DefaultIfEmpty()
                 select new
                 {
                     ID = api.ID,
                     User_Id = api.User_Id,
                     Mobile = upg.MobileNumber,
                     Username = upg.Tbl_Users.UserName,
                     Name = api.Name,
                     Url = api.Url,
                     PaymentPrice = pg.PaymentPrice ?? 0,
                 }).ToListAsync();

            var groupInvest =
                from a in fullJoin
                group a.PaymentPrice by a.ID into g
                select new ApiTokenViewModel
                {
                    ID = g.Key,
                    TotalInvestment = g.Sum(),
                };

            var groupUser =
                from a in fullJoin
                group a.User_Id by new { a.ID, a.Mobile, a.Username, a.Name, a.Url } into g
                select new ApiTokenViewModel
                {
                    ID = g.Key.ID,
                    Mobile = g.Key.Mobile,
                    Username = g.Key.Username,
                    Name = g.Key.Name,
                    Url = g.Key.Url,
                    UserCount = g.Count(),
                };

            var apiTokenVM =
                from u in groupUser
                join i in groupInvest on u.ID equals i.ID
                select new ApiTokenViewModel
                {
                    ID = u.ID,
                    Mobile = u.Mobile,
                    Username = u.Username,
                    Name = u.Name,
                    Url = u.Url,
                    UserCount = u.UserCount,
                    TotalInvestment = i.TotalInvestment,
                };

            return View(apiTokenVM.ToList());
        }

        public async Task<ActionResult> create()
        {
            ApiTokenViewModel apiTokenViewModel = new ApiTokenViewModel();
            ViewBag.User_ID = new SelectList(db.Tbl_Users, "UserID", "UserName");

            return View(apiTokenViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> create(ApiTokenViewModel apiTokenViewModel)
        {
            ViewBag.User_ID = new SelectList(db.Tbl_Users, "UserID", "UserName",apiTokenViewModel.UserID);
            if (ModelState.IsValid)
            {
                Tbl_ApiToken tbl_ApiToken = new Tbl_ApiToken();

                tbl_ApiToken.Url = apiTokenViewModel.Url;
                tbl_ApiToken.Name = apiTokenViewModel.Name;
                tbl_ApiToken.User_Id = apiTokenViewModel.UserID;
                tbl_ApiToken.Token = Guid.NewGuid().ToString();
                var bytes = UTF8Encoding.UTF8.GetBytes(tbl_ApiToken.Token);
                var shaM = new HMACSHA512();
                tbl_ApiToken.TokenHash = Convert.ToBase64String(shaM.ComputeHash(bytes));


                db.Tbl_ApiToken.Add(tbl_ApiToken);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View();
            }
            ApiTokenViewModel apiTokenViewModel = new ApiTokenViewModel();
            var qTbl_ApiToken = await db.Tbl_ApiToken.AsNoTracking().Where(i => i.ID == id).FirstOrDefaultAsync();
            if (qTbl_ApiToken != null)
            {
                apiTokenViewModel.ID = qTbl_ApiToken.ID;
                apiTokenViewModel.Name = qTbl_ApiToken.Name;
                apiTokenViewModel.Url = qTbl_ApiToken.Url;
                apiTokenViewModel.UserID = qTbl_ApiToken.User_Id;
                ViewBag.User_ID = new SelectList(db.Tbl_Users, "UserID", "UserName", qTbl_ApiToken.User_Id);
            }
            return View(apiTokenViewModel);
        }
        
        [HttpPost]
        public async Task<ActionResult> Edit(ApiTokenViewModel apiTokenViewModel)
        {
            if (ModelState.IsValid)
            {
                var qTbl_ApiToken = await db.Tbl_ApiToken.Where(i => i.ID == apiTokenViewModel.ID).FirstOrDefaultAsync();
                if(qTbl_ApiToken != null)
                {
                    qTbl_ApiToken.Name = apiTokenViewModel.Name;
                    qTbl_ApiToken.Url = apiTokenViewModel.Url;
                    qTbl_ApiToken.User_Id = apiTokenViewModel.UserID;
                    db.SaveChanges();

                }
                return RedirectToAction("Index");
            }
            return View();
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_ApiToken tbl_ApiToken = db.Tbl_ApiToken.Find(id);
            if (tbl_ApiToken == null)
            {
                return HttpNotFound();
            }
            return View(tbl_ApiToken);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Tbl_ApiToken tbl_ApiToken = db.Tbl_ApiToken.Find(id);
            tbl_ApiToken.IsDelete = true;
            db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}