using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using Hamafarin;
using ViewModels;

namespace HamAfarin.Controllers
{
    public class CommentController : Controller
    {
        HamAfarinDBEntities db = new HamAfarinDBEntities();
        UserService userService = new UserService();
        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// دریافت نظرات یک طرح
        /// </summary>
        /// <param name="id">شناسه طرح</param>
        /// <returns>لیست نظرات</returns>
        public ActionResult ShowCommentsOfPlan(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                bool isMyAdmin = UserSetAuthCookie.GetRoleID(User.Identity.Name) == 1;
                if (isMyAdmin)
                    ViewBag.IsMyAdmin = isMyAdmin;
            }

            return PartialView(new PlanService().GetCommentsOfPlan(db, id));
        }

        ///// <summary>
        ///// درج کامنت
        ///// </summary>
        ///// <param name="id">شناسه طرح</param>
        ///// <param name="parentId">نظر بالاسری</param>
        ///// <param name="comment">متن نظر</param>
        ///// <returns>نمایش نتیجه</returns>
        //public ActionResult AddComment(int id,int? parentId,string comment)
        //{
        //    Tbl_CommentPlan tbl_Comment = new Tbl_CommentPlan()
        //    {
        //        BusinessPlan_id = id,
        //        CommentText = comment,
        //        Parent_id = parentId,
        //        IsActive = false,
        //        IsDeleted = false,
        //        CreateDate = DateTime.Now
        //    };
        //    db.Tbl_CommentPlan.Add(tbl_Comment);
        //    db.SaveChanges();
        //    return View("ShowCommentsOfPlan/id="+id);
        //}


        [HttpGet]
        public ActionResult AddComment(int BusinessPlanId, int? id)
        {
            return PartialView(new PlanCommentItemViewModel()
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
        public ActionResult AddComment(PlanCommentItemViewModel planComment)
        {
            if (ModelState.IsValid)
            {
                Tbl_CommentPlan tbl_Comment = new Tbl_CommentPlan()
                {
                    BusinessPlan_id = planComment.BusinessPlan_id,
                    Parent_id = planComment.Parent_id,
                    CreateDate = DateTime.Now,
                    User_id = UserSetAuthCookie.GetUserID(User.Identity.Name),
                    CommentText = planComment.CommentText,
                    IsActive = false
                };
                db.Tbl_CommentPlan.Add(tbl_Comment);
                db.SaveChanges();

                TempData["addCommentIsSuccess"] = true;
                ViewBag.addCommentIsSuccess = true;
                if (User.Identity.IsAuthenticated)
                {
                    bool isMyAdmin = UserSetAuthCookie.GetRoleID(User.Identity.Name) == 1;
                    if (isMyAdmin)
                        ViewBag.IsMyAdmin = isMyAdmin;
                }

            }
            List<PlanCommentItemViewModel> listComments = new PlanService().GetCommentsOfPlan(db, planComment.BusinessPlan_id.Value);

            return PartialView("ShowCommentsOfPlan", listComments);
        }
    }
}