using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ViewModels
{
    public class CustomPasswordValidator : ValidationAttribute
    {

        public override bool IsValid(object value)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})";
            bool isValidPassword = Regex.IsMatch(value.ToString(), pattern);
            return isValidPassword;
        }
    }
}