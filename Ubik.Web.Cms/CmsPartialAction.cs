using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Ubik.Web.Cms.Contracts;
using Ubik.Web.Components.Domain;

namespace Ubik.Web.Cms
{
    public class CmsPartialAction : PartialAction, IHtmlHelperRendersMe
    {
        public CmsPartialAction(string friendlyName, string action, string controller, string area)
            : base(friendlyName, action, controller, area)
        {
        }

        public virtual void Render(HtmlHelper helper)
        {
            var dict = new RouteValueDictionary();
            foreach (var value in RouteValues)
            {
                dict.Add(value.Key, value.Value);
            }
            helper.RenderAction(Action, Controller, dict);
        }
    }
}