using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Ubik.Web.BuildingBlocks.Contracts;

namespace Ubik.Web.Client.Backoffice.ViewComponents
{
    [ViewComponent(Name = "MainMenu")]
    public class MainMenuComponent : ViewComponent
    {
        private readonly IResident _resident;
        public MainMenuComponent(IResident resident)
        {
            _resident = resident;
        }

        public  IViewComponentResult  Invoke()
        {
            var model = _resident.Administration.BackofficeMenu;
            return View(model);
        }
    }
}
