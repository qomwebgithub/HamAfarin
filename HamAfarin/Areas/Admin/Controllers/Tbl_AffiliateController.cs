using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels;
using Common;
using AutoMapper;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using System.Globalization;

namespace HamAfarin.Areas.Admin.Controllers
{
    public class Tbl_AffiliateController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();
        SMS oSms = new SMS();

        // GET: UserPanel/DepositToInvestors
        public async Task<ActionResult> Index()
        {
            var fullJoin = await
                (from api in db.Tbl_ApiToken
                join userProfiles in db.Tbl_UserProfiles on api.User_Id equals userProfiles.User_id into UserProfileGroup
                from upg in UserProfileGroup.DefaultIfEmpty()
                join affiliate in db.Tbl_Affiliate on api.ID equals affiliate.Token_Id into AffiliateGroup
                from ag in AffiliateGroup.DefaultIfEmpty()
                join businessPlanPayment in db.Tbl_BusinessPlanPayment 
                    on ag.User_Id equals businessPlanPayment.PaymentUser_id into PaymentGroup
                from pg in PaymentGroup.Where(g => g.IsConfirmedFromFaraboors && g.IsDelete == false).DefaultIfEmpty()
                select new
                {
                    ID = api.ID,
                    User_Id = api.User_Id,
                    Mobile = upg.MobileNumber,
                    Username = upg.Tbl_Users.UserName,
                    Name = api.Name,
                    Url = api.Url,
                    PaymentPrice = pg.PaymentPrice ?? 0,
                }).ToListAsync();

            var groupInvest =
                from a in fullJoin
                group a.PaymentPrice by a.ID into g
                select new ApiTokenViewModel
                {
                    ID = g.Key,
                    TotalInvestment = g.Sum(),
                };

            var groupUser =
                from a in fullJoin
                group a.User_Id by new { a.ID, a.Mobile, a.Username, a.Name, a.Url } into g
                select new ApiTokenViewModel
                {
                    ID = g.Key.ID,
                    Mobile = g.Key.Mobile,
                    Username = g.Key.Username,
                    Name = g.Key.Name,
                    Url = g.Key.Url,
                    UserCount = g.Count(),
                };

            var apiTokenVM = 
                from u in groupUser
                join i in groupInvest on u.ID equals i.ID
                select new ApiTokenViewModel
                {
                    ID = u.ID,
                    Mobile = u.Mobile,
                    Username = u.Username,
                    Name = u.Name,
                    Url = u.Url,
                    UserCount = u.UserCount,
                    TotalInvestment = i.TotalInvestment,
                };

            return View(apiTokenVM.ToList());
        }
    }
}