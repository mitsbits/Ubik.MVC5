using Mehdime.Entity;
using Ubik.EF;
using Ubik.Infra.Contracts;

namespace Ubik.Web.EF.Components
{
    public class PersistedTaxonomyElementRepository : ReadWriteRepository<PersistedTaxonomyElement, ComponentsDbContext>, ICRUDRespoditory<PersistedTaxonomyElement>
    {
        public PersistedTaxonomyElementRepository(IAmbientDbContextLocator ambientDbContextLocator) : base(ambientDbContextLocator)
        {
        }
    }
}