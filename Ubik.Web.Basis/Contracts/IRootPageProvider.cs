using System;
using System.Collections.Generic;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;

namespace Ubik.Web.Basis.Contracts
{
    internal interface IRootPageProvider
    {
        dynamic RootViewBag { get; }

        IDictionary<string, Object> RootRouteData { get; }

        ViewContext RootViewContext { get; }

        ITempDataDictionary RootTempData { get; }
    }
}