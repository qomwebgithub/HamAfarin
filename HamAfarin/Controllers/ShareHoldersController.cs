using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HamAfarin.Controllers
{
    public class ShareHoldersController : Controller
    {
        HamAfarinDBEntities db = new HamAfarinDBEntities();
        // GET: ShareHolders
        [Route("ShareHolder/{id}")]
        public ActionResult SingleShareHolder(int id)
        {
            return View(db.Tbl_ShareHoldersCompany.Find(id));
        }



    }
}