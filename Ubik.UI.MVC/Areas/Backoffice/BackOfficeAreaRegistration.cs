using System.Web.Mvc;

namespace Ubik.UI.MVC.Areas.Backoffice
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