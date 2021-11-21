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
    public class UserQuestionBusinessPlanController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();
        UserService userService = new UserService();
        PlanService planService = new PlanService();

        // GET: UserPanel/UserQuestionBusinessPlan
        public ActionResult Index()
        {
            // Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.MobileNumber == User.Identity.Name);
            int UserID = UserSetAuthCookie.GetUserID(User.Identity.Name);
            List<Tbl_BusinessPlanQuestion> qlstBusinessPlanQuestions = db.Tbl_BusinessPlanQuestion.Where(p => p.IsDeleted == false && p.User_id == UserID).OrderBy(p => p.BusinessPlan_id).OrderByDescending(p => p.CreateDate).ToList();
            List<UserQuestionBusinessPlanList> lstUserQuestionBusinessPlan = new List<UserQuestionBusinessPlanList>();

            int Row_id = 1;
            foreach (var item in qlstBusinessPlanQuestions)
            {

                string strBusinessPlanStatus = "درحال تامین سرمایه";
                int qRemainingDay = planService.CalculateRemainDay(item.Tbl_BussinessPlans.InvestmentExpireDate);
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
                string strSubQuestion = item.QuestionText;
                if (strSubQuestion.Length > 10)
                    strSubQuestion = strSubQuestion.Substring(0, 10) + " ...";

                lstUserQuestionBusinessPlan.Add(new UserQuestionBusinessPlanList()
                {
                    QuestionBusiness_id = item.QuestionID,
                    Row_id = Row_id,
                    BusinessPlanName = item.Tbl_BussinessPlans.Title,
                    BusinessPlanStatus = strBusinessPlanStatus,
                    CompanyName = item.Tbl_BussinessPlans.CompanyName,
                    BusinessQuestionStatus = strStatusComment,
                    CreateDate = item.CreateDate.Value,
                    QuestionText = strSubQuestion
                });
                Row_id++;
            }

            return View(lstUserQuestionBusinessPlan);
        }

        // GET: UserPanel/UserQuestionBusinessPlan/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_BusinessPlanQuestion tbl_BusinessPlanQuestion = db.Tbl_BusinessPlanQuestion.Find(id);
            if (tbl_BusinessPlanQuestion == null)
            {
                return HttpNotFound();
            }
            return View(tbl_BusinessPlanQuestion);
        }

        // GET: UserPanel/UserQuestionBusinessPlan/Create
        public ActionResult Create()
        {
            ViewBag.Parent_id = new SelectList(db.Tbl_BusinessPlanQuestion, "QuestionID", "QuestionText");
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName");
            ViewBag.BusinessPlan_id = new SelectList(db.Tbl_BussinessPlans, "BussinessPlanID", "Title");
            return View();
        }

        // POST: UserPanel/UserQuestionBusinessPlan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QuestionID,IsActive,IsDeleted,Parent_id,BusinessPlan_id,User_id,CreateDate,QuestionText")] Tbl_BusinessPlanQuestion tbl_BusinessPlanQuestion)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_BusinessPlanQuestion.Add(tbl_BusinessPlanQuestion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Parent_id = new SelectList(db.Tbl_BusinessPlanQuestion, "QuestionID", "QuestionText", tbl_BusinessPlanQuestion.Parent_id);
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_BusinessPlanQuestion.User_id);
            ViewBag.BusinessPlan_id = new SelectList(db.Tbl_BussinessPlans, "BussinessPlanID", "Title", tbl_BusinessPlanQuestion.BusinessPlan_id);
            return View(tbl_BusinessPlanQuestion);
        }

        // GET: UserPanel/UserQuestionBusinessPlan/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_BusinessPlanQuestion tbl_BusinessPlanQuestion = db.Tbl_BusinessPlanQuestion.Find(id);
            if (tbl_BusinessPlanQuestion == null)
            {
                return HttpNotFound();
            }
            ViewBag.Parent_id = new SelectList(db.Tbl_BusinessPlanQuestion, "QuestionID", "QuestionText", tbl_BusinessPlanQuestion.Parent_id);
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_BusinessPlanQuestion.User_id);
            ViewBag.BusinessPlan_id = new SelectList(db.Tbl_BussinessPlans, "BussinessPlanID", "Title", tbl_BusinessPlanQuestion.BusinessPlan_id);
            return View(tbl_BusinessPlanQuestion);
        }

        // POST: UserPanel/UserQuestionBusinessPlan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QuestionID,IsActive,IsDeleted,Parent_id,BusinessPlan_id,User_id,CreateDate,QuestionText")] Tbl_BusinessPlanQuestion tbl_BusinessPlanQuestion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_BusinessPlanQuestion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Parent_id = new SelectList(db.Tbl_BusinessPlanQuestion, "QuestionID", "QuestionText", tbl_BusinessPlanQuestion.Parent_id);
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_BusinessPlanQuestion.User_id);
            ViewBag.BusinessPlan_id = new SelectList(db.Tbl_BussinessPlans, "BussinessPlanID", "Title", tbl_BusinessPlanQuestion.BusinessPlan_id);
            return View(tbl_BusinessPlanQuestion);
        }

        /// <summary>
        /// حذف سوال
        /// </summary>
        /// <param name="id">شناسه سوال</param>
        public void DeleteQuestion(int id)
        {
            Tbl_BusinessPlanQuestion qQuestion = db.Tbl_BusinessPlanQuestion.Find(id);
            if (qQuestion.User_id == UserSetAuthCookie.GetUserID(User.Identity.Name))
            {
                qQuestion.IsDeleted = true;
                qQuestion.IsActive = false;
                db.SaveChanges();
            }
        }

        // GET: UserPanel/UserQuestionBusinessPlan/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_BusinessPlanQuestion tbl_BusinessPlanQuestion = db.Tbl_BusinessPlanQuestion.Find(id);
            if (tbl_BusinessPlanQuestion == null)
            {
                return HttpNotFound();
            }
            return View(tbl_BusinessPlanQuestion);
        }

        // POST: UserPanel/UserQuestionBusinessPlan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_BusinessPlanQuestion tbl_BusinessPlanQuestion = db.Tbl_BusinessPlanQuestion.Find(id);
            db.Tbl_BusinessPlanQuestion.Remove(tbl_BusinessPlanQuestion);
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
