using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using Hamafarin;
using ViewModels;

namespace HamAfarin.Areas.UserPanel.Controllers
{
    public class UserCommentBusinessPlanController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();
        UserService userService = new UserService();
        PlanService planService = new PlanService();

        // GET: UserPanel/UserCommentBusinessPlan
        /// <summary>
        /// نظرهای کاربر کاربر در طرح ها
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            // Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.MobileNumber == User.Identity.Name);
            int UserID = UserSetAuthCookie.GetUserID(User.Identity.Name);
            List<Tbl_CommentPlan> qlstBusinessPlanComments = db.Tbl_CommentPlan.Where(p => p.IsDeleted == false && p.User_id == UserID).OrderBy(p => p.BusinessPlan_id).OrderByDescending(p => p.CreateDate).ToList();
            List<UserCommentBusinessPlanList> lstUserCommentBusinessPlan = new List<UserCommentBusinessPlanList>();

            int Row_id = 1;
            foreach (var item in qlstBusinessPlanComments)
            {

                string strBusinessPlanStatus = "درحال تامین سرمایه";
                int qRemainingDay = planService.calculateRemainDay(item.Tbl_BussinessPlans);
                int qPercentageComplate = planService.GetPercentage(long.Parse(item.Tbl_BussinessPlans.AmountRequiredRoRaiseCapital),
                    planService.GetRaisedPrice(db, item.Tbl_BussinessPlans.BussinessPlanID));

                if (qRemainingDay > 0)
                {
                    strBusinessPlanStatus = "درحال تامین سرمایه";
                }
                else if (qRemainingDay <= 0 && qPercentageComplate >= 100)
                {
                    strBusinessPlanStatus = "تکمیل سرمایه";
                }
                else if (qRemainingDay <= 0 && qPercentageComplate < 100)
                {
                    strBusinessPlanStatus = "عدم تامین سرمایه";
                }
                else if (qRemainingDay <= 0 && qPercentageComplate >= 100)
                {
                    strBusinessPlanStatus = "شروع طرح";
                }
                else if (item.Tbl_BussinessPlans.IsSuccessBussinessPlan)
                {
                    strBusinessPlanStatus = "پایان طرح";
                }
                string strStatusComment = "فعال";
                if (item.IsActive == false)
                    strStatusComment = "غیرفعال";
                string strSubComment = item.CommentText;
                if (strSubComment.Length>10)
                    strSubComment = strSubComment.Substring(0,10)+" ...";

                lstUserCommentBusinessPlan.Add(new UserCommentBusinessPlanList()
                {
                    CommentBusiness_id = item.CommentID,
                    Row_id = Row_id,
                    BusinessPlanName = item.Tbl_BussinessPlans.Title,
                    BusinessPlanStatus = strBusinessPlanStatus,
                    CompanyName = item.Tbl_BussinessPlans.CompanyName,
                    BusinessCommentStatus = strStatusComment,
                    CreateDate = item.CreateDate.Value,
                    CommentText = strSubComment
                });
                Row_id++;
            }

            return View(lstUserCommentBusinessPlan);
        }

        // GET: UserPanel/UserCommentBusinessPlan/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_CommentPlan tbl_CommentPlan = db.Tbl_CommentPlan.Find(id);
            if (tbl_CommentPlan == null)
            {
                return HttpNotFound();
            }
            return View(tbl_CommentPlan);
        }

        // GET: UserPanel/UserCommentBusinessPlan/Create
        public ActionResult Create()
        {
            ViewBag.Parent_id = new SelectList(db.Tbl_CommentPlan, "CommentID", "CommentText");
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName");
            ViewBag.BusinessPlan_id = new SelectList(db.Tbl_BussinessPlans, "BussinessPlanID", "Title");
            return View();
        }

        // POST: UserPanel/UserCommentBusinessPlan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CommentID,IsActive,IsDeleted,Parent_id,BusinessPlan_id,User_id,CreateDate,CommentText")] Tbl_CommentPlan tbl_CommentPlan)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_CommentPlan.Add(tbl_CommentPlan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Parent_id = new SelectList(db.Tbl_CommentPlan, "CommentID", "CommentText", tbl_CommentPlan.Parent_id);
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_CommentPlan.User_id);
            ViewBag.BusinessPlan_id = new SelectList(db.Tbl_BussinessPlans, "BussinessPlanID", "Title", tbl_CommentPlan.BusinessPlan_id);
            return View(tbl_CommentPlan);
        }

        // GET: UserPanel/UserCommentBusinessPlan/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_CommentPlan tbl_CommentPlan = db.Tbl_CommentPlan.Find(id);
            if (tbl_CommentPlan == null)
            {
                return HttpNotFound();
            }
            ViewBag.Parent_id = new SelectList(db.Tbl_CommentPlan, "CommentID", "CommentText", tbl_CommentPlan.Parent_id);
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_CommentPlan.User_id);
            ViewBag.BusinessPlan_id = new SelectList(db.Tbl_BussinessPlans, "BussinessPlanID", "Title", tbl_CommentPlan.BusinessPlan_id);
            return View(tbl_CommentPlan);
        }

        // POST: UserPanel/UserCommentBusinessPlan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommentID,IsActive,IsDeleted,Parent_id,BusinessPlan_id,User_id,CreateDate,CommentText")] Tbl_CommentPlan tbl_CommentPlan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_CommentPlan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Parent_id = new SelectList(db.Tbl_CommentPlan, "CommentID", "CommentText", tbl_CommentPlan.Parent_id);
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_CommentPlan.User_id);
            ViewBag.BusinessPlan_id = new SelectList(db.Tbl_BussinessPlans, "BussinessPlanID", "Title", tbl_CommentPlan.BusinessPlan_id);
            return View(tbl_CommentPlan);
        }

        /// <summary>
        /// حذف نظر
        /// </summary>
        /// <param name="id">شناسه نظر</param>
        public void DeleteComment(int id)
        {
            Tbl_CommentPlan qComment = db.Tbl_CommentPlan.Find(id);
            if (qComment.User_id == UserSetAuthCookie.GetUserID(User.Identity.Name))
            {
                qComment.IsDeleted = true;
                qComment.IsActive = false;
                db.SaveChanges();
            }
        }

        // GET: UserPanel/UserCommentBusinessPlan/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_CommentPlan tbl_CommentPlan = db.Tbl_CommentPlan.Find(id);
            if (tbl_CommentPlan == null)
            {
                return HttpNotFound();
            }
            return View(tbl_CommentPlan);
        }

        // POST: UserPanel/UserCommentBusinessPlan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_CommentPlan tbl_CommentPlan = db.Tbl_CommentPlan.Find(id);
            db.Tbl_CommentPlan.Remove(tbl_CommentPlan);
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
    }
}
