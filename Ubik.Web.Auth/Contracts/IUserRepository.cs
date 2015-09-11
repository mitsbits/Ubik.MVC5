using Ubik.Infra.Contracts;

namespace Ubik.Web.Auth.Contracts
{
    public interface IUserRepository : IReadRepository<ApplicationUser>, IWriteRepository<ApplicationUser>
    {
    }
}