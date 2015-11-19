using System.Web.Mvc;
using Ubik.Web.Infra.Contracts;

namespace Ubik.Web.Backoffice.Controllers
{
    public class HomeController : BackofficeController
    {
        public ActionResult Index()
        {
            return View();
        }

        public HomeController(IErrorLogManager errorLogManager) : base(errorLogManager)
        {
        }
    }
}