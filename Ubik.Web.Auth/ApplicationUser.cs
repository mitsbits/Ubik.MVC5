using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;
using Ubik.Web.Auth.Managers;

namespace Ubik.Web.Auth
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            var claimsManager = manager as IAuthenticatedUserManager;
            if (claimsManager != null)
                userIdentity.AddClaims(claimsManager.RoleRelatedClaims(userIdentity.GetUserId()));
            return userIdentity;
        }
    }
}