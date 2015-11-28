using Mehdime.Entity;
using Ubik.EF;
using Ubik.Web.Membership.Contracts;

namespace Ubik.Web.Membership.Repositories
{
    internal class ClaimRepository : ReadWriteRepository<ApplicationClaim, AuthDbContext>, IClaimRepository
    {
        public ClaimRepository(IAmbientDbContextLocator ambientDbContextLocator)
            : base(ambientDbContextLocator)
        {
        }
    }
}