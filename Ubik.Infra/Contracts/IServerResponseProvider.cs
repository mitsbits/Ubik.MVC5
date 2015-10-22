using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ubik.Infra.Contracts
{
  public  interface IServerResponseProvider
    {
        ICollection<IServerResponse> Messages { get; }
    }
}
