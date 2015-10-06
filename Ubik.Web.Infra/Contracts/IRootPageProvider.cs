using System;
using System.Collections.Generic;

namespace Ubik.Web.Infra.Contracts
{
    internal interface IRootPageProvider
    {
        dynamic RootViewBag { get; }

        IDictionary<string, Object> RootRouteData { get; }
    }
}
