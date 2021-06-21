using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace HamAfarin.Controllers
{
    public class PagesController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();

        public ActionResult Index(int id, string title)
        {
            Tbl_Pages qPage = db.Tbl_Pages.FirstOrDefault(p => p.PageID == id);
            if (qPage != null)
            {
                return View(qPage);
            }
            return View();
        }
    }
}