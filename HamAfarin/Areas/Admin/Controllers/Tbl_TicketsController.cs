using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using HamAfarin;

namespace Hamafarin.Areas.Admin.Controllers
{
    public class Tbl_TicketsController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: Admin/Tbl_Tickets
        public ActionResult Index()
        {
            var tbl_Tickets = db.Tbl_Tickets
                .Where(t => t.IsDelete != true)
                .Include(t => t.Tbl_Tickets2)
                .Include(t => t.Tbl_Users)
                .Include(t => t.Tbl_Users1)
                .Include(t => t.Tbl_Users2)
                .Include(t => t.Tbl_Users3)
                .Include(t => t.Tbl_Users4)
                .OrderByDescending(t => t.CreateDateTime);
            return View(tbl_Tickets.ToList());
        }

        /// <summary>
        /// تیکت های چک نشده
        /// </summary>
        /// <returns></returns>
        public ActionResult UnCheckedTickets()
        {
            List<Tbl_Tickets> qlstTickets = db.Tbl_Tickets
                .Where(t => t.IsDelete != true
                && t.IsAdminChecked != true).OrderByDescending(q => q.TicketID).ToList();

            return PartialView(qlstTickets);
        }

        /// <summary>
        /// نمایش تیکت
        /// </summary>
        /// <param name="id">آیدی تیکت پدر</param>
        /// <returns></returns>
        public ActionResult TicketDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //تیکت پدر
            Tbl_Tickets qTicket = db.Tbl_Tickets.Find(id);

            if (qTicket == null)
            {
                return HttpNotFound();
            }

            //جزئیات تیکت
            List<Tbl_Tickets> qlstTickets = new List<Tbl_Tickets>();
            qlstTickets.Add(qTicket);
            qlstTickets.AddRange(db.Tbl_Tickets.Where(t => t.Parent_id == id));

