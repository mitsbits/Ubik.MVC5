using System.Collections.Generic;
using Ubik.Web.Auth.ViewModels;

namespace Ubik.Web.Auth.Contracts
{
    public interface IUserAdminstrationViewModelService
    {
        UserViewModel UserModel(string id);

        RoleViewModel RoleModel(string id);

        RoleViewModel RoleByNameModel(string name);

        IEnumerable<UserRowViewModel> Users();

        IEnumerable<RoleRowViewModel> Roles();

        void Execute(RoleViewModel model);

        //RoleViewModel RoleById(string id);

        //RoleViewModel RoleByName(string id);
    }
}