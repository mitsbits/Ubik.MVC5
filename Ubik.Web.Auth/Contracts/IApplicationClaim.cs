namespace Ubik.Web.Auth.Contracts
{
    public interface IApplicationClaim
    {
        string ClaimType { get; set; }

        string Value { get; set; }
    }
}