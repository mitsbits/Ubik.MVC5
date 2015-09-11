using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ubik.Web.Cms.Contracts;

namespace Ubik.Web.Cms
{
    public abstract class BasePageHelper : IRootPageProvider
    {
        protected readonly ViewContext ViewContext;
        private dynamic _viewBag;
        private IDictionary<string, object> _routeData;

        protected BasePageHelper(ViewContext viewContext)
        {
            ViewContext = viewContext;
        }

        public virtual dynamic RootViewBag
        {
            get { return _viewBag ?? (_viewBag = GetPageViewBag()); }
        }

        public virtual IDictionary<string, object> RootRouteData
        {
            get { return _routeData ?? (_routeData = GetPageRouteData()); }
        }

        private dynamic GetPageViewBag()
        {
            var viewContext = ViewContext;
            while (viewContext.IsChildAction)
            {
                viewContext = ViewContext.ParentActionViewContext;
            }
            return viewContext.ViewBag;
        }

        private Dictionary<string, object> GetPageRouteData()
        {
            var viewContext = ViewContext;
            while (viewContext.IsChildAction)
            {
                viewContext = ViewContext.ParentActionViewContext;
            }
            return viewContext.RouteData.Values.ToDictionary(x => x.Key, x => x.Value);
        }
    }
}