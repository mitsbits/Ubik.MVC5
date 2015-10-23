namespace Ubik.Web.EF.Components
{
    public class PersistedTaxonomyElement : PersistedComponent
    {
        public int ParentId { get; set; }
        public int Depth { get; set; }
        public int DivisionId { get; set; }
        public virtual PersistedTaxonomyDivision Division { get; set; }
        public int TextualId { get; set; }
        public virtual PersistedTextual Textual { get; set; }
    }
}