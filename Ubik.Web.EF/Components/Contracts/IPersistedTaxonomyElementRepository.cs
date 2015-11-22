using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ubik.Infra.Contracts;

namespace Ubik.Web.EF.Components.Contracts
{
    public interface IPersistedTaxonomyElementRepository : ICRUDRespoditory<PersistedTaxonomyElement>
    {
        Task<IEnumerable<PersistedTaxonomyElement>> ElementsFragment(int parentId);
        Task<IEnumerable<PersistedTaxonomyElement>> ElementsForDivisionFragment(int divisionId);
    }
}
