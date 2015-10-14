using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ubik.Infra.Contracts;
using Ubik.Web.Components.Domain;

namespace Ubik.Web.Components.ViewModels
{
    public class SectionSaveModel
    {
        public int Id { get; set; }
        public string FriendlyName { get; set; }
        public DeviceRenderFlavor ForFlavor { get; set; }
    }
    public class SectionViewModel : SectionSaveModel
    {
        public SectionViewModel()
        {
            ForFlavor = DeviceRenderFlavor.Empty;
        }
    }
    public class SectionViewModelBuilder : IViewModelBuilder<Section<int>, SectionViewModel>
    {
        public SectionViewModel CreateFrom(Section<int> entity)
        {
            return new SectionViewModel() { ForFlavor = entity.ForFlavor, FriendlyName = entity.FriendlyName, Id = entity.Id };
        }

        public void Rebuild(SectionViewModel model)
        {
            return;
        }
    }
}
