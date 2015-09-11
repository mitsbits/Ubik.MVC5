using System.Collections.Generic;
using Ubik.Web.Components;
using Ubik.Web.Components.Contracts;

namespace Ubik.Web.Cms
{
    public class CmsDevice : IDevice
    {
        private readonly ICollection<ISection> _sections;

        public CmsDevice()
        {
            _sections = new HashSet<ISection>();
        }

        public string FriendlyName { get; set; }

        public string Path { get; set; }

        public ICollection<ISection> Sections
        {
            get { return _sections; }
        }

        public DeviceRenderFlavor Flavor { get; set; }
    }
}