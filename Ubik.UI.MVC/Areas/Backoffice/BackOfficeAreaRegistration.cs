using System.Web.Mvc;

namespace Ubik.UI.MVC.Areas.Backoffice
{
    public class BackofficeAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Backoffice";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Backoffice_default",
                "Backoffice/{controller}/{action}/{id}",
                new { action = "Index", controller = "Home", id = UrlParameter.Optional },
                new[] { "Ubik.Web.Backoffice.Controllers" }
            );
        }
    }
}