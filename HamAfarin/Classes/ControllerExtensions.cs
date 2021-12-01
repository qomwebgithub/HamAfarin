using System;
using System.Web.Mvc;

namespace HamAfarin.Classes
{
    public static class ControllerExtensions
    {
        public static string ControllerName(this string controller)
        {
            return controller.EndsWith("Controller") ? controller.Substring(0, controller.Length - 10) : controller;
        }
    }
}