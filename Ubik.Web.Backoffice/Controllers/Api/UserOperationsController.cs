using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Ubik.Web.Auth.Contracts;
using Ubik.Web.Auth.ViewModels;

namespace Ubik.Web.Backoffice.Controllers.Api
{
    public class UserOperationsController : BackofficeOperationsController
    {
        private readonly IUserAdminstrationViewModelService _viewModelService;

        public UserOperationsController(IUserAdminstrationViewModelService viewModelService)
        {
            _viewModelService = viewModelService;
        }

        [Route("api/backoffice/users/claimsforroles/{id?}")]
        [HttpGet]
        public IHttpActionResult ClaimsForRoles(string id = "")
        {
            if (string.IsNullOrWhiteSpace(id)) return Ok(new List<RoleClaimViewModel>().ToArray());

            var names = id.Split(';');

            return Ok(_viewModelService.RoleModels()
                   .Where(x => names.Contains(x.Name))
                   .SelectMany(x => x.Claims.Select(c => c.Value))
                   .Distinct()
                   .ToArray());
        }
    }
}