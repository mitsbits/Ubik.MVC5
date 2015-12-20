using Microsoft.AspNet.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Ubik.Web.SSO.Contracts
{


    public interface IRoleStoreWithCustomClaims
    {
        Task<IdentityResult> ClearAllRoleClaims(string role, CancellationToken cancellationToken);
    }
}
