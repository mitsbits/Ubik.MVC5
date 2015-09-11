using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
