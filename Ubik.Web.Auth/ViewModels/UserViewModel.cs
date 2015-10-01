using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ubik.Web.Auth.Contracts;
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

    public class UserViewModel : UserSaveModel
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
}