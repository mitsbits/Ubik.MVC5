using System;
using System.Linq;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ModelBinding;
using Ubik.Infra;
using Ubik.Web.Basis;
using Ubik.Web.Basis.Contracts;
using Ubik.Web.Client.Backoffice.Contracts;
using Ubik.Web.Membership;

namespace Ubik.Web.Client.Backoffice.Controllers
{
    //[Authorize(Policy = "Over18")]
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

                if (!string.IsNullOrWhiteSpace(Request.Query[pageNumerVariableName]))
                    int.TryParse(Request.Query[pageNumerVariableName], out p);

                var r = 10;

                if (!string.IsNullOrWhiteSpace(Request.Query[rowCountVariableName]))
                    int.TryParse(Request.Query[rowCountVariableName], out r);
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

            return ViewBag;
        }

        #region Redirect Messages

        protected void AddRedirectMessage(ServerResponseStatus status, string title, string message = "")
        {
            this.AddRedirectMessages(new[] { new ServerResponse(status, title, message) });
        }

        protected void AddRedirectMessage(Exception exception)
        {
            _errorLogManager.LogException(exception);
            this.AddRedirectMessages(new[] { new ServerResponse(exception) });
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