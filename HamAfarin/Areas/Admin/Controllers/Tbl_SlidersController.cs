using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using HamAfarin;
using ViewModels;

namespace Hamafarin.Areas.Admin.Controllers
{
    public class Tbl_SlidersController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: Admin/Tbl_Sliders
        public ActionResult Index()
        {
            List<Tbl_Sliders> qSliders = db.Tbl_Sliders.Where(s => s.IsDeleted == false).OrderByDescending(c => c.CreateDate).ToList();
            List<AddEditSliderViewModel> sliderModels = new List<AddEditSliderViewModel>();
            foreach (var item in qSliders)
            {
                sliderModels.Add(convertToModel(item));
            }
            return View(sliderModels.ToList());
        }

        private string getPageFromType(int pageId)
        {
            switch (pageId)
            {
                case (int)SliderPages.MAIN:
                    return "صفحه اصلی";
                case (int)SliderPages.ALL_PLAN:
                    return "همه طرح ها";
                case (int)SliderPages.ABOUT_US:
                    return "درباره ما";
                case (int)SliderPages.PERMITS:
                    return "مجوزات";
                case (int)SliderPages.CONTACT_US:
                    return "تماس با ما";
                case (int)SliderPages.INVESTMENT_PROCESS:
                    return "فرآیند سرمایه گذاری";
                case (int)SliderPages.SITE_TERMS_CONDITIONS:
                    return "قوانین و مقررات سایت";
                case (int)SliderPages.PRIVACY:
                    return "حفظ حریم خصوصی";
                case (int)SliderPages.RISK_ALERT_STATEMENT:
                    return "بیانیه ریسک";
                case (int)SliderPages.SINGLE_FURURE_PLAN:
                    return "طرح های آینده";
                case (int)SliderPages.CAMPAIGN_PROCESS:
                    return "سوالات متداول";
                default:
                    return "";
            }
        }

        private AddEditSliderViewModel convertToModel(Tbl_Sliders item)
        {
            string pageTitle = "";
            if (item.IsShowHomePage) pageTitle = getPageFromType((int)SliderPages.MAIN);
            else if (item.Page_id != null) pageTitle = getPageFromType(item.Page_id.Value);

            AddEditSliderViewModel model = new AddEditSliderViewModel()
            {
                SliderID = item.SliderID,
                Title = item.Title,
                Url = item.Url,
                IsActive = item.IsActive,
                ImageName = item.ImageName,
                IsShowHomePage = item.IsShowHomePage,
                Page_id = item.Page_id,
                InMobile = item.InMobile,
                Page_Title = pageTitle
            };
            return model;
        }

        private Tbl_Sliders convertToEntity(AddEditSliderViewModel model)
        {

            Tbl_Sliders item = new Tbl_Sliders()
            {
                SliderID = model.SliderID,
                Url = model.Url,
                IsActive = model.IsActive,
                ImageName = model.ImageName,
                IsShowHomePage = model.IsShowHomePage,
                Page_id = model.Page_id
            };
            return item;
        }

