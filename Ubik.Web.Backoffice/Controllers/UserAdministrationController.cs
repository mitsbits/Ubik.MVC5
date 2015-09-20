using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ubik.Web.Auth.Contracts;
using Ubik.Web.BackOffice.Contracts;

namespace Ubik.Web.BackOffice.Controllers
{
    public class UserAdministrationController : BackofficeController
    {
        private readonly IUserAdminstrationService _userService;

        public UserAdministrationController(IUserAdminstrationService userService)
        {
            _userService = userService;
        }

        protected IUserAdminstrationService UserService
        {
            get { return _userService; }
        }

        public ActionResult Index()
        {
            SetContentPage(new BackofficeContent(){Title = "User Administration"});
            return View();

        }



    }
}