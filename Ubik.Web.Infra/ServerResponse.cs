using Ubik.Infra;
using Ubik.Infra.Contracts;

namespace Ubik.Web.Infra
{
   public class ServerResponse : IServerResponse
    {
       public ServerResponseStatus Status { get; set; }
       public string Title { get; set; }
       public string Message { get; set; }
       public object Data { get; set; }
    }
}
