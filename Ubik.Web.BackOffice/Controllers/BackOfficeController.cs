using System.Web.Mvc;
using System.Web.SessionState;

namespace Ubik.Web.BackOffice.Controllers
{
    [Authorize][SessionState(SessionStateBehavior.Required)]
    public class BackOfficeController : Controller
    {
    
    }
}