using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using Ubik.Infra.Contracts;
using Ubik.Web.Components.Domain;

namespace Ubik.Web.Components.ViewModels
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
        [Required]
        public DeviceRenderFlavor Flavor { get; set; }
        public ICollection<SectionViewModel> Sections { get; set; }
    }

    public class DeviceViewModel : DeviceSaveModel
    {
        public DeviceViewModel()
            : base()
        {
            Flavor = DeviceRenderFlavor.Empty;
        }
    }

    public class DeviceViewModelBuilder : IViewModelBuilder<Device<int>, DeviceViewModel>
    {
        public DeviceViewModel CreateFrom(Device<int> entity)
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
                    Id = s.Id
                }).ToList(),
            };

            return model;
        }

        public void Rebuild(DeviceViewModel model)
        {
            return;
        }
    }
}