using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Ubik.Web.Backoffice;
using Ubik.Web;

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
        public override void InitHelpers()
        {
            base.InitHelpers();
            PageContent = new BackofficeContentHelper(ViewContext);
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