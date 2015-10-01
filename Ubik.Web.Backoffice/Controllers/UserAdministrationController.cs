using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Ubik.Web.Auth.Contracts;
using Ubik.Web.Auth.ViewModels;

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
            SetContentPage(new BackofficeContent() { Title = "User Administration", Subtitle = "here you can manage memberships" });
            return View();
        }

        #region Users   


        [ActionName("new-user")]
        /*[AuthorizeOperationToResource(OperationKey = SystemClaims.Operations.Create, ResourceKey = UserAdministrationAuth.Resources.User)]*/
        public ActionResult NewUser()
        {
            SetContentPage(new BackofficeContent() { Title = "User Administration", Subtitle = "here you can create a new user" });
            var model = _userService.ViewModels.UserModel(string.Empty);
            return View("NewUser", model);
        }

        #endregion Users

        #region Roles

        public ActionResult Roles(string id)
        {
            return string.IsNullOrWhiteSpace(id) ? GetAllRoles() : GetOneRoleByName(id);
        }



        [ActionName("new-role")]
        /*[AuthorizeOperationToResource(OperationKey = SystemClaims.Operations.Create, ResourceKey = UserAdministrationAuth.Resources.Role)]*/
        public ActionResult NewRole()
        {
            SetContentPage(new BackofficeContent() { Title = "Role Administration", Subtitle = "here you can create a new role" });
            var model = _userService.ViewModels.RoleModel(string.Empty);
            return View("NewRole", model);
        }

        [ValidateAntiForgeryToken]
        public ActionResult UpdateRole(RoleViewModel model)
        {
            if (!ModelState.IsValid) return View("New-Role", model);
            var currentClaims = model.AvailableClaims.Where(x => x.IsSelected).ToList();
            model.Claims = new List<RoleClaimRowViewModel>(currentClaims);
            _userService.ViewModels.Execute(model);
            return RedirectToAction("Roles", "UserAdministration", new {id = model.Name});
        }

        [ValidateAntiForgeryToken]
        public ActionResult CopyRole(CopyRoleViewModel model)
        {
            if (!ModelState.IsValid) return View("~/Areas/Backoffice/Views/UserAdministration/Roles.cshtml", model);
             _userService.CopyRole(model.Name, model.Target);
            return RedirectToAction("Roles", "UserAdministration", new {id = model.Target});
        }

        private ActionResult GetOneRoleByName(string id)
        {
            var model = _userService.ViewModels.RoleByNameModel(id);
            var content = new BackofficeContent { Title = model.Name, Subtitle = "role management" };
            SetContentPage(content);
            return View(model);
        }

        private ActionResult GetAllRoles()
        {
            var content = new BackofficeContent() { Title = "User Administration", Subtitle = "here you can manage roles" };
            SetContentPage(content);
            return View(_userService.ViewModels.Roles());
        }
        #endregion Roles
    }
}