using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace Ubik.Web.Membership
{
    public class ApplicationRole : IdentityRole
    {
        private readonly ICollection<ApplicationClaim> _roleClaims;

        public ApplicationRole()
        {
            _roleClaims = new HashSet<ApplicationClaim>();
        }

        public ApplicationRole(string roleName)
            : base(roleName)
        {
            _roleClaims = new HashSet<ApplicationClaim>();
        }

        public ICollection<ApplicationClaim> RoleClaims
        {
            get { return _roleClaims; }
        }
    }
}