

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ubik.Infra.DataManagement;
using Ubik.Web.Components.Domain;

namespace Ubik.Web.Components.Contracts
{

    public interface IDeviceAdministrationService<TKey>
    {

        Task<PagedResult<Device<TKey>>> All(int pageNumber, int pageSize);
        Task<Device<TKey>> Get(TKey id);

    }

}