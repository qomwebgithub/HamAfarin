using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HamAfarin
{
    public static class UserSetAuthCookie
    {
        /// <summary>
        /// دریافت ایدی کاربر لاگین شده
        /// </summary>
        /// <param name="userIdentityName"></param>
        /// <returns></returns>
        public static int GetUserID(string userIdentityName)
        {
            return Convert.ToInt32(userIdentityName.Split(',')[0]);
        }

        /// <summary>
        /// دریافت ایدی نقش کاربر لاگین شده
        /// </summary>
        /// <param name="userIdentityName"></param>
        /// <returns></returns>
        public static int GetRoleID(string userIdentityName)
        {
            return Convert.ToInt32(userIdentityName.Split(',')[1]);
        }

        /// <summary>
        /// دریافت نام کاربری کاربر لاگین شده
        /// </summary>
        /// <param name="userIdentityName"></param>
        /// <returns></returns>
        public static string GetUserName(string userIdentityName)
        {
            return userIdentityName.Split(',')[2];
        }

        /// <summary>
        /// دریافت شماره موبایل کاربر لاگین شده
        /// </summary>
        /// <param name="userIdentityName"></param>
        /// <returns></returns>
        public static string GetMobileNumber(string userIdentityName)
        {
            return userIdentityName.Split(',')[3];
        }

        /// <summary>
        /// دریافت ثبت نام بودن در سجام کاربر لاگین شده
        /// </summary>
        /// <param name="userIdentityName"></param>
        /// <returns></returns>
        public static bool GetHasSejam(string userIdentityName)
        {
            bool hasSejam = false;

            string[] array = userIdentityName.Split(',');
            int index = 4;

            if (index < array.Length)
                hasSejam = Convert.ToBoolean(array[index]);

            return hasSejam;
        }

        /// <summary>
        /// دریافت نام و نام خانوادگی کاربر لاگین شده
        /// </summary>
        /// <param name="userIdentityName"></param>
        /// <returns></returns>
        public static string GetFullName(string userIdentityName)
        {
            string fullName = null;

            string[] array = userIdentityName.Split(',');
            int index = 5;

            if (index < array.Length)
                fullName = array[index];

            return fullName;
        }

        /// <summary>
        /// دریافت حقیقی یا حقوقی بودن کاربر لاگین شده
        /// </summary>
        /// <param name="userIdentityName"></param>
        /// <returns></returns>
        public static bool GetIsLegal(string userIdentityName)
        {
            bool isLegal = false;

            string[] array = userIdentityName.Split(',');
            int index = 6;

            if (index < array.Length)
                isLegal = Convert.ToBoolean(array[index]);

            return isLegal;
        }

        public static bool GetIsAffilate(string userIdentityName)
        {
            bool isAffilate = false;

            string[] array = userIdentityName.Split(',');
            int index = 7;

            if (index < array.Length)
                isAffilate = Convert.ToBoolean(array[index]);

            return isAffilate;
        }
    }
}