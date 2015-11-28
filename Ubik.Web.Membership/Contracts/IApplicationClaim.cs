namespace Ubik.Web.Membership.Contracts
{
    public interface IApplicationClaim
    {
        string ClaimType { get; set; }

        string Value { get; set; }
    }
}