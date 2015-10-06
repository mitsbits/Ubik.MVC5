using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ubik.Web.Infra.Contracts;

namespace Ubik.Web.Infra
{
    public abstract class BasePageHelper : IRootPageProvider
    {
        protected readonly ViewContext ViewContext;
        protected readonly TempDataDictionary TempData;
        private dynamic _viewBag;
        private IDictionary<string, object> _routeData;


        protected BasePageHelper(ViewContext viewContext)
        {
            ViewContext = viewContext;
            TempData = viewContext.TempData;
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