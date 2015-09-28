using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Ubik.Web.Auth.Contracts;
using Ubik.Web.Auth.Managers;
using Ubik.Web.Cms.Contracts;

namespace Ubik.Web.Auth.ViewModels
{
    public class RoleSaveModel
    {
        public string RoleId { get; set; }

        [Required]
        public string Name { get; set; }

        public IEnumerable<RoleClaimRowViewModel> Claims { get; set; }
    }

    public class RoleViewModel : RoleSaveModel, ISelectable
    {
        public RoleClaimRowViewModel[] AvailableClaims { get; set; }
        public bool IsSytemRole { get; set; }
        public bool IsPersisted { get; set; }
        public bool Selected { get; set; }
    }

    public class RoleViewModelBuilder : IViewModelBuilder<ApplicationRole, RoleViewModel>
    {
        private readonly IResident _resident;
        private readonly IRoleRepository _roleRepo;
        private readonly IUserRepository _userRepo;

        public RoleViewModelBuilder(IResident resident, IRoleRepository roleRepository, IUserRepository userRepository)
        {
            _resident = resident;
            _roleRepo = roleRepository;
            _userRepo = userRepository;
        }

        public RoleViewModel CreateFrom(ApplicationRole entity)
        {
            if (entity == null) return new RoleViewModel();
            var viewModel = new RoleViewModel
            {
                RoleId = entity.Id,
                Name = entity.Name,
                Claims = entity.RoleClaims.Select(
                    x => new RoleClaimRowViewModel() { ClaimId = "", Type = x.ClaimType, Value = x.Value }).ToList()
            };

            return viewModel;
        }

        public void Rebuild(RoleViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.RoleId)) return;
            model.AvailableClaims =
                _resident.Security.Roles.SelectMany(x => _resident.Security.ClaimsForRole(x.Value))
                    .Select(x => new RoleClaimRowViewModel() { ClaimId = "", Type = x.Type, Value = x.Value })
                    .Distinct()
                    .ToArray();
            foreach (var roleClaimRowViewModel in model.AvailableClaims)
            {
                roleClaimRowViewModel.Selected =
                    model.Claims.Any(x => x.Type == roleClaimRowViewModel.Type
                        && x.Value == roleClaimRowViewModel.Value);
            }
        }
    }

    public class RoleViewModelCommand : IViewModelCommand<RoleSaveModel>
    {
        private readonly IRoleRepository _roleRepo;
        private readonly ApplicationRoleManager _roleManager;

        public RoleViewModelCommand(IRoleRepository roleRepo, ApplicationRoleManager roleManager)
        {
            _roleRepo = roleRepo;

            _roleManager = roleManager;
        }

        public void Execute(RoleSaveModel model)
        {
            IdentityResult result;
            var dbRole = _roleManager.FindByIdAsync(model.RoleId).Result;
            if (dbRole == null)
            {
                var appRole = new ApplicationRole() { Id = model.RoleId, Name = model.Name };
                result = _roleManager.CreateAsync(appRole).Result;
            }
            else
            {
                dbRole.Name = model.Name;
                result = _roleManager.UpdateAsync(dbRole).Result;
            }
            if (!result.Succeeded) return;

            var entityRole = _roleRepo.Get(x => x.Id == model.RoleId);
            entityRole.RoleClaims.Clear();
            foreach (var claimViewModel in model.Claims)
            {
                entityRole.RoleClaims.Add(new ApplicationClaim(claimViewModel.Type, claimViewModel.Value));
            }
        }
    }
}