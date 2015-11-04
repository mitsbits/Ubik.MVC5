using Ubik.Web.Components.Domain;

namespace Ubik.Web.Cms
{
    public class CmsSectionSlotInfo : SectionSlotInfo
    {
        public CmsSectionSlotInfo(string sectionIdentifier, bool enabled, int ordinal)
            : base(sectionIdentifier, enabled, ordinal)
        {
        }
    }
}