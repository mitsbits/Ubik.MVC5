using Mehdime.Entity;
using Ubik.EF;
using Ubik.Web.Auth.Contracts;

namespace Ubik.Web.Auth.Repositories
{
    internal class UserRepository : ReadWriteRepository<ApplicationUser, AuthDbContext>, IUserRepository
    {
        public UserRepository(IAmbientDbContextLocator ambientDbContextLocator)
            : base(ambientDbContextLocator) { }
    }
}