        // GET: Admin/Tbl_Sliders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Sliders tbl_Sliders = db.Tbl_Sliders.Find(id);
            if (tbl_Sliders == null)
            {
                return HttpNotFound();
            }
            return View(convertToModel(tbl_Sliders));
        }

        // GET: Admin/Tbl_Sliders/Create
        public ActionResult Create()
        {
            setPagesViewBag(null);
            return View();
        }

        // POST: Admin/Tbl_Sliders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddEditSliderViewModel sliderModel, HttpPostedFileBase imgUp)
        {
            if (ModelState.IsValid)
            {
                if (imgUp == null)
                {
                    ModelState.AddModelError("ImageName", "تصویر را انتخاب کنید");
                    setPagesViewBag(sliderModel.Page_id);
                    return View(sliderModel);
                }
                Tbl_Sliders tbl_Sliders = new Tbl_Sliders();
                tbl_Sliders.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                imgUp.SaveAs(Server.MapPath("/Images/SliderImages/" + tbl_Sliders.ImageName));
                tbl_Sliders.CreateDate = DateTime.Now;
                tbl_Sliders.IsDeleted = false;
                if (tbl_Sliders.IsShowHomePage == true)
                    tbl_Sliders.Page_id = 1;
                else
                    tbl_Sliders.Page_id = sliderModel.Page_id;
                tbl_Sliders.Url = sliderModel.Url;
                tbl_Sliders.Title = sliderModel.Title;
                tbl_Sliders.IsShowHomePage = sliderModel.IsShowHomePage;
                tbl_Sliders.IsActive = sliderModel.IsActive;
                tbl_Sliders.InMobile = sliderModel.InMobile;
                db.Tbl_Sliders.Add(tbl_Sliders);
                db.SaveChanges();
                return RedirectToAction("Details/" + tbl_Sliders.SliderID);
            }

            setPagesViewBag(sliderModel.Page_id);
            return View(sliderModel);
        }

        // GET: Admin/Tbl_Sliders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Sliders tbl_Sliders = db.Tbl_Sliders.Find(id);
            if (tbl_Sliders == null)
            {
                return HttpNotFound();
            }
            if (tbl_Sliders.IsShowHomePage == true)
                tbl_Sliders.Page_id = 1;
            setPagesViewBag(tbl_Sliders.Page_id);
            return View(convertToModel(tbl_Sliders));
        }

        private void setPagesViewBag(int? selectedPage)
        {
            List<DropDownViewModel> pageList = new List<DropDownViewModel>()
            {
                new DropDownViewModel(){key = 2,value= getPageFromType(2)},
                new DropDownViewModel(){key = 3,value= getPageFromType(3)},
                new DropDownViewModel(){key = 4,value= getPageFromType(4)},
                new DropDownViewModel(){key = 5,value= getPageFromType(5)},
                new DropDownViewModel(){key = 6,value= getPageFromType(6)},
                new DropDownViewModel(){key = 7,value= getPageFromType(7)},
                new DropDownViewModel(){key = 8,value= getPageFromType(8)},
                new DropDownViewModel(){key = 9,value= getPageFromType(9)}
            };
            ViewBag.Page_id = new SelectList(pageList, "key", "value", selectedPage == null ? 1 : selectedPage);

        }

        // POST: Admin/Tbl_Sliders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AddEditSliderViewModel sliders, HttpPostedFileBase imgUp)
        {
            if (ModelState.IsValid)
            {
                if (imgUp != null)
                {
                    System.IO.File.Delete(Server.MapPath("/Images/SliderImages/" + sliders.ImageName));
                    sliders.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                    imgUp.SaveAs(Server.MapPath("/Images/SliderImages/" + sliders.ImageName));
                }
                Tbl_Sliders tbl_Sliders = db.Tbl_Sliders.Find(sliders.SliderID);
                if (tbl_Sliders.IsShowHomePage == true)
                    tbl_Sliders.Page_id = 1;
                else
                    tbl_Sliders.Page_id = sliders.Page_id;
                tbl_Sliders.Url = sliders.Url;
                tbl_Sliders.Title = sliders.Title;
                tbl_Sliders.IsShowHomePage = sliders.IsShowHomePage;
                tbl_Sliders.IsActive = sliders.IsActive;
                tbl_Sliders.InMobile = sliders.InMobile;
                tbl_Sliders.ImageName = sliders.ImageName;
                db.Entry(tbl_Sliders).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details/" + tbl_Sliders.SliderID);
            }
            setPagesViewBag(sliders.Page_id);
            return View(sliders);
        }

        // GET: Admin/Tbl_Sliders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Sliders tbl_Sliders = db.Tbl_Sliders.Find(id);
            if (tbl_Sliders == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Sliders);
        }

        // POST: Admin/Tbl_Sliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Sliders tbl_Sliders = db.Tbl_Sliders.Find(id);
            tbl_Sliders.IsDeleted = true;
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
