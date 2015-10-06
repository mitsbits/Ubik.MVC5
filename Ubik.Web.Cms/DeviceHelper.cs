using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ubik.Web.Cms.Contracts;
using Ubik.Web.Components;
using Ubik.Web.Components.Contracts;
using Ubik.Web.Infra;

namespace Ubik.Web.Cms
{
    public class DeviceHelper : BasePageHelper, IDevicePageProvider
    {
        public DeviceHelper(ViewContext viewContext)
            : base(viewContext)
        {
        }

        private IDevice _device;

        public IDevice Current
        {
            get { return _device ?? (_device = RootViewBag.Device as IDevice); }
        }

        public IEnumerable<ISection> ActiveSections
        {
            get
            {
                var flavor = Current.Flavor;
                return flavor == DeviceRenderFlavor.Empty
                    ? GetForEmptyFlavor() : GetForFlavor(flavor);
            }
        }

        private IEnumerable<ISection> GetForEmptyFlavor()
        {
            return new List<ISection>(Current.Sections
                .Where(s => s.Slots.Any(l => l.SectionSlotInfo.Enabled))
                .ToList());
        }

        private IEnumerable<ISection> GetForFlavor(DeviceRenderFlavor flavor)
        {
            return new List<ISection>(Current.Sections
                .Where(s => s.ForFlavor.HasFlag(flavor) &&
                    s.Slots.Any(l => l.SectionSlotInfo.Enabled))
                .ToList());
        }
    }
}