using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using DataLayer;
using Hamafarin;
using ViewModels;

namespace HamAfarin.Areas.Admin.Controllers
{
    public class Tbl_WalletController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();

        public ActionResult Index()
        {

            return View();
        }
        

    }
}
