using System.Collections.Generic;
using Ubik.Web.Auth.ViewModels;

namespace Ubik.Web.Auth.Contracts
{
    public interface IUserAdminstrationViewModelService
    {
        UserViewModel UserModel(string id);

        NewUserViewModel NewUserModel();

        RoleViewModel RoleModel(string id);

        RoleViewModel RoleByNameModel(string name);

        IEnumerable<UserRowViewModel> UserModels();

        IEnumerable<RoleViewModel> RoleModels();

        void Execute(RoleSaveModel model);

        void Execute(NewUserSaveModel model);

        IEnumerable<RoleViewModel> SystemRoleViewModels { get; }
    }
}