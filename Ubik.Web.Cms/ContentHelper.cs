using System.Web.Mvc;
using Ubik.Web.Cms.Contracts;
using Ubik.Web.Components.Contracts;

namespace Ubik.Web.Cms
{
    public class ContentHelper : BasePageHelper, IContentPageProvider
    {
        public ContentHelper(ViewContext viewContext)
            : base(viewContext)
        {
        }

        private IContent _content;

        public IContent Current
        {
            get { return _content ?? (_content = RootViewBag.Content as IContent); }
        }


    }
}