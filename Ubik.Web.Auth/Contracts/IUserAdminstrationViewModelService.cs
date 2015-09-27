using System.Collections.Generic;
using Ubik.Web.Auth.ViewModels;

namespace Ubik.Web.Auth.Contracts
{
    public interface IUserAdminstrationViewModelService
    {
        UserViewModel CreateUser(string id);

        RoleViewModel CreateRole(string id);

        RoleViewModel CreateRoleByName(string name);

        IEnumerable<UserRowViewModel> Users();

        IEnumerable<RoleRowViewModel> Roles();

        //RoleViewModel RoleById(string id);

        //RoleViewModel RoleByName(string id);
    }
}