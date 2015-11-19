using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Ubik.Infra;
using Ubik.Web.Backoffice.ViewModel;
using Ubik.Web.Infra.Contracts;

namespace Ubik.Web.Backoffice.Controllers
{
    public class ErrorLogsController : BackofficeController
    {
        private readonly IErrorLogManager _manager;

        public ErrorLogsController(IErrorLogManager manager)   :base(manager)
        {
            _manager = manager;
        }

        public async Task<ActionResult> Index()
        {
            var model = await _manager.GetLogs(Pager.Current, Pager.RowCount);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteErrorLog(DeleteErrorLogViewModel model)
        {
            await _manager.ClearLog(model.ErrorId.ToString());
            return Redirect(model.RedirectUrl);
        }

        [HttpPost]
        public async Task<ActionResult> ClearLogsForRange(DeleteErrorLogRangeViewModel model)
        {
            try
            {
                var rowsDeleted = await _manager.ClearLogs(model.RangeStart, model.RangeEnd);
                AddRedirectMessage(ServerResponseStatus.SUCCESS, string.Format("{0} logs deleted!", rowsDeleted));
                return Redirect(model.RedirectUrl);
            }
            catch (Exception ex)
            {
                AddRedirectMessage(ex);
                return RedirectToAction("Index", "ErrorLogs", null);
            }
        }
    }
}