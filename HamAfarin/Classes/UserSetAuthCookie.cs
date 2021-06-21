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
        /// <param name="UserIdentityName"></param>
        /// <returns></returns>
        public static int GetUserID(string UserIdentityName)
        {
            int UserID = Convert.ToInt32(UserIdentityName.Split(',')[0]);
            return UserID;
        }   
        /// <summary>
        /// دریافت ایدی نقش کاربر لاگین شده
        /// </summary>
        /// <param name="UserIdentityName"></param>
        /// <returns></returns>
        public static int GetRoleID(string UserIdentityName)
        {
            int RoleID = Convert.ToInt32(UserIdentityName.Split(',')[1]);
            return RoleID;
        } 
        /// <summary>
        /// دریافت نام کاربری کاربر لاگین شده
        /// </summary>
        /// <param name="UserIdentityName"></param>
        /// <returns></returns>
        public static string GetUserName(string UserIdentityName)
        {
            string Username = UserIdentityName.Split(',')[2];
            return Username;
        }  
        /// <summary>
        /// دریافت شماره موبایل کاربر لاگین شده
        /// </summary>
        /// <param name="UserIdentityName"></param>
        /// <returns></returns>
        public static string GetMobileNumber(string UserIdentityName)
        {
            string Username = UserIdentityName.Split(',')[3];
            return Username;
        }
    }
}