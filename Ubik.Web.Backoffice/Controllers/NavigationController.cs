using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ubik.Web.BackOffice.Controllers
{
    public class NavigationController : BackofficeController
    {
        public ActionResult LeftMenu()
        {
            return PartialView();
        }
    }
}