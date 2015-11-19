using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using Ubik.Infra;
using Ubik.Web.Auth;
using Ubik.Web.Backoffice.Contracts;
using Ubik.Web.Infra;
using Ubik.Web.Infra.Contracts;

namespace Ubik.Web.Backoffice.Controllers
{
    [AuthorizeRole(RoleName = SystemRoles.Backoffice)]
    [SessionState(SessionStateBehavior.Required)]
    public abstract class BackofficeController : Controller
    {

        private readonly IErrorLogManager _errorLogManager;

        protected BackofficeController(IErrorLogManager errorLogManager)
        {
            _errorLogManager = errorLogManager;
            
        }

        #region Pager

        private const string pageNumerVariableName = "p";
        private const string rowCountVariableName = "r";

        protected RequestPager Pager
        {
            get
            {
                var p = 1;
                if (Request.QueryString[pageNumerVariableName] != null)
                    int.TryParse(Request.QueryString[pageNumerVariableName], out p);
                var r = 10;
                if (Request.QueryString[rowCountVariableName] != null)
                    int.TryParse(Request.QueryString[rowCountVariableName], out r);
                return new RequestPager() { Current = p, RowCount = r };
            }
        }

        protected struct RequestPager
        {
            public int Current { get; set; }
            public int RowCount { get; set; }
        }

        #endregion Pager

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

        #region Redirect Messages

        protected void AddRedirectMessage(ServerResponseStatus status, string title, string message = "")
        {
            this.AddRedirectMessages(new ServerResponse(status, title, message));
        }

        protected void AddRedirectMessage(Exception exception)
        {
            _errorLogManager.LogException(exception);
            this.AddRedirectMessages(new ServerResponse(exception));
        }

        protected void AddRedirectMessage(ModelStateDictionary state)
        {
            foreach (var stm in state.Where(stm => stm.Value.Errors != null))
            {
                this.AddRedirectMessages(
                    stm.Value.Errors.Select(
                        e => new ServerResponse(ServerResponseStatus.ERROR, e.ErrorMessage, e.Exception.Message))
                        .ToArray());
            }
        }

        #endregion Redirect Messages
    }
}