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
       private readonly IDeviceAdministrationViewModelService _deviceViewModels;

       public DevicesController(IDeviceAdministrationService<int> deviceService, IDeviceAdministrationViewModelService deviceViewModels)
       {
           _deviceService = deviceService;
           _deviceViewModels = deviceViewModels;
       }

       public async Task<ActionResult> Index()
       {
           return View(await _deviceService.All(1, 10000));
       }

       public Task<ActionResult> LayOuts(int? id )
       {
           if (!id.HasValue)  return AllDevices();
        
           return id.Value > default(int) ? OneDeviceById(id.Value) : NewDevice();
       }

       private async Task<ActionResult> NewDevice()
       {
           return View(await _deviceViewModels.DeviceModel(0));
       }

       private async Task<ActionResult> OneDeviceById(int value)
       {
           return View(await _deviceViewModels.DeviceModel(value));
       }

       private async Task<ActionResult> AllDevices()
       {
           return View(await _deviceViewModels.DeviceModels());
       }
   }
}
