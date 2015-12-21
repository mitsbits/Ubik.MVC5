using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ubik.EF;
using Ubik.Web.SSO.Contracts;

namespace Ubik.Web.SSO.Repositories
{
    public class UserRepository : ReadWriteRepository<UbikUser, AuthDbContext>, IUserRepository
    {
        public UserRepository(IAmbientDbContextLocator ambientDbContextLocator)
            : base(ambientDbContextLocator)
        {
          
        }

        public async Task<IEnumerable<string>> GetRoleNames(int userId, CancellationToken cancelationToken)
        {
            return await DbContext.Roles.AsNoTracking().Where(x => x.Users.Any(u => u.UserId == userId)).OrderBy(x => x.Name).Select(x => x.Name).ToListAsync(cancelationToken);
        }

        public Task<bool> IsInRole(int userId, string roleName, CancellationToken cancelationToken)
        {
            return Task.FromResult(DbContext.Roles.Any(x => x.Users.Any(u => u.UserId == userId)));
        }

        public async virtual Task RemoveFromRole(int userId, string roleName , CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

         
            var roleEntity = await DbContext.Roles.Include(x=>x.Users).FirstOrDefaultAsync(x => x.Name.ToLower() == roleName.ToLower() && x.Users.Any(u => u.UserId == userId), cancellationToken);
            if (roleEntity != null)
            {
                var userToRemove = roleEntity.Users.FirstOrDefault(u => u.UserId == userId);
                roleEntity.Users.Remove(userToRemove);
            }

        }
    }
}
