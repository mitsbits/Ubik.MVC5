using System;
using System.Linq;
using System.Linq.Expressions;
using Ubik.Web.Auth.Contracts;
using Ubik.Web.Cms.Contracts;

namespace Ubik.Web.Auth.ViewModels
{
    public class AddUserSaveModel
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public RoleRowViewModel[] RolesToAdd { get; set; }
    }

    public class AddUserViewModel : AddUserSaveModel
    {
        public RoleRowViewModel[] AvailableRoles { get; set; }
    }

    public class AddUserViewModelBuilder : IViewModelBuilder<ApplicationUser, AddUserViewModel>
    {
        private readonly IResident _resident;
        private readonly IRoleRepository _roleRepo;

        public AddUserViewModelBuilder(IResident resident, IRoleRepository roleRepo)
        {
            _resident = resident;
            _roleRepo = roleRepo;
        }

        public AddUserViewModel CreateFrom(ApplicationUser entity)
        {
            var viewModel = new AddUserViewModel()
            {
                UserName = string.Empty,
                Email = string.Empty,
                Password = string.Empty,
                RolesToAdd = new RoleRowViewModel[] { },
            };
            return viewModel;
        }

        public void Rebuild(AddUserViewModel model)
        {
            Expression<Func<ApplicationRole, bool>> predicate = role => true;
            var databaseRoles = _roleRepo.Find(predicate, x => x.Name);//TODO: system roles must be populated from database at this point, no need to hit the database here
            model.AvailableRoles = _resident.Security.SystemRoles.Select(systemRole => new RoleRowViewModel() { RoleId = databaseRoles.Single(x => x.Name.Equals(systemRole.Value, StringComparison.InvariantCultureIgnoreCase)).Id, Name = systemRole.Value }).ToArray();
        }
    }
}