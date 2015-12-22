using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ubik.Web.SSO.Contracts
{
    public interface IUserStoreWithCustomClaims<in TKey>
    {
        Task<IEnumerable<Claim>> RoleRelatedClaims(TKey userId);
    }
}