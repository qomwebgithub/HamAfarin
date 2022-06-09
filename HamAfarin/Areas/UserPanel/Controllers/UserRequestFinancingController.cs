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
    public class UserRequestFinancingController : Controller
    {
        HamAfarinDBEntities db = new HamAfarinDBEntities();
        // GET: UserPanel/RequestFinancing
        public ActionResult Index()
        {
            if (UserSetAuthCookie.GetIsLegal(User.Identity.Name) == false)
            {
                return Redirect("/UserPanel/UserProfile");
            }
            int User_id = UserSetAuthCookie.GetUserID(User.Identity.Name);

            List<Tbl_RequestFinancing> qlstRequestFinancings = db.Tbl_RequestFinancing.Where(p => p.User_id == User_id && p.IsDelete == false).OrderByDescending(p=>p.CreateDate).ToList();
            // اگر طرحی در حال بررسی بود نمیتواند طرح جدید درخواست بدهد
            if (qlstRequestFinancings.Any(p=>p.Status_id == 2))
            {
                ViewBag.CanNotRequest = true;
            }
            List<ListRequestFinancingUserViewModel> requestFinancingUserViewModels = new List<ListRequestFinancingUserViewModel>();
            int row = 1;
            foreach (var item in qlstRequestFinancings)
            {
                requestFinancingUserViewModels.Add(new ListRequestFinancingUserViewModel()
                {
                    Row = row,
                    RequestFinancingID = item.ID,
                    Title = item.Title,
                    Status = item.Tbl_RequestFinancingStatus.Title,
                    RequestDate = item.CreateDate.Value
                });
                row++;
            }
            return View(requestFinancingUserViewModels);
        }

        public ActionResult RequestFinancing()
        {
            if (UserSetAuthCookie.GetIsLegal(User.Identity.Name) == false)
            {
                return Redirect("/UserPanel/UserProfile");
            }
            int User_id = UserSetAuthCookie.GetUserID(User.Identity.Name);

            if (db.Tbl_RequestFinancing.Any(p=>p.IsDelete == false && p.Status_id == 2 && p.User_id == User_id))
            {
                return RedirectToAction("Index");
            }
            ViewBag.SendPlan = db.Tbl_CapitalApplicantHelp.FirstOrDefault().SendPlan;

            return View();
        }

        [HttpPost]
        public ActionResult RequestFinancing(UserRequestFinancingViewModel userRequest, HttpPostedFileBase DocumentFile)
        {
            if (ModelState.IsValid == false)
            {
                return View(userRequest);
            }
            if (DocumentFile == null)
            {
                ModelState.AddModelError("", "لطفا فایل مدارک را بارگذاری کنید");
                return View(userRequest);
            }

            string DocumentFileName = "";
            if (DocumentFile != null)
            {
                DocumentFileName = Guid.NewGuid().ToString() + Path.GetExtension(DocumentFile.FileName);
                DocumentFile.SaveAs(Server.MapPath("/UploadFiles/DocumentFilePlan/" + DocumentFileName));
            }

            Tbl_RequestFinancing oRequestFinancing = new Tbl_RequestFinancing()
            {
                CheckDate = null,
                CreateDate = DateTime.Now,
                Description = userRequest.Description,
                DocumentFile = DocumentFileName,
                IsDelete = false,
                Status_id = 2,
                Title = userRequest.Title,
                User_id = UserSetAuthCookie.GetUserID(User.Identity.Name),
            };
            db.Tbl_RequestFinancing.Add(oRequestFinancing);
            db.SaveChanges();


            return RedirectToAction("Index");
        }

        public ActionResult RequestFinancingDetails(int id)
        {
            if (UserSetAuthCookie.GetIsLegal(User.Identity.Name) == false)
            {
                return Redirect("/UserPanel/UserProfile");
            }

            int User_id = UserSetAuthCookie.GetUserID(User.Identity.Name);
            Tbl_RequestFinancing qRequestFinancing = db.Tbl_RequestFinancing.FirstOrDefault(p => p.ID == id && p.User_id == User_id && p.IsDelete == false);
            if (qRequestFinancing == null)
            {
                return RedirectToAction("Index");
            }

            UserRequestFinancingViewModel userRequestFinancing = new UserRequestFinancingViewModel()
            {
                Description = qRequestFinancing.Description,
                DescriptionAdmin = qRequestFinancing.DescriptionAdmin,
                DocumentFile = "/UploadFiles/DocumentFilePlan/" + qRequestFinancing.DocumentFile,
                Status = qRequestFinancing.Tbl_RequestFinancingStatus.Title,
                Status_id = qRequestFinancing.Status_id,
                Title = qRequestFinancing.Title
            };

            return View(userRequestFinancing);
        }

    }
}