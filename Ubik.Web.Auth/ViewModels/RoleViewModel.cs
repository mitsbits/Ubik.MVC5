using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using Ubik.Web.Auth.Contracts;
using Ubik.Web.Cms.Contracts;

namespace Ubik.Web.Auth.ViewModels
{
    public class RoleSaveModel
    {
        public string RoleId { get; set; }

        [Required]
        public string RoleName { get; set; }

        public IEnumerable<RoleClaimRowViewModel> Claims { get; set; }

        public IEnumerable<UserRowViewModel> Users { get; set; }
    }

    public class RoleViewModel : RoleSaveModel
    {
        public RoleClaimRowViewModel[] AvailableClaims { get; set; }
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
                RoleName = entity.Name,
                Claims = entity.RoleClaims.Select(
                    x => new RoleClaimRowViewModel() {ClaimId = "", Type = x.ClaimType, Value = x.Value}).ToList()
            };

            Expression<Func<ApplicationUser, bool>> userPredicate = user => user.Roles.Any(x => x.RoleId == entity.Id);
            viewModel.Users =
                _userRepo.Find(userPredicate, user => user.UserName)
                    .Select(
                        x =>
                            new UserRowViewModel()
                            {
                                UserId = x.Id,
                                UserName = x.UserName,
                                Roles = new[] { new RoleRowViewModel() { Name = entity.Name, RoleId = entity.Id } }
                            })
                    .Distinct()
                    .ToList();

            return viewModel;
        }

        public void Rebuild(RoleViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.RoleId)) return;
            model.AvailableClaims =
                _resident.Security.Roles.SelectMany(x => _resident.Security.ClaimsForRole(x.Value))
                    .Select(x => new RoleClaimRowViewModel() {ClaimId = "", Type = x.Type, Value = x.Value})
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

    internal class RoleViewModelCommand : IViewModelCommand<RoleSaveModel>
    {
        public void Execute(RoleSaveModel model)
        {
            return;
        }
    }
}