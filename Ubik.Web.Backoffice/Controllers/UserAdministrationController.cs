using System.Web.Mvc;
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

        [ActionName("new-user")]
        /*[AuthorizeOperationToResource(OperationKey = SystemClaims.Operations.Create, ResourceKey = UserAdministrationAuth.Resources.User)]*/
        public ActionResult NewUser()
        {
            SetContentPage(new BackofficeContent() { Title = "User Administration", Subtitle = "here you can create a new user" });
            var model = _userService.ViewModels.CreateUser(string.Empty);
            return View("NewUser", model);
        }

        #region Roles

        public ActionResult Roles(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return View(_userService.ViewModels.Roles());
            return View(_userService.ViewModels.CreateRoleByName(id));
        }

        [ActionName("new-role")]
        /*[AuthorizeOperationToResource(OperationKey = SystemClaims.Operations.Create, ResourceKey = UserAdministrationAuth.Resources.Role)]*/
        public ActionResult NewRole()
        {
            SetContentPage(new BackofficeContent() { Title = "Role Administration", Subtitle = "here you can create a new role" });
            var model = _userService.ViewModels.CreateRole(string.Empty);
            return View("NewRole", model);
        }

        [ValidateAntiForgeryToken]
        public ActionResult CreateRole(RoleViewModel model)
        {
            return Redirect("index");
        }

        #endregion Roles
    }
}