using Microsoft.AspNet.Mvc;
using Ubik.Web.Basis.Contracts;

namespace Ubik.Web.Client.Backoffice.Controllers
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