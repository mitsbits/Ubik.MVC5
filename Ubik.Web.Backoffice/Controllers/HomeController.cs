using System.Web.Mvc;

namespace Ubik.Web.Backoffice.Controllers
{
    public class HomeController : BackofficeController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}