            return View(qlstTickets);
        }

        /// <summary>
        /// چک شدن تیکت توسط ادمین
        /// </summary>
        /// <param name="id">آیدی تیکت</param>
        /// <returns></returns>
        public ActionResult TicketAdminChecked(int id)
        {
            Tbl_Tickets qtbl_Tickets = db.Tbl_Tickets.Find(id);
            qtbl_Tickets.IsAdminChecked = true;
            qtbl_Tickets.AdminCheckedDateTime = DateTime.Now;
            qtbl_Tickets.AdminChecked_id = UserSetAuthCookie.GetUserID(User.Identity.Name);
            db.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        // GET: Admin/Tbl_Tickets/Create
        /// <summary>
        /// ایجاد تیکت
        /// </summary>
        /// <param name="id">شناسه ی تیکت پدر</param>
        /// درصورتی که شناسه ی تیکت پدر وارد نشده باشد یک تیکت اصلی ساخته می شود
        /// <returns></returns>
        public ActionResult Create(int? parentId)
        {
            if (parentId != null)
            {
                int quserId = db.Tbl_Tickets.Find(parentId).User_id.Value;
                ViewBag.User = db.Tbl_Users.Find(quserId);
            }

            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "MobileNumber");

            return View(new Tbl_Tickets { Parent_id = parentId });
        }

        // POST: Admin/Tbl_Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Parent_id,User_id,Subject,Text")] Tbl_Tickets tbl_Tickets, HttpPostedFileBase attachTicketFileUpload)
        {
            if (ModelState.IsValid)
            {

                //ذخیره ی فایل آپلود شده
                if (attachTicketFileUpload != null)
                {
                    string strFileUploadName = Guid.NewGuid().ToString() + Path.GetExtension(attachTicketFileUpload.FileName);

                    attachTicketFileUpload.SaveAs(Server.MapPath(FileAddressesDirectoryPath.TicketFileUploadUrl(strFileUploadName)));

                    tbl_Tickets.AttachFileName = strFileUploadName;
                }

                tbl_Tickets.User_id = UserSetAuthCookie.GetUserID(User.Identity.Name);
                tbl_Tickets.CreateDateTime = DateTime.Now;
                tbl_Tickets.UserCreate_id = UserSetAuthCookie.GetUserID(User.Identity.Name);
                tbl_Tickets.AdminCheckedDateTime = DateTime.Now;
                tbl_Tickets.IsAdminChecked = true;
                tbl_Tickets.AdminChecked_id = UserSetAuthCookie.GetUserID(User.Identity.Name);


                //تغییر وضعیت تیکت پدر به باز بودن تیکت در صورتی که جوابی به تیکت داده شد
                if (tbl_Tickets.Parent_id != null)
                {
                    db.Tbl_Tickets.Find(tbl_Tickets.Parent_id).IsClosed = false;
                }


                db.Tbl_Tickets.Add(tbl_Tickets);
                db.SaveChanges();



                //ریدایرکت به نمایش جزئیات تیکت با استفاده از آیدی تیکت پدر
                int? intGoToParentid = tbl_Tickets.Parent_id;
                if (tbl_Tickets.Parent_id == null)//درصورتی که تیکت ایجاد شده تیکت پدر بود
                    intGoToParentid = tbl_Tickets.TicketID;

                return RedirectToAction("TicketDetails", new { id = intGoToParentid });
            }

            ViewBag.Parent_id = new SelectList(db.Tbl_Tickets, "TicketID", "Subject", tbl_Tickets.Parent_id);
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "MobileNumber", tbl_Tickets.User_id);

            return View(tbl_Tickets);
        }

        // GET: Admin/Tbl_Tickets/Edit/5
        public ActionResult Edit(int id)
        {
            Tbl_Tickets tbl_Tickets = db.Tbl_Tickets.Find(id);
            if (tbl_Tickets == null)
            {
                return HttpNotFound();
            }
            ViewBag.Parent_id = new SelectList(db.Tbl_Tickets, "TicketID", "Subject", tbl_Tickets.Parent_id);
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "MobileNumber", tbl_Tickets.User_id);

            return View(tbl_Tickets);
        }

        // POST: Admin/Tbl_Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TicketID,Parent_id,IsClosed,ClosedDateTime,User_id,Subject,Text,IsUserChecked,IsAdminChecked,UserCheckedDateTime,AdminCheckedDateTime,AttachFileName,AdminChecked_id,UserCreate_id,CreateDateTime,EditDateTime,EditUser_id")] Tbl_Tickets tbl_Tickets, HttpPostedFileBase attachTicketFileUpload)
        {
            if (ModelState.IsValid)
            {




                //ذخیره ی فایل آپلود شده
                if (attachTicketFileUpload != null)
                {

                    //حذف فایل قبلی
                    if (tbl_Tickets.AttachFileName != null)
                    {
                        string strLastFilePath = Server.MapPath(FileAddressesDirectoryPath.TicketFileUploadUrl(tbl_Tickets.AttachFileName));

                        if (System.IO.File.Exists(strLastFilePath))
                        {
                            System.IO.File.Delete(strLastFilePath);
                        }
                    }


                    string strFileUploadName = Guid.NewGuid().ToString() + Path.GetExtension(attachTicketFileUpload.FileName);

                    attachTicketFileUpload.SaveAs(Server.MapPath(FileAddressesDirectoryPath.TicketFileUploadUrl(strFileUploadName)));

                    tbl_Tickets.AttachFileName = strFileUploadName;
                }

                tbl_Tickets.EditDateTime = DateTime.Now;
                tbl_Tickets.EditUser_id = UserSetAuthCookie.GetUserID(User.Identity.Name);



                //تغییر وضعیت تیکت پدر به باز بودن تیکت در صورتی که جوابی به تیکت داده شد
                if (tbl_Tickets.Parent_id != null)
                {
                    db.Tbl_Tickets.Find(tbl_Tickets.Parent_id).IsClosed = false;
                }



                db.Entry(tbl_Tickets).State = EntityState.Modified;
                db.SaveChanges();

                //ریدایرکت به نمایش جزئیات تیکت با استفاده از آیدی تیکت پدر
                int? intGoToParentid = tbl_Tickets.Parent_id;
                if (tbl_Tickets.Parent_id == null)//درصورتی که تیکت ایجاد شده تیکت پدر بود
                    intGoToParentid = tbl_Tickets.TicketID;

                return RedirectToAction("TicketDetails", new { id = intGoToParentid });
            }

            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "MobileNumber", tbl_Tickets.User_id);

            return View(tbl_Tickets);
        }

        // GET: Admin/Tbl_Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Tickets tbl_Tickets = db.Tbl_Tickets.Find(id);
            if (tbl_Tickets == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Tickets);
        }

        // POST: Admin/Tbl_Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Tickets tbl_Tickets = db.Tbl_Tickets.Find(id);


            //db.Tbl_Tickets.Remove(tbl_Tickets);

            ///حذف منطقی
            tbl_Tickets.IsDelete = true;
            tbl_Tickets.DeleteDateTime = DateTime.Now;
            tbl_Tickets.DeleteUser_id = UserSetAuthCookie.GetUserID(User.Identity.Name);

            db.SaveChanges();

            //حذف فایل آپلود شده
            if (tbl_Tickets.AttachFileName != null)
            {
                string strLastFilePath = Server.MapPath(FileAddressesDirectoryPath.TicketFileUploadUrl(tbl_Tickets.AttachFileName));

                if (System.IO.File.Exists(strLastFilePath))
                {
                    System.IO.File.Delete(strLastFilePath);
                }
            }


            //ریدایرکت به نمایش جزئیات تیکت با استفاده از آیدی تیکت پدر
            if (tbl_Tickets.Parent_id != null)//درصورتی که تیکت ایجاد شده تیکت پدر بود
                return RedirectToAction("TicketDetails", new { id = tbl_Tickets.Parent_id });



            return RedirectToAction("Index");
        }

        public ActionResult AnswerTicket(int parentId)
        {
            Tbl_Tickets qTicket = db.Tbl_Tickets.FirstOrDefault(t => t.TicketID == parentId);

            ViewBag.User = db.Tbl_Users.FirstOrDefault(u => u.UserID == qTicket.User_id);
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "MobileNumber");
            ViewBag.Subject = qTicket.Subject;
            ViewBag.Text = qTicket.Text;

            return View(new Tbl_Tickets { Parent_id = parentId, Subject = qTicket.Subject });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AnswerTicket([Bind(Include = "Parent_id,User_id,Subject,Text")] Tbl_Tickets tbl_Tickets, HttpPostedFileBase attachTicketFileUpload)
        {

            if (ModelState.IsValid == false)
            {
                ViewBag.Parent_id = new SelectList(db.Tbl_Tickets, "TicketID", "Subject", tbl_Tickets.Parent_id);
                ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "MobileNumber", tbl_Tickets.User_id);

                return View(tbl_Tickets);
            }
            
            //ذخیره ی فایل آپلود شده
            if (attachTicketFileUpload != null)
            {
                string strFileUploadName = Guid.NewGuid().ToString() + Path.GetExtension(attachTicketFileUpload.FileName);

                attachTicketFileUpload.SaveAs(Server.MapPath(FileAddressesDirectoryPath.TicketFileUploadUrl(strFileUploadName)));

                tbl_Tickets.AttachFileName = strFileUploadName;
            }

            tbl_Tickets.CreateDateTime = DateTime.Now;
            tbl_Tickets.UserCreate_id = UserSetAuthCookie.GetUserID(User.Identity.Name);
            tbl_Tickets.AdminCheckedDateTime = DateTime.Now;
            tbl_Tickets.IsAdminChecked = true;
            tbl_Tickets.AdminChecked_id = UserSetAuthCookie.GetUserID(User.Identity.Name);

            Tbl_Tickets qParentTicket = db.Tbl_Tickets.Find(tbl_Tickets.Parent_id);
            tbl_Tickets.User_id = qParentTicket.User_id;
            qParentTicket.IsClosed = false;
            qParentTicket.IsAdminChecked = true;

            db.Tbl_Tickets.Add(tbl_Tickets);
            db.SaveChanges();

            //ریدایرکت به نمایش جزئیات تیکت با استفاده از آیدی تیکت پدر
            int? intGoToParentid = tbl_Tickets.Parent_id;

            return RedirectToAction("TicketDetails", new { id = intGoToParentid });
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
