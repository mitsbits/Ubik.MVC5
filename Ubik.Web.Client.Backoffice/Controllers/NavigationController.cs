using Microsoft.AspNet.Mvc;
using Ubik.Web.Basis.Contracts;
using Ubik.Web.BuildingBlocks.Contracts;

namespace Ubik.Web.Client.Backoffice.Controllers
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