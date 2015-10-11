using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Ubik.Web.Components.Contracts;

namespace Ubik.Web.Backoffice.Controllers
{
   public class DevicesController : BackofficeController
   {
       private readonly IDeviceAdministrationService<int> _deviceService;

       public DevicesController(IDeviceAdministrationService<int> deviceService)
       {
           _deviceService = deviceService;
       }

       public async Task<ActionResult> Index()
       {
           return View(await _deviceService.All(1, 10000));
       }
   }
}
