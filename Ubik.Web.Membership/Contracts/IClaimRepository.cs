using Ubik.Infra.Contracts;

namespace Ubik.Web.Membership.Contracts
{
    public interface IClaimRepository : IReadRepository<ApplicationClaim>, IWriteRepository<ApplicationClaim>
    {
    }
}