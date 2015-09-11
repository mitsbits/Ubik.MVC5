using Mehdime.Entity;
using Ubik.EF;
using Ubik.Web.Auth.Contracts;

namespace Ubik.Web.Auth.Repositories
{
    internal class ClaimRepository : ReadWriteRepository<ApplicationClaim, AuthDbContext>, IClaimRepository
    {
        public ClaimRepository(IAmbientDbContextLocator ambientDbContextLocator)
            : base(ambientDbContextLocator)
        {
        }
    }
}