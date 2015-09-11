using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ubik.Infra.Contracts;
using Ubik.Web.Components.Domain;

namespace Ubik.Web.Components.Contracts
{
    internal interface IDeviceRepository<TKey> : IReadRepository<Device<TKey>>, IWriteRepository<Device<TKey>>
    {
    }
}
