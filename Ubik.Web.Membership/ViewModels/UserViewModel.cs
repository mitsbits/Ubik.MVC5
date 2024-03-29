﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Ubik.Infra.Contracts;
using Ubik.Web.BuildingBlocks.Contracts;
using Ubik.Web.Membership.Contracts;
using Ubik.Web.Membership.Managers;

namespace Ubik.Web.Membership.ViewModels
{
    #region Edit User

    public class UserSaveModel : IHasRoles
    {
        public bool IsTransient { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public RoleViewModel[] Roles { get; set; }

        public bool IsLockedOut { get; set; }
    }

    public class UserViewModel : UserSaveModel, IHasRolesToSelect
    {
        public RoleViewModel[] AvailableRoles { get; set; }
    }

    public class UserViewModelBuilder : IViewModelBuilder<ApplicationUser, UserViewModel>
    {
        private readonly IEnumerable<RoleViewModel> _roleViewModels;

        public UserViewModelBuilder(IEnumerable<RoleViewModel> roleViewModels)
        {
            _roleViewModels = roleViewModels;
        }

        public UserViewModel CreateFrom(ApplicationUser entity)
        {
            var viewModel = new UserViewModel()
            {
                UserId = entity.Id,
                UserName = entity.UserName,
                IsLockedOut = entity.LockoutEnabled
            };

            var entityRoles = entity.Roles.ToList();

            viewModel.Roles = entityRoles.Select(role => new RoleViewModel()
            {
                RoleId = role.RoleId,
                Name = _roleViewModels.Single(x => x.RoleId == role.RoleId).Name,
                Claims = _roleViewModels.Single(x => x.RoleId == role.RoleId).Claims,
                AvailableClaims = _roleViewModels.Single(x => x.RoleId == role.RoleId).AvailableClaims
            }).ToArray();

            return viewModel;
        }

        public void Rebuild(UserViewModel model)
        {
            foreach (var role in _roleViewModels)
            {
                role.IsSelected = model.Roles.Any(x => x.Name == role.Name);
                foreach (var claim in role.AvailableClaims)
                {
                    claim.IsSelected = role.Claims.Any(c => c.Type == claim.Type && c.Value == claim.Value);
                }
            }
            model.AvailableRoles = _roleViewModels.ToArray();
        }
    }

    public class UserViewModelCommand : IViewModelCommand<UserSaveModel>
    {
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationRoleManager _roleManager;
        private readonly IResident _resident;

        public UserViewModelCommand(ApplicationUserManager userManager, ApplicationRoleManager roleManager, IResident resident)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _resident = resident;
        }

        public async Task Execute(UserSaveModel model)
        {
            await SaveNonPersistedRoles(model);

            var existingRoles = _userManager.GetRoles(model.UserId);
            var results = new List<IdentityResult>
            {
                await
                    _userManager.RemoveFromRolesAsync(model.UserId,
                        existingRoles.ToArray()),
                _userManager.AddToRoles(model.UserId, model.Roles.Select(x => x.Name).ToArray())
            };

            if (results.All(x => x.Succeeded)) return;
            throw new ApplicationException(string.Join("\n", results.SelectMany(x => x.Errors)));
        }

        private async Task SaveNonPersistedRoles(IHasRoles model)
        {
            var results = new List<IdentityResult>();
            var exesistingRoleNames = await _roleManager.Roles.Select(x => x.Name).ToListAsync();
            foreach (var roleViewModel in model.Roles)
            {
                var viewModel = roleViewModel;

                if (!exesistingRoleNames.Contains(viewModel.Name))
                {
                    var role = new ApplicationRole(viewModel.Name);

                    foreach (var roleClaimRowViewModel in _resident.Security.ClaimsForRole(viewModel.Name))
                    {
                        role.RoleClaims.Add(new ApplicationClaim(roleClaimRowViewModel.Type, roleClaimRowViewModel.Value));
                    }
                    results.Add(await _roleManager.CreateAsync(role));
                }
            }

            if (results.All(x => x.Succeeded)) return;
            throw new ApplicationException(string.Join("\n", results.SelectMany(x => x.Errors)));
        }
    }

    #endregion Edit User

    #region Create User

    public class NewUserSaveModel : IHasRoles
    {
        public NewUserSaveModel()
        {
            Roles = new RoleViewModel[] { };
        }

        public string UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public RoleViewModel[] Roles { get; set; }
    }

    public class NewUserViewModel : NewUserSaveModel, IHasRolesToSelect
    {
        public RoleViewModel[] AvailableRoles { get; set; }
    }

    public class NewUserViewModelBuilder : IViewModelBuilder<ApplicationUser, NewUserViewModel>
    {
        private readonly IEnumerable<RoleViewModel> _roleViewModels;

        public NewUserViewModelBuilder(IEnumerable<RoleViewModel> roleViewModels)
        {
            _roleViewModels = roleViewModels;
        }

        public NewUserViewModel CreateFrom(ApplicationUser entity)
        {
            var viewModel = new NewUserViewModel()
            {
                UserId = entity.Id,
                UserName = entity.UserName,
                Email = entity.Email
            };
            return viewModel;
        }

        public void Rebuild(NewUserViewModel model)
        {
            model.AvailableRoles = _roleViewModels.ToArray();
            foreach (var role in model.AvailableRoles)
            {
                role.IsSelected = model.Roles.Any(x => x.Name == role.Name);
            }
        }
    }

    public class NewUserViewModelCommand : IViewModelCommand<NewUserSaveModel>
    {
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationRoleManager _roleManager;

        private readonly IResident _resident;

        public NewUserViewModelCommand(ApplicationUserManager userManager, ApplicationRoleManager roleManager, IResident resident)
        {
            _userManager = userManager;
            _roleManager = roleManager;

            _resident = resident;
        }

        public async Task Execute(NewUserSaveModel model)
        {
            var entity = new ApplicationUser() { Id = model.UserId, Email = model.UserName, UserName = model.UserName };
            await SaveNonPersistedRoles(model);
            var results = new List<IdentityResult>
            {
                await _userManager.CreateAsync(entity, model.Password),
                await _userManager.AddToRolesAsync(entity.Id, model.Roles.Select(x => x.Name).ToArray())
            };

            if (results.All(x => x.Succeeded)) return;
            throw new ApplicationException(string.Join("\n", results.SelectMany(x => x.Errors)));
        }

        private async Task SaveNonPersistedRoles(IHasRoles model)
        {
            var results = new List<IdentityResult>();
            var exesistingRoleNames = await _roleManager.Roles.Select(x => x.Name).ToListAsync();
            foreach (var roleViewModel in model.Roles)
            {
                var viewModel = roleViewModel;

                if (!exesistingRoleNames.Contains(viewModel.Name))
                {
                    var role = new ApplicationRole(viewModel.Name);

                    foreach (var roleClaimRowViewModel in _resident.Security.ClaimsForRole(viewModel.Name))
                    {
                        role.RoleClaims.Add(new ApplicationClaim(roleClaimRowViewModel.Type, roleClaimRowViewModel.Value));
                    }
                    results.Add(await _roleManager.CreateAsync(role));
                }
            }

            if (results.All(x => x.Succeeded)) return;
            throw new ApplicationException(string.Join("\n", results.SelectMany(x => x.Errors)));
        }
    }

    #endregion Create User

    #region Change Password

    public class UserChangPasswordViewModel
    {
        public string RedirectURL { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }

    public class UserLockedStateViewModel
    {
        public string RedirectURL { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public bool IsLocked { get; set; }
    }

    #endregion Change Password
}