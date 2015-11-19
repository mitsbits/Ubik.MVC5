using System.Web.Mvc;
using Ubik.Web.Cms.Contracts;
using Ubik.Web.Infra.Contracts;

namespace Ubik.Web.Backoffice.Controllers
{
    public class NavigationController : BackofficeController
    {
        private readonly IResident _resident;

        public NavigationController(IErrorLogManager errorLogManager, IResident resident)  : base(errorLogManager)
        {
            _resident = resident;
        }

        public ActionResult LeftMenu()
        {
            var model = _resident.Administration.BackofficeMenu;
            return PartialView(model);
        }
    }
}