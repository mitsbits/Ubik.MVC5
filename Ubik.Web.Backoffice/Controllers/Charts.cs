using System.Web.Mvc;

namespace Ubik.Web.Backoffice.Controllers
{
    public class ChartsController : Controller
    {
        public ActionResult Flot()
        {
            return View();
        }

        public ActionResult Inline()
        {
            return View();
        }

        public ActionResult Morris()
        {
            return View();
        }
    }
}