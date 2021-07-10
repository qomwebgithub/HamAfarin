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

        // GET: Admin/Tbl_Sms
        public async Task<ActionResult> Index()
        {
            List<Tbl_Sms> tbl_Sms = await db.Tbl_Sms.ToListAsync();
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
    }
}