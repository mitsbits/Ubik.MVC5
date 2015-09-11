using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ubik.Web.Cms.Contracts
{
    internal interface IRootPageProvider
    {
        dynamic RootViewBag { get; }

        IDictionary<string, Object> RootRouteData { get; }
    }
}
