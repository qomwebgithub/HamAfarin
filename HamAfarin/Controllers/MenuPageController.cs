using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HamAfarin.Controllers
{

    public class MenuPageController : Controller
    {
        HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: MenuPage
        /// <summary>
        /// گرفتن لیست منوهای صفحه
        /// </summary>
        /// <param name="id">شناسه نوع منوی صفحه</param>
        /// <param name="title">عنوان منوی صفحه</param>
        /// <returns>لیستی از منوی های صفحه</returns>
        [Route("Page/{id}/{title}")]
        public ActionResult Index(int id, string title)
        {
            List<Tbl_MenuPage> qMenus = db.Tbl_MenuPage.Where(m=>m.MenuType_id==id && m.IsActive && m.IsDelete==false).OrderBy(o=>o.Sort).ToList();
            return PartialView(qMenus);
        }
    }
}