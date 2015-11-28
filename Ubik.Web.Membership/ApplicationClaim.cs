using System.Security.Claims;
using Ubik.Web.Membership.Contracts;

namespace Ubik.Web.Membership
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
            Value = value;
            ClaimType = claimType;
        }

        public string ClaimType { get; set; }

        public string Value { get; set; }
    }
}