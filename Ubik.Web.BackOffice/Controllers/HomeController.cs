using System.Web.Mvc;

namespace Ubik.Web.BackOffice.Controllers
{
    public class HomeController : BackofficeController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}