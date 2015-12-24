using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ubik.Web.SSO.Contracts
{
    public interface IUserStoreWithCustomClaims<in TKey> where TKey : IEquatable<TKey>
    {
        Task<IEnumerable<Claim>> RoleRelatedClaims(TKey userId);
    }
}