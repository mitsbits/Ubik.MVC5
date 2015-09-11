using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ubik.Web.Components.Contracts
{
    interface IPartialAction
    {
        string Area { get; }

        string Controller { get; }

        string Action { get; }

        IReadOnlyDictionary<string, object> RouteValues { get; }
    }
}
