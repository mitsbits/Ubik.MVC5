using System.Collections.Generic;
using System.Threading.Tasks;
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

        Task Execute(RoleSaveModel model);

        Task Execute(NewUserSaveModel model);
        Task Execute(UserSaveModel model);
        IEnumerable<RoleViewModel> SystemRoleViewModels { get; }
        
    }
}