using Mehdime.Entity;
using Ubik.EF;

namespace Ubik.Web.SSO.Repositories
{
    public class RoleRepository : ReadWriteRepository<UbikRole, AuthDbContext>
    {
        public RoleRepository(IAmbientDbContextLocator ambientDbContextLocator)
            : base(ambientDbContextLocator)
        {
        }
    }
}