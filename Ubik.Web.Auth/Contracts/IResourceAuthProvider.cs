using System.Collections.Generic;
using System.Security.Claims;

namespace Ubik.Web.Auth.Contracts
{
    public interface IResourceAuthProvider
    {
        string[] RoleNames { get; }

        IEnumerable<Claim> Claims(string role);

        string ResourceGroup { get; }

        bool ContainsClaim(Claim claim);
    }
}