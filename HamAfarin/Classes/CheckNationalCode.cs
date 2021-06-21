﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class CheckNationalCode
    {
        public bool check(string Code, out string Message)
        {
            try
            {
                char[] chArray = Code.ToCharArray();
                int[] numArray = new int[chArray.Length];
                for (int i = 0; i < chArray.Length; i++)
                {
                    numArray[i] = (int)char.GetNumericValue(chArray[i]);
                }
                int num2 = numArray[9];
                switch (Code)
                {
                    case "0000000000":
                    case "1111111111":
                    case "2222222222":
                    case "3333333333":
                    case "4444444444":
                    case "5555555555":
                    case "6666666666":
                    case "7777777777":
                    case "8888888888":
                    case "9999999999":
                        Message = "کد ملی وارد شده صحیح نمی باشد";

                        return false;
                }
                int num3 = ((((((((numArray[0] * 10) + (numArray[1] * 9)) + (numArray[2] * 8)) + (numArray[3] * 7)) + (numArray[4] * 6)) + (numArray[5] * 5)) + (numArray[6] * 4)) + (numArray[7] * 3)) + (numArray[8] * 2);
                int num4 = num3 - ((num3 / 11) * 11);
                if ((((num4 == 0) && (num2 == num4)) || ((num4 == 1) && (num2 == 1))) || ((num4 > 1) && (num2 == Math.Abs((int)(num4 - 11)))))
                {
                    Message = "";
                    return true;
                }
                else
                {
                    Message = "کد ملی نامعتبر است";

                    return false;
                }
            }
            catch (Exception)
            {
                Message = "لطفا یک عدد 10 رقمی وارد کنید";

                return false;
            }
        }
    }
}
