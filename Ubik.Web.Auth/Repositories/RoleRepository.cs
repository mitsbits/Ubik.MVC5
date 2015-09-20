using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Ubik.EF;
using Ubik.Web.Auth.Contracts;

namespace Ubik.Web.Auth.Repositories
{
    public class RoleRepository : ReadWriteRepository<ApplicationRole, AuthDbContext>, IRoleRepository
    {
        public RoleRepository(IAmbientDbContextLocator ambientDbContextLocator)
            : base(ambientDbContextLocator)
        {
        }

        public override IEnumerable<ApplicationRole> Find(Expression<Func<ApplicationRole, bool>> predicate, Func<ApplicationRole, object> orderby)
        {
            return DbContext.Set<ApplicationRole>().Include(x => x.RoleClaims).Where(predicate).OrderBy(orderby).ToList();
        }

        public override ApplicationRole Get(Expression<Func<ApplicationRole, bool>> predicate)
        {
            return DbContext.Set<ApplicationRole>().Include(x => x.RoleClaims).FirstOrDefault(predicate);
        }
    }
}