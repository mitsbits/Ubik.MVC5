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
        private readonly IResident _resident;

        public UserAdminstrationViewModelService(IViewModelBuilder<ApplicationUser, UserViewModel> userBuilder, IViewModelBuilder<ApplicationRole, RoleViewModel> roleBuilder, IUserRepository userRepo, IRoleRepository roleRepo, IResident resident)
        {
            _userBuilder = userBuilder;
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _resident = resident;
            _roleBuilder = roleBuilder;
        }

        public UserViewModel CreateUser(string id)
        {
            Expression<Func<ApplicationUser, bool>> predicate = user => user.Id == id;
            var userEntity = _userRepo.Get(predicate) ?? new ApplicationUser();
            var model = _userBuilder.CreateFrom(userEntity);
            _userBuilder.Rebuild(model);
            return model;
        }

        public RoleViewModel CreateRole(string id)
        {
            Expression<Func<ApplicationRole, bool>> predicate = role => role.Id == id;
            var roleEntity = _roleRepo.Get(predicate);
            if (roleEntity == null) return null;//TODO: safe guard for null
            var model = _roleBuilder.CreateFrom(roleEntity);
            _roleBuilder.Rebuild(model);
            return model;
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
            var dbRoles = _roleRepo.Find(x => true, role => role.Name);
            return dbRoles.Select(sarekRole => new RoleRowViewModel
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
            }).ToList();
        }
    }
}