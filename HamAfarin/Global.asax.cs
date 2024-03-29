﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using GSD.Globalization;
using System.Threading;

namespace HamAfarin
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);   
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (!Context.Request.IsSecureConnection && Context.Request.Url.Host != "localhost")
                Response.Redirect(Context.Request.Url.ToString().Replace("http:", "https:"));

            if (Context.Request.AppRelativeCurrentExecutionFilePath.Contains("~/UserPanel/UserPaymentBusinessPlan/SendFaraboors") == false)
            {
                PersianCulture persianCulture = new PersianCulture();
                Thread.CurrentThread.CurrentCulture = persianCulture;
                Thread.CurrentThread.CurrentUICulture = persianCulture;
            }
        }

        protected void Application_PostAuthorizeRequest()
        {
            HttpContext.Current.SetSessionStateBehavior
                (SessionStateBehavior.Required);
        }
    }
}