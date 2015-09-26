using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ubik.Web.Auth.Contracts;
using Ubik.Web.Auth.ViewModels;
using Ubik.Web.Cms.Contracts;

namespace Ubik.Web.Auth.Services
{
    public class UserAdminstrationViewModelService : IUserAdminstrationViewModelService
    {
        private readonly IViewModelBuilder<ApplicationUser, UserViewModel> _userBuilder;
        private readonly IViewModelBuilder<ApplicationRole, RoleViewModel> _roleBuilder;

        private readonly IUserRepository _userRepo;
        private readonly IRoleRepository _roleRepo;

        private readonly IDbContextScopeFactory _dbContextScopeFactory;

        private readonly IEnumerable<IResourceAuthProvider> _authProviders;

        public UserAdminstrationViewModelService(IViewModelBuilder<ApplicationUser,
            UserViewModel> userBuilder,
            IViewModelBuilder<ApplicationRole, RoleViewModel> roleBuilder,
            IUserRepository userRepo, IRoleRepository roleRepo,
            IDbContextScopeFactory dbContextScopeFactory,
            IEnumerable<IResourceAuthProvider> authProviders)
        {
            _userBuilder = userBuilder;
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _dbContextScopeFactory = dbContextScopeFactory;
            _authProviders = authProviders;
            _roleBuilder = roleBuilder;
        }

        public UserViewModel CreateUser(string id)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                Expression<Func<ApplicationUser, bool>> predicate = user => user.Id == id;
                var userEntity = _userRepo.Get(predicate) ?? new ApplicationUser();
                var model = _userBuilder.CreateFrom(userEntity);
                _userBuilder.Rebuild(model);
                return model;
            }
        }

        public RoleViewModel CreateRole(string id)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                Expression<Func<ApplicationRole, bool>> predicate = role => role.Id == id;
                var roleEntity = _roleRepo.Get(predicate) ?? new ApplicationRole();
                var model = _roleBuilder.CreateFrom(roleEntity);
                _roleBuilder.Rebuild(model);
                return model;
            }
        }

        public IEnumerable<UserRowViewModel> Users()
        {
            var dbCollection = _userRepo.Find(x => true, user => user.UserName);
            var dbRoles = _roleRepo.Find(x => true, role => role.Name);
            return dbCollection.Select(sarekUser => new UserRowViewModel
            {
                UserId = sarekUser.Id,
                UserName = sarekUser.UserName,
                Roles = sarekUser.Roles.Select(
                    role =>
                        new RoleRowViewModel()
                        {
                            Name = dbRoles.Single(x => x.Id == role.RoleId).Name,
                            RoleId = role.RoleId
                        }).ToList()
            }).ToList();
        }

        public IEnumerable<RoleRowViewModel> Roles()
        {
            var systemRoleNames = _authProviders.SelectMany(x => x.RoleNames).Distinct();
            var list = new List<RoleRowViewModel>();
            foreach (var systemRoleName in systemRoleNames)
            {
                var name = systemRoleName;
                var roles = _authProviders.Select(x => new RoleRowViewModel()
                {
                    Name = name,
                    RoleId = "",
                    Claims = x.Claims(name).Select(systemClaim => new RoleClaimRowViewModel()
                    {
                        ClaimId = "",
                        Type = systemClaim.Type,
                        Value = systemClaim.Value
                    })
                });
                list.AddRange(roles);
            }

            using (_dbContextScopeFactory.CreateReadOnly())
            {
                var dbRoles = _roleRepo.Find(x => true, role => role.Name);
                list.AddRange(dbRoles.Select(sarekRole => new RoleRowViewModel
                {
                    Name = sarekRole.Name,
                    RoleId = sarekRole.Id,
                    Claims =
                        sarekRole.RoleClaims.Select(
                            dbClaim =>
                                new RoleClaimRowViewModel()
                                {
                                    ClaimId = "",
                                    Type = dbClaim.ClaimType,
                                    Value = dbClaim.Value
                                }).ToList()
                }).ToList());
            }


            return list.GroupBy(x=>x.Name).Select(roleGroup => new RoleRowViewModel()
            {
                Name = roleGroup.Key,
                RoleId = roleGroup.Max(g => g.RoleId),
                Claims = roleGroup.SelectMany(x => x.Claims).Distinct().ToList()
            });
         
        }

        public RoleViewModel Role(string id)
        {
            var dbRole = _roleRepo.Get(x => x.Id == id);
            if (dbRole == null) return null;
            return new RoleViewModel();
        }
    }
}