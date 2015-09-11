using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Ubik.Web.Auth.Stores
{
    public class ApplicationRoleStore : RoleStore<ApplicationRole>, IUserStoreWithCustomClaims<string>
    {
        public ApplicationRoleStore(AuthDbContext context)
            : base(context)
        {
        }

        public IEnumerable<Claim> RoleRelatedClaims(string userId)
        {
            return
                Roles.Where(x => x.Users.Any(user => user.UserId == userId))
                    .SelectMany(role => role.RoleClaims)
                    .Distinct()
                    .Select(appClaim => new Claim(appClaim.ClaimType, appClaim.Value))
                    .ToList();
        }
    }

    public class ApplicationUserStore : UserStore<ApplicationUser>, IUserStoreWithCustomClaims<string>
    {
        public ApplicationUserStore(AuthDbContext context)
            : base(context)
        {
        }

        public IEnumerable<Claim> RoleRelatedClaims(string userId)
        {
            return Users.Single(u => u.Id == userId)
                      .Roles.Cast<ApplicationRole>().SelectMany(role => role.RoleClaims)
                      .Distinct()
                      .Select(appClaim => new Claim(appClaim.ClaimType, appClaim.Value))
                      .ToList();
        }
    }

    public interface IUserStoreWithCustomClaims<in TUserKey>
    {
        IEnumerable<Claim> RoleRelatedClaims(TUserKey userId);
    }
}