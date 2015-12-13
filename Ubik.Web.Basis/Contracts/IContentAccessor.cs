using Microsoft.AspNet.Mvc.ViewFeatures.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Rendering;

namespace Ubik.Web.Basis.Contracts
{
    public interface IContentAccessor
    {
        IPageContent Current { get; }
    }



    public class ContentAccessor : IContentAccessor, ICanHasViewContext
    {

        private ViewContext _viewContext;
        public ContentAccessor()
        {

        }


        private IPageContent _current;

        public IPageContent Current
        {
            get
            {
                var page = _current ?? (_current = (_viewContext.ViewBag.ContentInfo as IPageContent));
                if (page != null) return page;
                page = Default();
                _viewContext.ViewBag.ContentInfo = page;
                return page;
            }
        }

        private IPageContent Default()
        {
            var page = new PageContent() { Title = "Ubik 1.0" };

            return page;
        }

        public void Contextualize(ViewContext viewContext)
        {
            _viewContext = viewContext;
        }
    }

    public interface IPageContent
    {
        string Title { get; }
        string Subtitle { get; }
    }

    public class PageContent : IPageContent
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
    }
}
