using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ubik.Web.Membership.Managers;

namespace Ubik.Web.Membership
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            var claimsManager = manager as IAuthenticatedUserManager;
            if (claimsManager != null)
            {
                var customClaims = await claimsManager.RoleRelatedClaims(userIdentity.GetUserId());
                userIdentity.AddClaims(customClaims.ToList());
            }

            return userIdentity;
        }
    }
}