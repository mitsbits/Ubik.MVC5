using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ubik.Web.Infra.Contracts;

namespace Ubik.Web.Infra
{
    public abstract class BasePageHelper : IRootPageProvider
    {
        protected readonly ViewContext _rootViewContext;
        protected readonly TempDataDictionary _rootTempData;
        private dynamic _viewBag;
        private IDictionary<string, object> _routeData;

        protected BasePageHelper(ViewContext viewContext)
        {
            _rootViewContext = viewContext;
            while (_rootViewContext.IsChildAction)
            {
                _rootViewContext = _rootViewContext.ParentActionViewContext;
            }
            _rootTempData = _rootViewContext.TempData;
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
            return RootViewContext.ViewBag;
        }

        private Dictionary<string, object> GetPageRouteData()
        {
            return RootViewContext.RouteData.Values.ToDictionary(x => x.Key, x => x.Value);
        }

        public ViewContext RootViewContext
        {
            get { return _rootViewContext; }
        }

        public TempDataDictionary RootTempData
        {
            get { return RootViewContext.TempData; }
        }
    }
}