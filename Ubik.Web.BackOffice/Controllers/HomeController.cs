using System.Web.Mvc;

namespace Ubik.Web.BackOffice.Controllers
{
    public class HomeController : BackOfficeController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}