using System.Collections.Generic;
using Ubik.Infra.Contracts;

namespace Ubik.Web.Infra.Contracts
{
    public interface IServerResponseProvider
    {
        ICollection<IServerResponse> Messages { get; }
    }
}