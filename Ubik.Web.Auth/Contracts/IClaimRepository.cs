using Ubik.Infra.Contracts;

namespace Ubik.Web.Auth.Contracts
{
    public interface IClaimRepository : IReadRepository<ApplicationClaim>, IWriteRepository<ApplicationClaim>
    {
    }
}