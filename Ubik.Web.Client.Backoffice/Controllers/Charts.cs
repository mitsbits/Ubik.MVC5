using Microsoft.AspNet.Mvc;

namespace Ubik.Web.Client.Backoffice.Controllers
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