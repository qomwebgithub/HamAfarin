using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace HamAfarin.Areas.UserPanel.Controllers
{
    public class UserPanelController : Controller
    {
        HamAfarinDBEntities db = new HamAfarinDBEntities();
        // GET: UserPanel/UserPanel
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _Menu()
        {
            if (GetUserStatus())
            {
                ViewBag.UserStatus = true;
            }
            return PartialView();
        }

        /// <summary>
        /// دریافت وضعیت کاربر
        /// </summary>
        /// <returns></returns>
        public bool GetUserStatus()
        {
            int UserID = UserSetAuthCookie.GetUserID(User.Identity.Name);
            Tbl_UserProfiles qUserProfile = db.Tbl_UserProfiles.FirstOrDefault(p => p.IsDeleted == false && p.User_id == UserID);
            if (qUserProfile != null)
            {
                return qUserProfile.IsActive;
            }
            return false;
        }
    }
}