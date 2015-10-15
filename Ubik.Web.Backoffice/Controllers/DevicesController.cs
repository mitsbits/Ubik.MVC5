using System;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Ubik.Infra;
using Ubik.Web.Components.AntiCorruption.Contracts;
using Ubik.Web.Components.AntiCorruption.ViewModels;
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

        public Task<ActionResult> LayOuts(int? id)
        {
            if (!id.HasValue) return AllDevices();

            return id.Value > default(int) ? OneDeviceById(id.Value) : NewDevice();
        }

        private async Task<ActionResult> NewDevice()
        {
            return View(await _deviceViewModels.DeviceModel(0));
        }

        private async Task<ActionResult> OneDeviceById(int value)
        {
            var model = await _deviceViewModels.DeviceModel(value);
            return View(model);
        }

        private async Task<ActionResult> AllDevices()
        {
            return View(await _deviceViewModels.DeviceModels());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateLayout(DeviceViewModel model)
        {
            try
            {
                var isNew = model.Id == default(int);
                if (!ModelState.IsValid)
                {
                    AddRedirectMessage(ModelState);
                    return View("Layouts", model);
                }
                await _deviceViewModels.Execute(model);
                AddRedirectMessage(ServerResponseStatus.SUCCESS, string.Format("Device '{0}' {1}!", model.FriendlyName, (isNew) ? "created" : "updated"));
                return RedirectToAction("Layouts", "Devices", new { id = model.Id });
            }
            catch (Exception ex)
            {
                AddRedirectMessage(ex);
                return RedirectToAction("Layouts", "Devices", null);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< ActionResult> CreateSection(SectionSaveModel model)
        {
            try
            {
                var isNew = model.SectionId == default(int);
                if (!ModelState.IsValid)
                {
                    AddRedirectMessage(ModelState);
                    return RedirectToAction("LayOuts", "Devices", new {id = model.DeviceId});
                }
                await _deviceViewModels.Execute(model);
                AddRedirectMessage(ServerResponseStatus.SUCCESS, string.Format("Section '{0}' {1}!", model.FriendlyName, (isNew) ? "created" : "updated"));
                return RedirectToAction("Layouts", "Devices", new { id = model.DeviceId, section = model.SectionId });
            }
            catch (Exception ex)
            {
                AddRedirectMessage(ex);
                return RedirectToAction("Layouts", "Devices", null);
            }
        }
    }
}