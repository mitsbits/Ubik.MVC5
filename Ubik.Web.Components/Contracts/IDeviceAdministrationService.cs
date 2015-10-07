

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Ubik.Web.Components.Domain;

namespace Ubik.Web.Components.Contracts
{

    public interface IDeviceAdministrationService<TKey> {

        IEnumerable<Device<TKey>> Find(Expression<Func<Device<TKey>, bool>> predicate, int pageNumber, int pageSize, out int totalRecords);
        Device<TKey> Get(TKey id);



    }

}