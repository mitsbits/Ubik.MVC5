﻿using System.Collections.Generic;
using Ubik.Web.Auth.ViewModels;

namespace Ubik.Web.Auth.Contracts
{
    public interface IUserAdminstrationViewModelService
    {
        UserViewModel UserModel(string id);

        NewUserViewModel NewUserModel();

        RoleViewModel RoleModel(string id);

        RoleViewModel RoleByNameModel(string name);

        IEnumerable<UserRowViewModel> Users();

        IEnumerable<RoleViewModel> Roles();

        void Execute(RoleViewModel model);

        void Execute(NewUserViewModel model);

        IEnumerable<RoleViewModel> SystemRoleViewModels { get; }
    }
}