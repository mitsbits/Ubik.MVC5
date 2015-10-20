using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Ubik.Web.Infra.Contracts;

namespace Ubik.Web.Backoffice.Controllers.Api
{
    public class ErrorLogOperationsController : BackofficeOperationsController
    {
        private readonly IErrorLogManager _manager;

        public ErrorLogOperationsController(IErrorLogManager manager)
        {
            _manager = manager;
        }

        [Route("api/backoffice/errorlogs/clearlogs/")]
        [HttpPost]
        public async Task<IHttpActionResult> ClaimsForRoles([FromBody]IEnumerable<string> ids)
        {
            var tasks = new List<Task<int>>();
            foreach (var id in ids)
            {
                tasks.Add(_manager.ClearLog(id));
            }
            await Task.WhenAll(tasks.ToArray());
            return Ok();
        }
    }
}
