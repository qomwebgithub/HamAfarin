using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using ViewModels;

namespace HamAfarin.Areas.UserPanel.Controllers
{
    public class AffiliateController : Controller
    {
        HamAfarinDBEntities db = new HamAfarinDBEntities();
        // GET: UserPanel/RequestFinancing
        public ActionResult Index()
        {
            int userId = UserSetAuthCookie.GetUserID(User.Identity.Name);

            List<int> tokenIds = db.Tbl_ApiToken.Where(a => a.User_Id == userId).Select(a => a.ID).ToList();

            if (!tokenIds.Any())
                return View();

            List<AffiliateViewModel> affiliateVM = db.Tbl_Affiliate
                .Where(a => tokenIds.Contains(a.Token_Id.Value) && a.Tbl_Users.IsActive)
                .Select(a => new AffiliateViewModel
                {
                    Id = a.User_Id.Value,
                    Mobile = a.Tbl_Users.MobileNumber,
                    CreateDate = a.Tbl_Users.RegisterDate
                }).ToList();

            foreach (var item in affiliateVM)
                item.Mobile = item.Mobile.Remove(4, 4).Insert(4, "****");

            return View(affiliateVM);
        }
    }
}