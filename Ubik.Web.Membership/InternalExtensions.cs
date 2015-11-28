using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Ubik.Web.Membership.Contracts;
using Ubik.Web.Membership.Managers;
using Ubik.Web.Membership.ViewModels;

namespace Ubik.Web.Membership
{
    internal static class InternalExt
    {
        public static IEnumerable<RoleViewModel> RoleModels(this IEnumerable<IResourceAuthProvider> authProviders)
        {
            var resourceAuthProviders = authProviders as IResourceAuthProvider[] ?? authProviders.ToArray();
            var systemRoleNames = resourceAuthProviders.SelectMany(x => x.RoleNames).Distinct();

            foreach (var role in systemRoleNames)
            {
                var model = new RoleViewModel()
                {
                    Name = role,
                    IsSytemRole = true,
                    IsPersisted = false,
                    RoleId = string.Empty,
                    IsSelected = false,
                };
                var claims = resourceAuthProviders.SelectMany(x => x.Claims(model.Name)).ToList();
                if (claims.Any())
                {
                    model.Claims =
                        claims.Select(
                            x => new RoleClaimViewModel()
                            {
                                Type = x.Type,
                                Value = x.Value,
                                IsSelected = true,
                                ResourceGroups = resourceAuthProviders.Where(p => p.ContainsClaim(x)).Select(r => r.ResourceGroup).Distinct().ToArray()
                            });
                }
                else
                {
                    model.Claims = new List<RoleClaimViewModel>();
                }

                var avalableClaims = authProviders.AvailableSystemClaims();
                foreach (var roleClaimRowViewModel in avalableClaims)
                {
                    roleClaimRowViewModel.IsSelected =
                        model.Claims.Any(x => x.Type == roleClaimRowViewModel.Type && x.Value == roleClaimRowViewModel.Value);
                }
                model.AvailableClaims = avalableClaims.ToArray();

                yield return model;
            }
        }

        [Obsolete("Use other overload", true)]
        public static IEnumerable<RoleViewModel> RoleModelsCheckDB(
            this IEnumerable<IResourceAuthProvider> authProviders, IRoleRepository roleRepo)
        {
            var resourceAuthProviders = authProviders as IResourceAuthProvider[] ?? authProviders.ToArray();
            var systemRoleViewModels = resourceAuthProviders.RoleModels().ToList();
            var roleViewModels = new List<RoleViewModel>(systemRoleViewModels);

            var dbRoles = roleRepo.Find(x => true, role => role.Name).ToList();

            roleViewModels.AddRange(from applicationRole in dbRoles
                                    where systemRoleViewModels.All(x => x.Name != applicationRole.Name)
                                    select new RoleViewModel
                                    {
                                        Name = applicationRole.Name,
                                        RoleId = applicationRole.Id,
                                        Claims = applicationRole.RoleClaims.Select(dbClaim => new RoleClaimViewModel()
                                        {
                                            Type = dbClaim.ClaimType,
                                            Value = dbClaim.Value
                                        }),
                                        IsPersisted = true,
                                        IsSytemRole = false
                                    });

            foreach (var dbRole in dbRoles)
            {
                var found = roleViewModels.FirstOrDefault(x => x.Name == dbRole.Name && x.IsSytemRole);
                if (found != null) found.RoleId = dbRole.Id;
            }

            //var availableClaims = resourceAuthProviders.AvailableSystemClaims().ToArray();

            //foreach (var roleViewModel in roleViewModels)
            //{
            //    foreach (var roleClaimRowViewModel in availableClaims)
            //    {
            //        roleClaimRowViewModel.IsSelected =
            //            roleViewModel.Claims.Any(
            //                x =>
            //                    x.Type == roleClaimRowViewModel.Type && x.Type == roleClaimRowViewModel.Value);
            //    }
            //    roleViewModel.AvailableClaims = availableClaims.ToArray();
            //}

            return roleViewModels;
        }

        public static IEnumerable<RoleViewModel> RoleModelsCheckDB(
            this IEnumerable<IResourceAuthProvider> authProviders, ApplicationRoleManager roleManager)
        {
            var resourceAuthProviders = authProviders as IResourceAuthProvider[] ?? authProviders.ToArray();
            var systemRoleViewModels = resourceAuthProviders.RoleModels().ToList();
            var roleViewModels = new List<RoleViewModel>(systemRoleViewModels);

            var dbRoles = roleManager.Roles.Include(x => x.RoleClaims).ToList();

            roleViewModels.AddRange(from applicationRole in dbRoles
                                    where systemRoleViewModels.All(x => x.Name != applicationRole.Name)
                                    select new RoleViewModel
                                    {
                                        Name = applicationRole.Name,
                                        RoleId = applicationRole.Id,
                                        Claims = applicationRole.RoleClaims.Select(dbClaim => new RoleClaimViewModel()
                                        {
                                            Type = dbClaim.ClaimType,
                                            Value = dbClaim.Value
                                        }),
                                        IsPersisted = true,
                                        IsSytemRole = false
                                    });

            foreach (var dbRole in dbRoles)
            {
                var found = roleViewModels.FirstOrDefault(x => x.Name == dbRole.Name && x.IsSytemRole);
                if (found != null) found.RoleId = dbRole.Id;
            }

            foreach (var roleViewModel in roleViewModels)
            {
                if (roleViewModel.AvailableClaims == null)
                {
                    var avalableClaims = authProviders.AvailableSystemClaims();
                    foreach (var roleClaimRowViewModel in avalableClaims)
                    {
                        roleClaimRowViewModel.IsSelected =
                            roleViewModel.Claims.Any(x => x.Type == roleClaimRowViewModel.Type && x.Value == roleClaimRowViewModel.Value);
                    }
                    roleViewModel.AvailableClaims = avalableClaims.ToArray();
                }
            }

            return roleViewModels;
        }

        public static IEnumerable<RoleClaimViewModel> AvailableSystemClaims(this IEnumerable<IResourceAuthProvider> authProviders)
        {
            var result = new List<RoleClaimViewModel>();
            foreach (var systemRole in new SystemRoles())
            {
                foreach (var resourceAuthProvider in authProviders)
                {
                    foreach (var claim in resourceAuthProvider.Claims(systemRole.Value))
                    {
                        if (!result.Any(x => x.Type == claim.Type && x.Value == claim.Value))
                        {
                            result.Add(new RoleClaimViewModel() { Type = claim.Type, Value = claim.Value, ResourceGroups = authProviders.Where(p => p.ContainsClaim(claim)).Select(r => r.ResourceGroup).Distinct().ToArray() });
                        }
                    }
                }
            }
            return result.Distinct();
        }
    }
}