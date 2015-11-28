using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Ubik.Infra.Contracts;
using Ubik.Web.Basis;

namespace Ubik.Web
{
    public static class TempDataResponseProviderExtentions
    {
        public static void AddRedirectMessages(this Controller controller, params ServerResponse[] messages)
        {
            var source = controller.TempData[TempDataResponseProvider.Key] as IEnumerable<IServerResponse>;
            var bucket = source == null ? new List<IServerResponse>() : new List<IServerResponse>(source);
            bucket.AddRange(messages);
            controller.TempData[TempDataResponseProvider.Key] = bucket;
        }
    }
}