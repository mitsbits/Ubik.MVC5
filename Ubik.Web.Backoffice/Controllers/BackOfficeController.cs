using System.Web.Mvc;
using System.Web.SessionState;
using Ubik.Web.Backoffice.Contracts;

namespace Ubik.Web.Backoffice.Controllers
{
    /*[Authorize]*/[SessionState(SessionStateBehavior.Required)]
    public abstract class BackofficeController : Controller
    {
        private const string pageNumerVariableName = "p";
        private const string rowCountVariableName = "r";

        protected RequestPager Pager
        {
            get
            {
                var p = 1;
                if (ControllerContext.RouteData.Values[pageNumerVariableName] != null)
                    int.TryParse(ControllerContext.RouteData.Values[pageNumerVariableName].ToString(), out p);
                var r = 10;
                if (ControllerContext.RouteData.Values[rowCountVariableName] != null)
                    int.TryParse(ControllerContext.RouteData.Values[rowCountVariableName].ToString(), out r);
                return new RequestPager(){ Current = p, RowCount = r};
            }
        }

        protected void SetContentPage(IBackofficeContent content)
        {
            var viewBag = GetRootViewBag();
            viewBag.ContentInfo = content;
        }

        private dynamic GetRootViewBag()
        {
            var context = ControllerContext;
            while (context.IsChildAction)
            {
                context = context.ParentActionViewContext.Controller.ControllerContext;
            }
            return context.Controller.ViewBag;
        }

        protected struct RequestPager
        {
            public int Current { get; set; }
            public int RowCount { get; set; }
        }

   
    }
}