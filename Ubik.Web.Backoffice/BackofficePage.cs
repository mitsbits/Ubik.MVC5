using System.Web.Mvc;
using Ubik.Web.Infra.Helpers;

namespace Ubik.Web.Backoffice
{
    //public abstract class BackofficePage : WebViewPage
    //{
    //    protected BackofficePage()
    //        : base()
    //    {
    //    }
    //    public BackofficeContentHelper PageContent { get; private set; }
    //    public override void InitHelpers()
    //    {
    //        base.InitHelpers();
    //        PageContent = new BackofficeContentHelper(ViewContext);
    //    }

    //}

    public abstract class BackofficePage<TModel> : WebViewPage<TModel>
    {
        protected BackofficePage()
            : base()
        {
        }

        public BackofficeContentHelper PageContent { get; private set; }
        public ServerResponseHelper Feedback { get; private set; }

        public override void InitHelpers()
        {
            base.InitHelpers();
            PageContent = new BackofficeContentHelper(ViewContext);
            Feedback = new ServerResponseHelper(ViewContext);
        }

        public void AddBackofficeBottom(string url)
        {
            const string prefix = @"~/Areas/Backoffice/Scripts/framework/";
            url = url.TrimStart('/');
            var path = string.Format("{0}{1}", Url.Content(prefix), (url.EndsWith(".js") ? url : url + ".js"));
            Html.Statics().FooterScripts.Add(path);
        }
    }
}