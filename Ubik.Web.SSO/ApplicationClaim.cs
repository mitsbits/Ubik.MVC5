using System.Security.Claims;
using Ubik.Web.SSO.Contracts;

namespace Ubik.Web.SSO
{
    public class ApplicationClaim : IApplicationClaim
    {
        public virtual UbikRole Role { get; set; }

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