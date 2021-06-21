using DataLayer;
using ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hamafarin;

namespace HamAfarin.Controllers
{
    public class QuestionController : Controller
    {
        HamAfarinDBEntities db = new HamAfarinDBEntities();
        UserService userService = new UserService();

        // GET: Question
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// دریافت سوالات یک طرح
        /// </summary>
        /// <param name="id">شناسه طرح</param>
        /// <returns>لیست سوالات</returns>
        public ActionResult ShowQuestionsOfPlan(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                bool isMyAdmin = UserSetAuthCookie.GetRoleID(User.Identity.Name) == 1;
                if (isMyAdmin)
                    ViewBag.IsMyAdmin = isMyAdmin;
            }
            return PartialView(new PlanService().GetQuestionsOfPlan(db, id));
        }

        public ActionResult AddQuestion(int BusinessPlanId, int? id)
        {
            return PartialView(new PlanQuestionItemViewModel()
            {
                BusinessPlan_id = BusinessPlanId,
                Parent_id = id
            });
        }

        // POST: Admin/Product_Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddQuestion(PlanQuestionItemViewModel planQuestion)
        {
            if (ModelState.IsValid)
            {
                Tbl_BusinessPlanQuestion tbl_BusinessPlanQuestion = new Tbl_BusinessPlanQuestion()
                {
                    BusinessPlan_id = planQuestion.BusinessPlan_id,
                    Parent_id = planQuestion.Parent_id,
                    CreateDate = DateTime.Now,
                    User_id = UserSetAuthCookie.GetUserID(User.Identity.Name),
                    QuestionText = planQuestion.QuestionText,
                    IsActive = false,
                    IsDeleted = false
                };
                db.Tbl_BusinessPlanQuestion.Add(tbl_BusinessPlanQuestion);
                db.SaveChanges();


                if (User.Identity.IsAuthenticated)
                {
                    bool isMyAdmin = UserSetAuthCookie.GetRoleID(User.Identity.Name) == 1;
                    if (isMyAdmin)
                        ViewBag.IsMyAdmin = isMyAdmin;
                }
            }
            List<PlanQuestionItemViewModel> listQuestions = new PlanService().GetQuestionsOfPlan(db, planQuestion.BusinessPlan_id.Value);

            return PartialView("ShowQuestionsOfPlan", listQuestions);
        }
    }
}