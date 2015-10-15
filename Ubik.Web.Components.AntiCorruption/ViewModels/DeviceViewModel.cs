using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Ubik.Infra.Contracts;
using Ubik.Web.Components.Domain;
using Ubik.Web.EF.Components;

namespace Ubik.Web.Components.AntiCorruption.ViewModels
{
    public class DeviceSaveModel
    {
        public DeviceSaveModel()
        {
            Sections = new HashSet<SectionViewModel>();
        }
        [Required]
        public int Id { get; set; }
        [Required]
        public string FriendlyName { get; set; }
        [Required]
        public string Path { get; set; }

        public DeviceRenderFlavor Flavor { get; set; }

        public ICollection<SectionViewModel> Sections { get; set; }
    }

    public class DeviceViewModel : DeviceSaveModel
    {
        public DeviceViewModel()
            : base()
        {

        }
    }

    public class DeviceViewModelBuilder : IViewModelBuilder<PersistedDevice, DeviceViewModel>
    {
        public DeviceViewModel CreateFrom(PersistedDevice entity)
        {
            var model = new DeviceViewModel
            {
                Flavor = entity.Flavor,
                FriendlyName = entity.FriendlyName,
                Id = entity.Id,
                Path = entity.Path,
                Sections = entity.Sections.Select(s => new SectionViewModel()
                {
                    ForFlavor = s.ForFlavor,
                    FriendlyName = s.FriendlyName,
                    SectionId = s.Id
                }).ToList(),
            };

            return model;
        }

        public void Rebuild(DeviceViewModel model)
        {
            return;
        }
    }

    public class DeviceViewModelCommand : IViewModelCommand<DeviceSaveModel>
    {
        private readonly ICRUDRespoditory<PersistedDevice> _persistedDeviceRepo;

        public DeviceViewModelCommand(ICRUDRespoditory<PersistedDevice> persistedDeviceRepo)
        {
            _persistedDeviceRepo = persistedDeviceRepo;
        }

        public async Task Execute(DeviceSaveModel model)
        {
            PersistedDevice data;
            if (model.Id != default(int))
            {
                data = await _persistedDeviceRepo.GetAsync(x => x.Id == model.Id);
                data.Flavor = model.Flavor;
                data.FriendlyName = model.FriendlyName;
                data.Path = model.Path;
                await _persistedDeviceRepo.UpdateAsync(data);
            }
            else
            {
                data = new PersistedDevice(){FriendlyName = model.FriendlyName, Flavor = model.Flavor, Path = model.Path};
                await _persistedDeviceRepo.CreateAsync(data);
            }
        }
    }
}