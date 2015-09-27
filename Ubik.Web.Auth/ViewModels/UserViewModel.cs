using System;
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

        public RoleRowViewModel[] Roles { get; set; }
    }

    public class UserViewModel : UserSaveModel
    {
        public RoleRowViewModel[] AvailableRoles { get; set; }
    }

    public class UserViewModelBuilder : IViewModelBuilder<ApplicationUser, UserViewModel>
    {
        private readonly IResident _resident;
        private readonly IRoleRepository _roleRepo;

        public UserViewModelBuilder(IResident resident, IRoleRepository roleRepository)
        {
            _resident = resident;
            _roleRepo = roleRepository;
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

            viewModel.Roles = entityRoles.Select(role => new RoleRowViewModel()
            {
                RoleId = role.RoleId,
                Name = userRoles.Single(r => r.Id == role.RoleId).Name
            }).ToArray();

            return viewModel;
        }

        public void Rebuild(UserViewModel model)
        {
            Expression<Func<ApplicationRole, bool>> predicate = role => true;
            var databaseRoles = _roleRepo.Find(predicate, x => x.Name);//TODO: system roles must be populated from database at this point, no need to hit the database here
            model.AvailableRoles = _resident.Security.Roles.Select(systemRole => new RoleRowViewModel() { RoleId = databaseRoles.Single(x => x.Name.Equals(systemRole.Value, StringComparison.InvariantCultureIgnoreCase)).Id, Name = systemRole.Value }).ToArray();
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