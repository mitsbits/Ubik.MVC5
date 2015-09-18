using Ubik.Web.Components.Contracts;

namespace Ubik.Web.Components.Domain
{
    public class Content<TKey> : ComponentBase<TKey>, IContent
    {
        protected Content()
        {
        }

        public Content(TKey id, Textual textual, string canonicalUrl)
            : base(id)
        {
            Textual = textual;
            BrowserAddress = new BrowserAddress(canonicalUrl);
        }

        ITextualInfo IContent.Textual
        {
            get { return Textual; }
        }

        public Textual Textual { get; private set; }

        IBrowserAddress IContent.BrowserAddress
        {
            get { return BrowserAddress; }
        }

        public BrowserAddress BrowserAddress { get; private set; }
    }
}