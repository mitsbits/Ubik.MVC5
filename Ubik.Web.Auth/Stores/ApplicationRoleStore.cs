using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ubik.Web.Auth.Stores
{
    public class ApplicationRoleStore : RoleStore<ApplicationRole>, IUserStoreWithCustomClaims<string>, IRoleStoreWithCustomClaims<string>
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

        public async Task<IdentityResult> ClearAllRoleClaims(string role)
        {
            var db = Context as AuthDbContext;
            var claims = db.RoleClaims.Where(x => x.Role.Name == role);
            foreach (var applicationClaim in claims)
            {
                db.RoleClaims.Remove(applicationClaim);
            }
            try
            {
                await db.SaveChangesAsync();
                return await Task.FromResult(IdentityResult.Success);
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(ex.Message);
            }
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

    public interface IRoleStoreWithCustomClaims<in TUserKey>
    {
        Task<IdentityResult> ClearAllRoleClaims(string role);
    }
}