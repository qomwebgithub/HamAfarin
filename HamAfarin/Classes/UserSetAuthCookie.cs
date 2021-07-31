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
            int UserID = Convert.ToInt32(userIdentityName.Split(',')[0]);
            return UserID;
        }

        /// <summary>
        /// دریافت ایدی نقش کاربر لاگین شده
        /// </summary>
        /// <param name="userIdentityName"></param>
        /// <returns></returns>
        public static int GetRoleID(string userIdentityName)
        {
            int RoleID = Convert.ToInt32(userIdentityName.Split(',')[1]);
            return RoleID;
        }

        /// <summary>
        /// دریافت نام کاربری کاربر لاگین شده
        /// </summary>
        /// <param name="userIdentityName"></param>
        /// <returns></returns>
        public static string GetUserName(string userIdentityName)
        {
            string Username = userIdentityName.Split(',')[2];
            return Username;
        }

        /// <summary>
        /// دریافت شماره موبایل کاربر لاگین شده
        /// </summary>
        /// <param name="userIdentityName"></param>
        /// <returns></returns>
        public static string GetMobileNumber(string userIdentityName)
        {
            string Username = userIdentityName.Split(',')[3];
            return Username;
        }

        /// <summary>
        /// دریافت ثبت نام بودن در سجام کاربر لاگین شده
        /// </summary>
        /// <param name="userIdentityName"></param>
        /// <returns></returns>
        public static bool GetHasSejam(string userIdentityName)
        {
            bool hasSejam = Convert.ToBoolean(userIdentityName.Split(',')[4]);
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
            {
                fullName = array[index];
            }
            return fullName;
        }
    }
}