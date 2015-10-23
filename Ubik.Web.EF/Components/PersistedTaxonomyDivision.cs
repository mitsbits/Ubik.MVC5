using System.Collections.Generic;

namespace Ubik.Web.EF.Components
{
    public class PersistedTaxonomyDivision
    {
        public PersistedTaxonomyDivision()
        {
            Elements = new HashSet<PersistedTaxonomyElement>();
        }
        public int Id { get; set; }
        public virtual ICollection<PersistedTaxonomyElement> Elements { get; set; }
        public int TextualId { get; set; }
        public virtual PersistedTextual Textual { get; set; }
    }
}