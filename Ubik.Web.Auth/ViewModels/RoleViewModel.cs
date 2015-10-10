using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Ubik.Infra.Contracts;
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
        public bool IsSelected { get; set; }
    }

    public class RoleViewModelBuilder : IViewModelBuilder<ApplicationRole, RoleViewModel>
    {
        private readonly IResident _resident;

        public RoleViewModelBuilder(IResident resident)
        {
            _resident = resident;
        }

        public RoleViewModel CreateFrom(ApplicationRole entity)
        {
            if (entity == null) return new RoleViewModel();
            var viewModel = new RoleViewModel
            {
                RoleId = entity.Id,
                Name = entity.Name,
                Claims = entity.RoleClaims.Select(
                    x => new RoleClaimRowViewModel() { Type = x.ClaimType, Value = x.Value }).ToList()
            };

            return viewModel;
        }

        public void Rebuild(RoleViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.RoleId)) return;
            model.AvailableClaims =
                _resident.Security.Roles.SelectMany(x => _resident.Security.ClaimsForRole(x.Value))
                    .Select(x => new RoleClaimRowViewModel() { Type = x.Type, Value = x.Value })
                    .Distinct()
                    .ToArray();
            foreach (var roleClaimRowViewModel in model.AvailableClaims)
            {
                roleClaimRowViewModel.IsSelected =
                    model.Claims.Any(x => x.Type == roleClaimRowViewModel.Type
                        && x.Value == roleClaimRowViewModel.Value);
            }
        }
    }

    public class RoleViewModelCommand : IViewModelCommand<RoleSaveModel>
    {

        private readonly ApplicationRoleManager _roleManager;

        public RoleViewModelCommand(ApplicationRoleManager roleManager)
        {
        

            _roleManager = roleManager;
        }

        public async Task Execute(RoleSaveModel model)
        {
            var results = new List<IdentityResult>();
            var dbRole = await _roleManager.FindByIdAsync(model.RoleId);
            if (dbRole == null)
            {
                dbRole = new ApplicationRole() { Id = model.RoleId, Name = model.Name };
                results.Add(await _roleManager.CreateAsync(dbRole));
            }
            else
            {
                dbRole.Name = model.Name;
                results.Add(await _roleManager.UpdateAsync(dbRole));
            }

            dbRole.RoleClaims.Clear();
            results.Add(await _roleManager.ClearClaims(dbRole.Name));
            foreach (var claimViewModel in model.Claims)
            {
                dbRole.RoleClaims.Add(new ApplicationClaim(claimViewModel.Type, claimViewModel.Value));
            }
            results.Add(await _roleManager.UpdateAsync(dbRole));

            if (results.All(x => x.Succeeded)) return;
            throw new ApplicationException(string.Join("\n", results.SelectMany(x => x.Errors)));
        }
    }

    public class CopyRoleViewModel : RoleViewModel
    {
        [Required]
        public string Target { get; set; }

        public CopyRoleViewModel()
        {
        }

        public CopyRoleViewModel(RoleViewModel source)
        {
            AvailableClaims = source.AvailableClaims;
            Claims = source.Claims;
            IsPersisted = source.IsPersisted;
            IsSytemRole = source.IsSytemRole;
            Name = source.Name;
            RoleId = source.RoleId;
            IsSelected = source.IsSelected;
        }
    }
}