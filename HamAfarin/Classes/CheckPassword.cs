using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Common
{
    public class CheckPassword
    {
        public bool check(string Password, out string Message)
        {
            try
            {
                Message = "با موفقیت انجام شد";
                string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})";
                
                bool isValidPassword = Regex.IsMatch(Password, pattern);
                
                if (!isValidPassword)
                {
                    throw new Exception();
                }

                return true;

            }
            catch (Exception)
            {
                Message = "پسورد باید حداقل شامل یک حروف بزرگ , کوچک انگلیسی , یک عدد , یکی از علامت های !@#$%^&* , 8 کاراکتر باشد";

                return false;
            }
        }
    }
}