using System.Collections.Generic;
using System.Web.Mvc;
using Ubik.Infra.Contracts;
using Ubik.Web.Infra;

namespace Ubik.Web
{
    public static class TempDataResponseProviderExtentions
    {
        public static void AddRedirectMessages(this Controller controller, params IServerResponse[] messages)
        {
            var source = controller.TempData[TempDataResponseProvider.Key] as IEnumerable<IServerResponse>;
            var bucket = source == null ? new List<IServerResponse>() : new List<IServerResponse>(source);
            bucket.AddRange(messages);
            controller.TempData[TempDataResponseProvider.Key] = bucket;
        }
    }
}