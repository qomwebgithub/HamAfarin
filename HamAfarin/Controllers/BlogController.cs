using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using PagedList;
using ViewModels;

namespace HamAfarin.Controllers
{
    public class BlogController : Controller
    {
        HamAfarinDBEntities db = new HamAfarinDBEntities();
        // GET: Blog
        public ActionResult Index(int page = 1)
        {
            
            List<BlogItemViewModel> lstBlog = new List<BlogItemViewModel>();
            List<Tbl_Blog> qlstBlog = db.Tbl_Blog.Where(b => b.IsActive && b.IsDeleted == false && b.ShowMainPage)
                .OrderByDescending(b => b.CreateDate).ToList();
            foreach (var item in qlstBlog)
            {
                BlogItemViewModel blog = new BlogItemViewModel()
                {
                    BlogID = item.BlogID,
                    Title = item.Title,
                    Description = item.Description,
                    BlogUrl = ConfigurationManager.AppSettings["ThisDomain"] + "/" + item.BlogID
                    + "/" + item.TitleUrl,
                    ImageUrl = ConfigurationManager.AppSettings["ThisDomain"] + "/Images/BlogImages/Image/" + item.ImageName,
                    ImageAlt = item.ImageAlt
                };
                lstBlog.Add(blog);
            }
            IPagedList pagedList = lstBlog.ToPagedList(page, 6);
            ViewBag.Count = lstBlog.Count();
            return PartialView(pagedList);
        }

        [Route("Blog/{id}")]
        public ActionResult SingleBlog(int id)
        {
            Tbl_Blog qBlog = db.Tbl_Blog.Find(id);
            PersianCalendar pct = new PersianCalendar();
            DateTime Dt=new DateTime();
            if (qBlog.CreateDate != null)
            {
                Dt = (DateTime)qBlog.CreateDate ;
            }

            SingleBlogViewModel model =
                new SingleBlogViewModel
                {
                    Title = qBlog.Title,
                    Canonical = qBlog.Canonical,
                    FullText = qBlog.FullText,
                    ImageName = qBlog.ImageName,
                    MetaDescription = qBlog.MetaDescription,
                    MetaTitle = qBlog.MetaTitle,
                    SeoKey = qBlog.SeoKey,
                    ImageAlt = qBlog.ImageAlt,
                    CreateDate = new DateTime(pct.GetYear(Dt), pct.GetMonth(Dt), pct.GetDayOfMonth(Dt), pct.GetHour(Dt), pct.GetMinute(Dt), pct.GetSecond(Dt))
                };
            return View(model);
        }

        public ActionResult BlogPartial()
        {
            List<BlogItemViewModel> lstBlog = new List<BlogItemViewModel>();
            List<Tbl_Blog> qlstBlog = db.Tbl_Blog.Where(b => b.IsActive && b.IsDeleted == false && b.ShowMainPage)
                .OrderByDescending(b => b.CreateDate).ToList();
            foreach (var item in qlstBlog)
            {
                BlogItemViewModel blog = new BlogItemViewModel()
                {
                    BlogID = item.BlogID,
                    Title = item.Title,
                    Description = item.Description,
                    BlogUrl = ConfigurationManager.AppSettings["ThisDomain"] + "/" + item.BlogID
                    + "/" + item.TitleUrl,
                    ImageUrl = ConfigurationManager.AppSettings["ThisDomain"] + "/Images/BlogImages/Image/" + item.ImageName,
                    ImageAlt = item.ImageAlt
                };
                lstBlog.Add(blog);
            }
            return PartialView(lstBlog);
        }
    }
}