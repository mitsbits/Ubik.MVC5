using System.Web.Mvc;

namespace Ubik.Web.Presenatation.Admin.Areas.BackOffice
{
    public class BackOfficeAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "BackOffice";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            var defaultRoute = context.MapRoute(
                  "BackOffice_default",
                  "BackOffice/{controller}/{action}/{id}",
                  new { action = "Index", controller = "Home", id = UrlParameter.Optional },
                  null,
                  new[] { "Ubik.Web.BackOffice.Controllers" }
              );
        }
    }
}