using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ubik.Web.Membership.Stores
{
    public class ApplicationRoleStore : RoleStore<ApplicationRole>, IUserStoreWithCustomClaims<string>, IRoleStoreWithCustomClaims
    {
        public ApplicationRoleStore(AuthDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Claim>> RoleRelatedClaims(string userId)
        {
            return
             await Roles.Where(x => x.Users.Any(user => user.UserId == userId))
                    .SelectMany(role => role.RoleClaims)
                    .Distinct()
                    .Select(appClaim => new Claim(appClaim.ClaimType, appClaim.Value))
                    .ToListAsync();
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

        public async Task<IEnumerable<Claim>> RoleRelatedClaims(string userId)
        {
            var db = Context as AuthDbContext;
            var roles = await db.Roles.Where(x => x.Users.Any(u => u.UserId == userId)).Select(x => x.Id).ToListAsync();
            var claims = await db.RoleClaims.Where(x => roles.Any(r => r == x.ApplicationRoleId)).Distinct().ToListAsync();
            return claims.Select(appClaim => new Claim(appClaim.ClaimType, appClaim.Value));
        }
    }

    public interface IUserStoreWithCustomClaims<in TUserKey>
    {
        Task<IEnumerable<Claim>> RoleRelatedClaims(TUserKey userId);
    }

    public interface IRoleStoreWithCustomClaims
    {
        Task<IdentityResult> ClearAllRoleClaims(string role);
    }
}