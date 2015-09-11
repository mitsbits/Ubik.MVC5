using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ubik.Web.Cms.Contracts;
using Ubik.Web.Components.Contracts;

namespace Ubik.Web.Cms
{
    public static class Extensions
    {
        public static void RenderViewSection(this HtmlHelper html, ISection section, Action<Exception> handleException = null)
        {
            IEnumerable<ISlot> items =
                section.Slots.Where(x => x.SectionSlotInfo.Enabled)
                .OrderBy(x => x.SectionSlotInfo.Ordinal)
                .ToList();
            if (!items.Any()) return;
            foreach (var item in items)
            {
                try
                {
                    var toRender = item.Module as IHtmlHelperRendersMe;
                    if (toRender != null) toRender.Render(html);
                }
                catch (Exception ex)
                {
                    if (handleException != null)
                    {
                        handleException.Invoke(ex);
                    }
                }
            }
        }
    }
}
