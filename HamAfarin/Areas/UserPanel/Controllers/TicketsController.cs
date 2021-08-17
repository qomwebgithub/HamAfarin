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

namespace HamAfarin.Areas.UserPanel
{
    public class TicketsController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: Admin/Tbl_Tickets
        public ActionResult Index()
        {
            int userId = UserSetAuthCookie.GetUserID(User.Identity.Name);
            var tbl_Tickets = db.Tbl_Tickets.Where(t => t.IsDelete != true && t.User_id == userId && t.Parent_id == null).Include(t => t.Tbl_Tickets2).Include(t => t.Tbl_Users).Include(t => t.Tbl_Users1).Include(t => t.Tbl_Users2).Include(t => t.Tbl_Users3).Include(t => t.Tbl_Users4);
            return View(tbl_Tickets.ToList());
        }

        /// <summary>
        /// تیکت های چک نشده کاربر
        /// </summary>
        /// <returns></returns>
        public ActionResult UnCheckedTickets()
        {
            int userId = UserSetAuthCookie.GetUserID(User.Identity.Name);

            List<Tbl_Tickets> qlstTickets = db.Tbl_Tickets
                .Where(t => t.IsDelete != true
                && t.IsUserChecked != true
                ).OrderByDescending(q => q.TicketID).ToList();

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

            if (qTicket == null || qTicket.User_id != UserSetAuthCookie.GetUserID(User.Identity.Name))
            {
                return HttpNotFound();
            }

            //جزئیات تیکت
            List<Tbl_Tickets> qlstTickets = new List<Tbl_Tickets>();
            qlstTickets.Add(qTicket);
            qlstTickets.AddRange(db.Tbl_Tickets.Where(t => t.Parent_id == id));

            return View(qlstTickets.OrderBy(q => q.CreateDateTime).ToList());
        }

        /// <summary>
        /// چک شدن تیکت توسط کاربر
        /// </summary>
        /// <param name="id">آیدی تیکت</param>
        /// <returns></returns>
        public ActionResult TicketUserChecked(int id)
        {
            Tbl_Tickets qtbl_Tickets = db.Tbl_Tickets.Find(id);

            if (qtbl_Tickets.User_id == UserSetAuthCookie.GetUserID(User.Identity.Name))
            {
                qtbl_Tickets.IsUserChecked = true;
                qtbl_Tickets.UserCheckedDateTime = DateTime.Now;

                db.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

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
            int userId = UserSetAuthCookie.GetUserID(User.Identity.Name);




            return View(new Tbl_Tickets { Parent_id = parentId, User_id = userId });
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

                tbl_Tickets.CreateDateTime = DateTime.Now;
                tbl_Tickets.UserCreate_id = UserSetAuthCookie.GetUserID(User.Identity.Name);
                tbl_Tickets.User_id = UserSetAuthCookie.GetUserID(User.Identity.Name);
                tbl_Tickets.IsUserChecked = true;
                tbl_Tickets.UserCheckedDateTime = DateTime.Now;

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


            return View(tbl_Tickets);
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
