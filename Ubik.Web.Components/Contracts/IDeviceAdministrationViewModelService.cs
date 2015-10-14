using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ubik.Web.Components.ViewModels;

namespace Ubik.Web.Components.Contracts
{
    public interface IDeviceAdministrationViewModelService
    {
        Task<DeviceViewModel> DeviceModel(int id);
        Task<IEnumerable<DeviceViewModel>> DeviceModels();
    }
}