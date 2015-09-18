using System.Collections.Generic;

namespace Ubik.Web.Components.Contracts
{
    public interface IBrowserAddress
    {
        string CanonicalURL { get; }
        IEnumerable<IHtmlMeta> Metas { get; }
        string Slug { get; }
    }
}