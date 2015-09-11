using System.Web.Mvc;
using System.Web.Mvc.Html;
using Ubik.Web.Cms.Contracts;
using Ubik.Web.Components.Domain;

namespace Ubik.Web.Cms
{
    public class CmsPartialView : PartialView, IHtmlHelperRendersMe
    {
        public CmsPartialView(string friendlyName, string viewPath)
            : base(friendlyName, viewPath)
        {
        }

        public virtual void Render(HtmlHelper helper)
        {
            helper.RenderPartial(ViewPath, Parameters);
        }
    }
}