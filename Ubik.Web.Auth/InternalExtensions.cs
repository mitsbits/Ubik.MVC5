using System.Collections.Generic;
using System.Linq;
using Ubik.Web.Auth.Contracts;
using Ubik.Web.Auth.ViewModels;

namespace Ubik.Web.Auth
{
    internal static class InternalExt
    {
        public static IEnumerable<RoleViewModel> RoleModels(this IEnumerable<IResourceAuthProvider> authProviders)
        {
            var resourceAuthProviders  = authProviders as IResourceAuthProvider[] ?? authProviders.ToArray();
            var systemRoleNames = resourceAuthProviders.SelectMany(x => x.RoleNames).Distinct();
            var systemRoleViewModels = new List<RoleViewModel>();
            foreach (var roles in systemRoleNames.Select(name => resourceAuthProviders.Select(x => new RoleViewModel()
            {
                Name = name,
                RoleId = "",
                IsSytemRole = true,
                IsPersisted = false,
                Claims = x.Claims(name).Select(systemClaim => new RoleClaimRowViewModel()
                {
                    ClaimId = "",
                    Type = systemClaim.Type,
                    Value = systemClaim.Value
                })
            })))
            {
                systemRoleViewModels.AddRange(roles);
            }
            return systemRoleViewModels;
        }


        public static IEnumerable<RoleViewModel> RoleModelsCheckDB(
            this IEnumerable<IResourceAuthProvider> authProviders, IRoleRepository roleRepo)
        {
            var systemRoleViewModels = authProviders.RoleModels().ToList();
            var roleViewModels = new List<RoleViewModel>(systemRoleViewModels);

            var dbRoles = roleRepo.Find(x => true, role => role.Name).ToList();

            roleViewModels.AddRange(from applicationRole in dbRoles
                where systemRoleViewModels.All(x => x.Name != applicationRole.Name)
                select new RoleViewModel
                {
                    Name = applicationRole.Name, RoleId = applicationRole.Id, Claims = applicationRole.RoleClaims.Select(dbClaim => new RoleClaimRowViewModel()
                    {
                        ClaimId = "", Type = dbClaim.ClaimType, Value = dbClaim.Value
                    }),
                    IsPersisted = true, IsSytemRole = false
                });

            foreach (var dbRole in dbRoles)
            {
                var found = roleViewModels.FirstOrDefault(x => x.Name == dbRole.Name && x.IsSytemRole);
                if (found != null) found.RoleId = dbRole.Id;
            }

            return roleViewModels;
        }
    }
}