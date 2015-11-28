using Ubik.Infra.Contracts;

namespace Ubik.Web.Membership.Contracts
{
    public interface IRoleRepository : IReadRepository<ApplicationRole>, IWriteRepository<ApplicationRole>, IReadAsyncRepository<ApplicationRole>
    {
    }
}