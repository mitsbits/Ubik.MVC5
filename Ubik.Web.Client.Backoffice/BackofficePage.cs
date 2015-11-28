using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Mvc.Rendering;
using Ubik.Web.Basis.Helpers;

namespace Ubik.Web.Client.Backoffice
{

    public abstract class BackofficePage<TModel> : RazorPage<TModel>
    {
        protected BackofficePage()
            : base()
        {
            PageContent = new BackofficeContentHelper(ViewContext);
            Feedback = new ServerResponseHelper(ViewContext);
        }

        public BackofficeContentHelper PageContent { get; private set; }
        public ServerResponseHelper Feedback { get; private set; }

        public void AddBackofficeBottom(string urlstring, IUrlHelper url, IHtmlHelper html)
        {
            const string prefix = @"~/Areas/Backoffice/Scripts/framework/";
            urlstring = urlstring.TrimStart('/');

            var path = string.Format("{0}{1}", url.Content(prefix), (urlstring.EndsWith(".js") ? urlstring : urlstring + ".js"));
            html.Statics().FooterScripts.Add(path);
        }
    }
}