using Mehdime.Entity;
using Ubik.EF;
using Ubik.Web.Membership.Contracts;

namespace Ubik.Web.Membership.Repositories
{
    public class UserRepository : ReadWriteRepository<ApplicationUser, AuthDbContext>, IUserRepository
    {
        public UserRepository(IAmbientDbContextLocator ambientDbContextLocator)
            : base(ambientDbContextLocator)
        { }


    }
}