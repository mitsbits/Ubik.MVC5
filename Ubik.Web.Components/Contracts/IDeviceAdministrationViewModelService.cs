using System.Collections;
using System.Collections.Generic;
using Ubik.Web.Components.ViewModels;

namespace Ubik.Web.Components.Contracts
{
    public interface IDeviceAdministrationViewModelService
    {
        DeviceViewModel DeviceModel(int id);
        IEnumerable<DeviceRowViewModel> DeviceModels();
    }
}