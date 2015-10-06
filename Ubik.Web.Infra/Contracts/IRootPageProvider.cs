using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Ubik.Web.Infra.Contracts
{
    internal interface IRootPageProvider
    {
        dynamic RootViewBag { get; }

        IDictionary<string, Object> RootRouteData { get; }

        ViewContext RootViewContext { get; }

        TempDataDictionary RootTempData { get; }
    }
}
