using HamAfarin;
using Microsoft.Owin;
using Autofac;
using Owin;
using Parbad.Builder;
using System;
using System.Web.Mvc;
using System.Web.Routing;

[assembly: OwinStartup(typeof(Startup))]

namespace HamAfarin
{
    //This file is an example of How to integrate Parbad with a Dependency Injection library such as Autofac.
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
