﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using ViewModels;

namespace Hamafarin
{
    public class UserService
    {

        HamAfarinDBEntities db = new HamAfarinDBEntities();


        /// <summary>
        /// بدست آوردن شناسه ی کاربر با استفاده از نام کاربری
        /// </summary>
        /// <param name="userName">نام کاربری</param>
        /// <returns>شناسه ی کاربر</returns>
        public int GetUserIdByUserName(string userName)
        {
            int qIntUserId = db.Tbl_Users.Where(u => u.UserName == userName).Single().UserID;
            return qIntUserId;
        }
        public int GetUserIdByUserName(HamAfarinDBEntities db,string userName)
        {
            int qIntUserId = db.Tbl_Users.Where(u => u.UserName == userName).Single().UserID;
            return qIntUserId;
        }
        /// <summary>
        /// تبدیل آبتم دیتابیس به ویو مدل پروفایل
        /// </summary>
        /// <param name="qProfile">آیتم دیتابیس</param>
        /// <returns>ویو مدل پروفایل</returns>
        internal ProfileViewModel ConvertTblProfileToProfileModel(Tbl_UserProfiles qProfile)
        {
            return new ProfileViewModel()
            {
                ProfileID = qProfile.ProfileID,
                MobileNumber = qProfile.MobileNumber,
                FirstName = qProfile.FirstName,
                LastName = qProfile.LastName,
                ImageName = qProfile.ImageName,
                Bio = qProfile.Bio,
                FatherName = qProfile.FatherName,
                ProfileNationalId = qProfile.ProfileNationalId,
                SejamCode = qProfile.SejamCode,
                AccountNumber = qProfile.AccountNumber,
                AccountSheba = qProfile.AccountSheba,
                Email = qProfile.Email,
                BirthDate = qProfile.BirthDate
            };
        }

        internal PersonLegalViewModel ConvertTblLegalToLegalModel(Tbl_PersonLegal qLegal)
        {
            return new PersonLegalViewModel()
            {
                RegistratioNumber = qLegal.RegistratioNumber,
                NationalId = qLegal.NationalId,
                EconomicCode = qLegal.EconomicCode,
                CompanyName = qLegal.CompanyName,
                Address = qLegal.Address
            };
        }

        internal int GetMyUserId(HamAfarinDBEntities db, string userName)
        {
            return GetUserIdByUserName(userName);
        }

        /// <summary>
        /// بدست آوردن نام کاربر با استفاده از شناسه کاربری
        /// </summary>
        /// <param name="id">شناسه کاربری</param>
        /// <returns>نام کاربر</returns>
        public string GetUserNameByUserId(int id)
        {
            string qStrUserName = db.Tbl_Users.Where(u => u.UserID == id).Single().UserName;
            return qStrUserName;
        }

        /// <summary>
        /// بدست آوردن شناسه کاربری با استفاده از موبایل
        /// </summary>
        /// <param name="mobile"> موبایل</param>
        /// <returns>شناسه کاربر</returns>
        public int GetUserIdByMobile(string mobile)
        {
            int qIntUserId = db.Tbl_Users.Where(u => u.MobileNumber == mobile).Single().UserID;
            return qIntUserId;
        }
        /// <summary>
        /// بدست آوردن نقش کاربر با استفاده از نام کاربری
        /// </summary>
        /// <param name="userName">نام کاربری</param>
        /// <returns>شناسه نقش کاربر</returns>
        public int GetRoleTypeByUserName(string userName)
        {
            int qIntRoleId = db.Tbl_Users.Where(u => u.UserName == userName).Single().Role_id.Value;
            return qIntRoleId;
        }

    }
}