using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ubik.Web.Auth.Contracts;

namespace Ubik.Web.Auth
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    public class ApplicationClaim : IApplicationClaim
    {
        public virtual ApplicationRole Role { get; set; }

        public virtual string ApplicationRoleId { get; set; }
        public ApplicationClaim()
        {
            Value = string.Empty;
            ClaimType = string.Empty;
        }
        public ApplicationClaim(Claim claim)
        {
            Value = claim.Value;
            ClaimType = claim.Type;
        }

        public ApplicationClaim(string claimType, string value)
        {
            Value = claimType;
            ClaimType = value;
        }

        public string ClaimType { get; set; }

        public string Value { get; set; }
    }
}
