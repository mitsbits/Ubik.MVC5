using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ubik.Web.Auth.Contracts;
using Ubik.Web.Backoffice.Contracts;

namespace Ubik.Web.Backoffice.Controllers
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
            SetContentPage(new BackofficeContent(){Title = "User Administration", Subtitle = "here you can manage memberships"});
            return View();

        }

        [ActionName("new-user")]
        public ActionResult NewUser()
        {
            SetContentPage(new BackofficeContent() { Title = "User Administration", Subtitle = "here you can create a new user" });
            var model = _userService.ViewModels.CreateUser(string.Empty);
            return View("NewUser",model);
        }


    }
}