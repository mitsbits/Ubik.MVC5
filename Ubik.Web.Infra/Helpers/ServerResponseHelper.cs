using System.Collections.Generic;
using System.Web.Mvc;
using Ubik.Infra.Contracts;
using Ubik.Web.Infra.Contracts;

namespace Ubik.Web.Infra.Helpers
{
    public class ServerResponseHelper : BasePageHelper, IServerResponseProvider
    {
        private readonly IServerResponseProvider _provider;
        public ServerResponseHelper(ViewContext viewContext) : base(viewContext)
        {
            _provider = TempDataResponseProvider.Create(ViewContext);
        }

        public ICollection<IServerResponse> Messages {
            get { return _provider.Messages; }
        }
    }
}
