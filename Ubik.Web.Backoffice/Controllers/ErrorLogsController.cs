using Ubik.Web.Infra.Contracts;

namespace Ubik.Web.Backoffice.Controllers
{
    public class ErrorLogsController : BackofficeController
    {
        private readonly IErrorLogManager _manager;

        public ErrorLogsController(IErrorLogManager manager)
        {
            _manager = manager;
        }
    }
}