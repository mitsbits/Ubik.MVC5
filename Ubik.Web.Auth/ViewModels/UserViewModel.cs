using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Ubik.Web.Auth.Contracts;
using Ubik.Web.Auth;
using Ubik.Web.Auth.Managers;
using Ubik.Web.Cms.Contracts;

namespace Ubik.Web.Auth.ViewModels
{
    public class UserSaveModel
    {
        public bool IsTransient { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public RoleViewModel[] Roles { get; set; }
    }

    public class UserViewModel : UserSaveModel, IHasRolesToSelect
    {
        public RoleViewModel[] AvailableRoles { get; set; }
    }

    public class UserViewModelBuilder : IViewModelBuilder<ApplicationUser, UserViewModel>
    {

        private readonly IRoleRepository _roleRepo;



        private readonly IEnumerable<RoleViewModel> _roleViewModels;


        public UserViewModelBuilder(IRoleRepository roleRepository, IEnumerable<RoleViewModel> roleViewModels)
        {
            _roleRepo = roleRepository;
            _roleViewModels = roleViewModels;
        }

        public UserViewModel CreateFrom(ApplicationUser entity)
        {
            var viewModel = new UserViewModel()
            {
                UserId = entity.Id,
                UserName = entity.UserName
            };
            var userRoles = _roleRepo.Find(x => x.Users.Any(u => u.UserId == entity.Id), x => x.Name);
            var entityRoles = entity.Roles.ToList();

            viewModel.Roles = entityRoles.Select(role => new RoleViewModel()
            {
                RoleId = role.RoleId,
                Name = userRoles.Single(r => r.Id == role.RoleId).Name
            }).ToArray();

            return viewModel;
        }

        public void Rebuild(UserViewModel model)
        {
            model.AvailableRoles = _roleViewModels.ToArray();
            foreach (var role in model.AvailableRoles)
            {
                role.IsSelected = model.Roles.Any(x => x.Name == role.Name);
            }

        }
    }

    internal class UserViewModelCommand : IViewModelCommand<UserSaveModel>
    {
        public void Execute(UserSaveModel model)
        {
            return;
        }
    }

    #region Create User

    public class NewUserSaveModel
    {
        public NewUserSaveModel()
        {
            Roles = new RoleViewModel[] { };
        }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

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
        private readonly IRoleRepository _roleRepo;
        private readonly IResident _resident;
        public NewUserViewModelCommand(ApplicationUserManager userManager, ApplicationRoleManager roleManager, IRoleRepository roleRepo, IResident resident)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepo = roleRepo;
            _resident = resident;
        }

        public void Execute(NewUserSaveModel model)
        {
            var entity = new ApplicationUser() { Id = model.UserId, Email = model.UserName, UserName = model.UserName };
            SaveNonPersistedRoles(model);
            var result = _userManager.CreateAsync(entity, model.Password).Result;
            if (result.Succeeded) return;
            throw new ApplicationException(string.Join("\n", result.Errors));
        }

        private void SaveNonPersistedRoles(NewUserSaveModel model)
        {
            var tasks = new List<Task<IdentityResult>>();
            foreach (var roleViewModel in model.Roles)
            {
                var viewModel = roleViewModel;
                if (!_roleManager.Roles.Any(x => x.Name == viewModel.Name))
                {
                    var role = new ApplicationRole(viewModel.Name);

                    foreach (var roleClaimRowViewModel in _resident.Security.ClaimsForRole(viewModel.Name))
                    {
                        role.RoleClaims.Add(new ApplicationClaim(roleClaimRowViewModel.Type, roleClaimRowViewModel.Value));
                    }
                    tasks.Add(_roleManager.CreateAsync(role));
                }
            }
            var result = Task.WhenAll(tasks.ToArray()).Result;
            if (result.All(x => x.Succeeded)) return;
            throw new ApplicationException(string.Join("\n", result.SelectMany(x => x.Errors)));

        }
    }
    #endregion


    public interface IHasRolesToSelect
    {
        RoleViewModel[] AvailableRoles { get; }
    }
}