﻿using System.Web.Mvc;
using System.Web.Routing;

namespace Ubik.Test.MvcClient
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "TestIt", action = "Ordinal", id = UrlParameter.Optional }
            );
        }
    }
}