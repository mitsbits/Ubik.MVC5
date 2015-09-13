using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mehdime.Entity;
using Ubik.EF;
using Ubik.Infra.Contracts;
using Ubik.Web.Components.Contracts;
using Ubik.Web.Components.Domain;

namespace Ubik.Web.EF.Components
{
    public class DeviceRepository : IDeviceRepository<int>
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IReadRepository<PersistedDevice> _readRepo; 

        public DeviceRepository(IDbContextScopeFactory dbContextScopeFactory, IReadRepository<PersistedDevice> readRepo)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
            _readRepo = readRepo;
        }

        public Device<int> Get(int id)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                var db = _readRepo.GetQuery();
                var hit = db.Include(x => x.Sections).Include("Section.Slots").FirstOrDefault(x => x.Id == id);
                return Mapper.MapToDomain(hit);
            }
        }
    }
}
