using System.Collections.Generic;
using Ubik.Web.Components.Contracts;

namespace Ubik.Web.Components.Domain
{
    public class Content<TKey> : ComponentBase<TKey>, IContent
    {
        protected Content() 
        {
            Metas = new HashSet<Meta>();
        }

        public Content(TKey id, Textual textual, string canonicalUrl)
            : base(id)
        {
            Textual = textual;
            CanonicalURL = canonicalUrl;
        }


        ITextualInfo IContent.Textual
        {
            get { return Textual; }
        }

        public Textual Textual { get; private set; }

        public string CanonicalURL
        {
            get;
            private set;
        }

        IEnumerable<IHtmlMeta> IContent.Metas
        {
            get { return Metas; }
        }

        public ICollection<Meta> Metas
        {
            get;
            private set;
        }
    }
}