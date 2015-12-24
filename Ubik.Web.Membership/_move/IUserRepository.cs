using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ubik.Infra.Contracts;

namespace Ubik.Web.SSO.Contracts
{
    public interface IUserRepository : ICRUDRespoditory<UbikUser>
    {
        Task RemoveFromRole(int userId, string roleName, CancellationToken cancelationToken);

        Task<IEnumerable<string>> GetRoleNames(int userId, CancellationToken cancelationToken);

        Task<bool> IsInRole(int userId, string roleName, CancellationToken cancelationToken);

        Task<IEnumerable<IdentityUserClaim<int>>> GetUserClaims(int userId, CancellationToken cancelationToken);
    }
}