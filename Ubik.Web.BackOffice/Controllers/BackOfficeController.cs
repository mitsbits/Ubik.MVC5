using System.Web.Mvc;
using System.Web.SessionState;
using Ubik.Web.BackOffice.Contracts;

namespace Ubik.Web.BackOffice.Controllers
{
    /*[Authorize]*/[SessionState(SessionStateBehavior.Required)]
    public abstract class BackofficeController : Controller
    {
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
    }
}