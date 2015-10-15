using Ubik.Infra.Contracts;
using Ubik.Web.Components.Domain;

namespace Ubik.Web.Components.AntiCorruption.ViewModels
{
    public class SlotSaveModel
    {
        public string SectionIdentifier { get; set; }

        public bool Enabled { get; set; }

        public int Ordinal { get; set; }

        public BasePartialModule Module { get; set; }
    }

    public class SlotViewModel : SlotSaveModel
    {

    }
    public class SlotViewModelBuilder : IViewModelBuilder<Slot, SlotViewModel>
    {
        public SlotViewModel CreateFrom(Slot entity)
        {
            var model = new SlotViewModel()
            {
                SectionIdentifier = entity.SectionSlotInfo.SectionIdentifier,
                Enabled = entity.SectionSlotInfo.Enabled,
                Ordinal = entity.SectionSlotInfo.Ordinal,
                Module = entity.Module
            };

            return model;
        }

        public void Rebuild(SlotViewModel model)
        {
            return;
        }
    }
}
