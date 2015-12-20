using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
