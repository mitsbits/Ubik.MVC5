using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Mehdime.Entity;
using Ubik.EF;
using Ubik.Infra.Contracts;
using Ubik.Web.EF.Components.Contracts;

namespace Ubik.Web.EF.Components
{
    public class PersistedTaxonomyElementRepository : ReadWriteRepository<PersistedTaxonomyElement, ComponentsDbContext>, IPersistedTaxonomyElementRepository
    {
        public PersistedTaxonomyElementRepository(IAmbientDbContextLocator ambientDbContextLocator)
            : base(ambientDbContextLocator)
        {
        }

        public virtual async Task<IEnumerable<PersistedTaxonomyElement>> ElementsFragment(int parentId)
        {
            return await DbContext.TaxonomyElements
                .Include("Textual")
                .Include("Division")
                .Include("Division.Textual")
                .Include("TaxonomyElementRecursions")
                .Where(x => x.TaxonomyElementRecursions.All(r => r.AncestorId == parentId))
                .ToListAsync();
        }

        public virtual async Task<IEnumerable<PersistedTaxonomyElement>> ElementsForDivisionFragment(int divisionId)
        {
            return await DbContext.TaxonomyElements
                .Include("Textual")
                .Include("Division")
                .Include("Division.Textual")
                .Include("TaxonomyElementRecursions")
                .Where(x => x.DivisionId == divisionId)
                .ToListAsync();
        }
    }
}