using System.Web.Mvc;

namespace Ubik.Web.Cms.Contracts
{
    internal interface IHtmlHelperRendersMe
    {
        void Render(HtmlHelper helper);
    }
}