using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HamAfarin.Controllers
{
    public class SliderController : Controller
    {
        HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: Slider
        public ActionResult OtherPageSlider(int? sliderPageId = 1)
        {
            return PartialView(getSliders(sliderPageId.Value));
        }

        public ActionResult AboutUsPageSlider()
        {
            List<Tbl_Sliders> listSlider = getSliders((int)SliderPages.ABOUT_US);
            return PartialView("OtherPageSlider", listSlider);
        }

        private List<Tbl_Sliders> getSliders(int sliderPages)
        {
            List<Tbl_Sliders> qSlider = db.Tbl_Sliders.Where(s => s.IsActive && s.IsDeleted == false && s.IsShowHomePage == false && s.Page_id == sliderPages)
                .OrderByDescending(b => b.CreateDate).ToList();
            if (qSlider == null || qSlider.Count == 0)
            {
                qSlider = db.Tbl_Sliders.Where(b => b.IsActive && b.IsDeleted == false && b.IsShowHomePage == false)
               .OrderByDescending(b => b.CreateDate).ToList();
            }
            return qSlider;
        }
    }
}