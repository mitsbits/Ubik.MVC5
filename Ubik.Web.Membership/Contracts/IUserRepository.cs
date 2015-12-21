using System.Threading.Tasks;
using Ubik.Infra.Contracts;

namespace Ubik.Web.Membership.Contracts
{
    public interface IUserRepository : IReadRepository<ApplicationUser>, IWriteRepository<ApplicationUser>
    {
        Task RemoveFromRole(int userId, string roleName);
    }
}