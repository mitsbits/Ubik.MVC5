using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mehdime.Entity;
using Ubik.EF;
using Ubik.Infra.Contracts;
using Ubik.Web.Components.Domain;

namespace Ubik.Web.EF.Components
{
    public class DeviceRepository : 
        ReadWriteRepository<Device<int>, 
        ComponentsDbContext> , IReadRepository<Device<int>>, IWriteRepository<Device<int>>
    {
        public DeviceRepository(IAmbientDbContextLocator ambientDbContextLocator) : base(ambientDbContextLocator)
        {
        }
    }
}
