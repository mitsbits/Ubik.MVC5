using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ubik.Web.BackOffice;

namespace Ubik.Web.Backoffice
{
    public abstract class BackofficePage : WebViewPage
    {
        protected BackofficePage()
            : base()
        {
        }
        public BackofficeContentHelper Content { get; private set; }
        public override void InitHelpers()
        {
            base.InitHelpers();
            Content = new BackofficeContentHelper(ViewContext);
        }
    }

    public abstract class BackofficePage<TModel> : WebViewPage<TModel>
    {
        protected BackofficePage()
            : base()
        {
        }

        public BackofficeContentHelper Content { get; private set; }
        public override void InitHelpers()
        {
            base.InitHelpers();
            Content = new BackofficeContentHelper(ViewContext);
        }
    }
}