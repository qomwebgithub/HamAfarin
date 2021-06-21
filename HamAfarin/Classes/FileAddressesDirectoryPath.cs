using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HamAfarin
{
    public static class FileAddressesDirectoryPath
    {

        /// <summary>
        /// آدرس روت اصلی فایل ها و پوشه های ذخیره شده در هاست دانلود برای آپلود
        /// </summary>
        /// <returns></returns>
        public static string UploadRootDirectory()
        {
            return "";
        }


        /// <summary>
        /// آدرس روت هاست دانلود
        /// </summary>
        /// <returns></returns>
        public static string DownloadRootDirectory()
        {

            return "";
        }

        #region Tickets



        /// <summary>
        /// آدرس ذخیره ی فایل آپلود شده در قسمت تیکت 
        /// </summary>
        /// <param name="fileName">نام فایل</param>
        /// <returns></returns>
        public static string TicketFileUploadUrl(string fileName)
        {
            return UploadRootDirectory() + "/UploadFiles/TicketFileUploads/" + fileName;
        }


        /// <summary>
        /// آدرس ذخیره ی فایل آپلود شده در قسمت تیکت 
        /// </summary>
        /// <param name="fileName">نام فایل</param>
        /// <returns></returns>
        public static string TicketFileDownloadUrl(string fileName)
        {
            return UploadRootDirectory() + "/UploadFiles/TicketFileUploads/" + fileName;
        }






        #endregion
    }
}