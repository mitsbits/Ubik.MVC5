using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Routing;

namespace Ubik.Web.Client.Backoffice
{
    public static class RouteConfig
    {
        public static void SetBackofficeRoutes(this IRouteBuilder routes)
        {
            routes.MapRoute(name: "areaRoute",
                template: "{area:exists}/{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" });
        }
    }
}