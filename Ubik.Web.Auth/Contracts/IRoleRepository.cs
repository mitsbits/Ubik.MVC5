using Ubik.Infra.Contracts;

namespace Ubik.Web.Auth.Contracts
{
    public interface IRoleRepository : IReadRepository<ApplicationRole>, IWriteRepository<ApplicationRole>
    {
    }
}