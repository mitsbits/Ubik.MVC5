﻿using System.Web.Mvc;
using Ubik.Web.Backoffice.Contracts;
using Ubik.Web.Infra;

namespace Ubik.Web.Backoffice
{
    public class BackofficeContentHelper : BasePageHelper, IBackofficeContentProvider
    {
        public BackofficeContentHelper(ViewContext viewContext)
            : base(viewContext)
        {
        }

        private IBackofficeContent _current;

        public IBackofficeContent Current
        {
            get
            {
                var page = _current ?? (_current = (RootViewBag.ContentInfo as IBackofficeContent));
                if (page != null) return page;
                page = Default();
                RootViewBag.ContentInfo = page;
                return page;
            }
        }

        private IBackofficeContent Default()
        {
            var page = new BackofficeContent() { Title = "Ubik 1.0" };
            if (RootRouteData.ContainsKey("Plugin"))
            {
                page.Title = RootRouteData["Plugin"].ToString();
            }
            return page;
        }
    }
}