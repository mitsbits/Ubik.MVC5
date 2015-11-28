using Microsoft.AspNet.Mvc;

namespace Ubik.Web.Client.Backoffice.Controllers
{
    public class MailboxController : Controller
    {
        public ActionResult Mailbox()
        {
            return View();
        }
    }
}