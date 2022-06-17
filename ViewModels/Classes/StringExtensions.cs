using System;
using System.Globalization;
using System.Linq;

namespace Common
{
    public static class StringExtensions
    {

        public static string MobbileFix(this string mobile)
        {
            if (mobile.Count() == 12)
            {
                int ew = mobile[0];
                int ew2 = mobile[1];
                if (ew != 48)
                {
                    if (ew == 57 && ew2 == 56)
                    {
                        mobile = mobile.Substring(2, 10);
                        mobile = '0' + mobile;
                    }
                }
            }
            return mobile;
        }

        public static bool PhoneValid(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            if (value.Count() < 11 || value.Count() > 11)
            {
                return false;
            }

            if (!NumberPhoneValid(value))
            {
                return false;
            }

            return true;
        }

        public static bool NumberPhoneValid(this string value)
        {
            value = Fa2En(value);


            for (int i = 0; i < value.Count(); i++)
            {
                int ew = value[i];
                if (i == 0)
                {
                    if (ew != 48)
                    {
                        return false;
                    }
                }
                if (ew < 48 || ew > 57)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool NumberValid(this string value)
        {
            value = Fa2En(value);
            for (int i = 0; i < value.Count(); i++)
            {
                int ew = value[i];
                if (ew < 48 || ew > 57)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool HasValue(this string value, bool ignoreWhiteSpace = true)
        {
            return ignoreWhiteSpace ? !string.IsNullOrWhiteSpace(value) : !string.IsNullOrEmpty(value);
        }

        public static int ToInt(this string value)
        {
            return Convert.ToInt32(value);
        }

        public static decimal ToDecimal(this string value)
        {
            return Convert.ToDecimal(value);
        }

        public static string ToNumeric(this int value)
        {
            return value.ToString("N0"); //"123,456"
        }

        public static string ToNumeric(this decimal value)
        {
            return value.ToString("N0");
        }

        public static string ToCurrency(this int value)
        {
            //fa-IR => current culture currency symbol => ریال
            //123456 => "123,123ریال"
            return value.ToString("C0");
        }

        public static string ToCurrency(this decimal value)
        {
            return value.ToString("C0");
        }

        public static string En2Fa(this string str)
        {
            return str.Replace("0", "۰")
                .Replace("1", "۱")
                .Replace("2", "۲")
                .Replace("3", "۳")
                .Replace("4", "۴")
                .Replace("5", "۵")
                .Replace("6", "۶")
                .Replace("7", "۷")
                .Replace("8", "۸")
                .Replace("9", "۹");
        }

        public static string Fa2En(this string str)
        {
            return str.Replace("۰", "0")
                .Replace("۱", "1")
                .Replace("۲", "2")
                .Replace("۳", "3")
                .Replace("۴", "4")
                .Replace("۵", "5")
                .Replace("۶", "6")
                .Replace("۷", "7")
                .Replace("۸", "8")
                .Replace("۹", "9")
                //iphone numeric
                .Replace("٠", "0")
                .Replace("١", "1")
                .Replace("٢", "2")
                .Replace("٣", "3")
                .Replace("٤", "4")
                .Replace("٥", "5")
                .Replace("٦", "6")
                .Replace("٧", "7")
                .Replace("٨", "8")
                .Replace("٩", "9");
        }

        public static string FixPersianChars(this string str)
        {
            return str.Replace("ﮎ", "ک")
                .Replace("ﮏ", "ک")
                .Replace("ﮐ", "ک")
                .Replace("ﮑ", "ک")
                .Replace("ك", "ک")
                .Replace("ي", "ی")
                // .Replace(" ", " ")
                // .Replace("‌", " ")
                .Replace("ھ", "ه");//.Replace("ئ", "ی");
        }

        public static string CleanString(this string str)
        {
            return str.Trim().FixPersianChars().Fa2En().NullIfEmpty();
        }

        public static string NullIfEmpty(this string str)
        {
            return str?.Length == 0 ? null : str;
        }

        /// <summary>
        /// تبدیل تاریخ از 
        /// string
        /// به
        /// datetime   
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static Nullable<DateTime> StringToDate(string strDate)
        {
            if (string.IsNullOrEmpty(strDate) == false)
            {
                strDate = Fa2En(strDate);
                PersianCalendar pc = new PersianCalendar();
                string PersianDate1 = strDate;
                string[] parts = PersianDate1.Split('/', '-');
                return pc.ToDateTime(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]), 0, 0, 0, 0);
            }
            return null;
        }
        public static string MonthcountToMonthName(int month)
        {
            string monthName=null;
            switch (month)
            {
                case 1:
                    monthName = "فروردین";
                    break;

                case 2:
                    monthName = "اردیبهشت";
                    break;

                case 3:
                    monthName = "خرداد";
                    break;

                case 4:
                    monthName = "تیر";
                    break;

                case 5:
                    monthName = "مرداد";
                    break;

                case 6:
                    monthName = "شهریور";
                    break;

                case 7:
                    monthName = "مهر";
                    break;

                case 8:
                    monthName = "آبان";
                    break;

                case 9:
                    monthName = "آذر";
                    break;

                case 10:
                    monthName = "دی";
                    break;

                case 11:
                    monthName = "بهمن";
                    break;

                case 12:
                    monthName = "اسفند";
                    break;

            }
            return monthName;
        }
    }
}
