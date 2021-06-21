using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;

namespace HamAfarin
{
    public class Generator
    {
        static HamAfarinDBEntities db = new HamAfarinDBEntities();
        public static string InvoiceNumber()
        {
            var datetime = DateTime.Now;
            List<Tbl_BusinessPlanPayment> qlstBusinessPlanPayment = db.Tbl_BusinessPlanPayment.Where(p => p.IsDelete == false).ToList();

            int id = 1;
            string Code = "HAfarin-" + datetime.ToString("yyMMdd") + "-" + id;
            string strTempCode = "HAfarin-" + datetime.ToString("yyMMdd");

            do
            {
                // چک میکنیم کد جنریت شده در دیتابیس موجود نباشد
                if (!qlstBusinessPlanPayment.Any(f => f.InvoiceNumber == Code))
                {
                    return Code;
                }

                // اگر کد در دیتابیس موجود بود کد جدید جنریت میکنیم
                id++;
                Code = strTempCode + "-" + id;

            } while (true);

        }
    }
}