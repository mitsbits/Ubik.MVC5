using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Ubik.Web.Auth.Contracts;
using Ubik.Web.Auth.ViewModels;

namespace Ubik.Web.Backoffice.Controllers
{
    public class UserAdministrationController : BackofficeController
    {
        private readonly IUserAdminstrationService _userService;
        private readonly IUserAdminstrationViewModelService _viewModelsService;

        public UserAdministrationController(IUserAdminstrationService userService, IUserAdminstrationViewModelService viewModelsService)
        {
            _userService = userService;
            _viewModelsService = viewModelsService;
        }

        protected IUserAdminstrationService UserService
        {
            get { return _userService; }
        }

        protected IUserAdminstrationViewModelService ViewModelsService
        {
            get { return _viewModelsService; }
        }

        public ActionResult Index()
        {
            SetContentPage(new BackofficeContent() { Title = "User Administration", Subtitle = "here you can manage memberships" });
            return View();
        }

        #region Users

        public ActionResult Users(string id)
        {
            return string.IsNullOrWhiteSpace(id) ? GetAllUsers() : GetOneUserById(id);
        }

        [ActionName("new-user")]
        /*[AuthorizeOperationToResource(OperationKey = SystemClaims.Operations.Create, ResourceKey = UserAdministrationAuth.Resources.User)]*/
        public ActionResult NewUser()
        {
            SetContentPage(new BackofficeContent() { Title = "User Administration", Subtitle = "here you can create a new user" });
            var model = ViewModelsService.NewUserModel();
            return View("NewUser", model);
        }

        public ActionResult CreateUser(NewUserViewModel model)
        {
            model.Email = model.UserName;
            if (!ModelState.IsValid) return View("NewUser", model);
            model.Roles = model.AvailableRoles.Where(x => x.IsSelected).ToArray();
            ViewModelsService.Execute(model);
            return RedirectToAction("Users", "UserAdministration", new { id = model.UserId });
        }

        public async Task<ActionResult> LockUser(string id)
        {
            await _userService.LockUser(id, 5);
            return RedirectToAction("Users", "UserAdministration");
        }

        public async Task<ActionResult> UnlockUser(string id)
        {
            await _userService.UnockUser(id);
            return RedirectToAction("Users", "UserAdministration");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangUserPassword(UserChangPasswordViewModel model)
        {
            if (ModelState.IsValid)
                await _userService.SetPassword(model.UserId, model.NewPassword);
            return Redirect(model.RedirectURL);
        }

        private ActionResult GetOneUserById(string id)
        {
            var model = _viewModelsService.UserModel(id);
            SetContentPage(new BackofficeContent() { Title = "Manage: " + model.UserName, Subtitle = "here you can manage this user" });
            return View(model);
        }

        private ActionResult GetNoUser()
        {
            var content = new BackofficeContent() { Title = "User Administration", Subtitle = "here you can manage users" };
            SetContentPage(content);
            return View(new List<UserRowViewModel>());
        }

        private ActionResult GetAllUsers()
        {
            SetContentPage(new BackofficeContent() { Title = "User Administration", Subtitle = "here you can manage users" });
            return View(_viewModelsService.Users());
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
            var model = ViewModelsService.RoleModel(string.Empty);
            return View("NewRole", model);
        }

        [ValidateAntiForgeryToken]
        public ActionResult UpdateRole(RoleViewModel model)
        {
            if (!ModelState.IsValid) return View("NewRole", model);
            var currentClaims = model.AvailableClaims.Where(x => x.IsSelected).ToList();
            model.Claims = new List<RoleClaimRowViewModel>(currentClaims);
            ViewModelsService.Execute(model);
            return RedirectToAction("Roles", "UserAdministration", new { id = model.Name });
        }

        [ValidateAntiForgeryToken]
        public ActionResult CopyRole(CopyRoleViewModel model)
        {
            if (!ModelState.IsValid) return View("~/Areas/Backoffice/Views/UserAdministration/Roles.cshtml", model);
            UserService.CopyRole(model.Name, model.Target);
            return RedirectToAction("Roles", "UserAdministration", new { id = model.Target });
        }

        public ActionResult DeleteRole(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                UserService.DeleteRole(id);
            }
            return RedirectToAction("Roles", "UserAdministration");
        }

        private ActionResult GetOneRoleByName(string id)
        {
            var model = ViewModelsService.RoleByNameModel(id);
            var content = new BackofficeContent { Title = model.Name, Subtitle = "role management" };
            SetContentPage(content);
            return View(model);
        }

        private ActionResult GetAllRoles()
        {
            var content = new BackofficeContent() { Title = "User Administration", Subtitle = "here you can manage roles" };
            SetContentPage(content);
            return View(ViewModelsService.Roles());
        }

        #endregion Roles
    }
}