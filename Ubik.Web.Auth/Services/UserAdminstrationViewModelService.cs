using System.Data;
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

        private readonly IViewModelCommand<RoleSaveModel> _roleCommand;

        private readonly IDbContextScopeFactory _dbContextScopeFactory;

        private readonly IEnumerable<IResourceAuthProvider> _authProviders;



        public UserAdminstrationViewModelService(IViewModelBuilder<ApplicationUser,
            UserViewModel> userBuilder,
            IViewModelBuilder<ApplicationRole, RoleViewModel> roleBuilder,
            IUserRepository userRepo, IRoleRepository roleRepo,
            IDbContextScopeFactory dbContextScopeFactory,
            IEnumerable<IResourceAuthProvider> authProviders,
            IViewModelCommand<RoleSaveModel> roleCommand)
        {
            _userBuilder = userBuilder;
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _dbContextScopeFactory = dbContextScopeFactory;
            _authProviders = authProviders;
            _roleCommand = roleCommand;
            _roleBuilder = roleBuilder;
        }

        public UserViewModel UserModel(string id)
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

        public RoleViewModel RoleModel(string id)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                ApplicationRole roleEntity;
                if (string.IsNullOrWhiteSpace(id)) //creates a new blank transient entity
                {
                    roleEntity = new ApplicationRole();
                }
                else
                {
                    Expression<Func<ApplicationRole, bool>> predicate = role => role.Id == id;
                    roleEntity = _roleRepo.Get(predicate);
                }
                var model = _roleBuilder.CreateFrom(roleEntity);
                _roleBuilder.Rebuild(model);
                return model;
            }
        }

        public RoleViewModel RoleByNameModel(string name)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                RoleViewModel model;
                Expression<Func<ApplicationRole, bool>> predicate = role => role.Name == name;
                var roleEntity = _roleRepo.Get(predicate);
                if (roleEntity == null && SystemRoleViewModels.Any(x => x.Name == name))
                {
                    model = SystemRoleViewModels.First(x => x.Name == name);
                    model.RoleId = Guid.NewGuid().ToString();
                }
                else {
                    model = _roleBuilder.CreateFrom(roleEntity);
                }
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
            var list = new List<RoleRowViewModel>(SystemRoleViewModels
                .Select(x => new RoleRowViewModel() { Claims = x.Claims, Name = x.Name, RoleId = x.RoleId, IsPersisted = false, IsSytemRole = true }));

            using (_dbContextScopeFactory.CreateReadOnly())
            {
                var dbRoles = _roleRepo.Find(x => true, role => role.Name).ToList();
                foreach (var existing in dbRoles
                    .Select(applicationRole => list.FirstOrDefault(x => x.Name == applicationRole.Name))
                    .Where(existing => existing != null))
                {
                    list.Remove(existing);
                }
                list.AddRange(dbRoles.Select(appRole => new RoleRowViewModel
                {
                    Name = appRole.Name,
                    RoleId = appRole.Id,
                    Claims =
                        appRole.RoleClaims.Select(
                            dbClaim =>
                                new RoleClaimRowViewModel()
                                {
                                    ClaimId = "",
                                    Type = dbClaim.ClaimType,
                                    Value = dbClaim.Value
                                }),
                    IsPersisted = true,
                    IsSytemRole = SystemRoleViewModels.Any(x => x.Name == appRole.Name)
                }));
            }


            return list.OrderBy(x => x.Name).ToList();

        }

        public void Execute(RoleViewModel model)
        {
            using (var tran = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.Serializable))
            {
                _roleCommand.Execute(model);
                tran.SaveChanges();
            }

        }

        private List<RoleViewModel> _systemRoleViewModels;
        private IEnumerable<RoleViewModel> SystemRoleViewModels
        {
            get
            {
                if (_systemRoleViewModels != null) return _systemRoleViewModels;
                var systemRoleNames = _authProviders.SelectMany(x => x.RoleNames).Distinct();
                _systemRoleViewModels = new List<RoleViewModel>();
                foreach (var roles in systemRoleNames.Select(name => _authProviders.Select(x => new RoleViewModel()
                {
                    Name = name,
                    RoleId = "",
                    Claims = x.Claims(name).Select(systemClaim => new RoleClaimRowViewModel()
                    {
                        ClaimId = "",
                        Type = systemClaim.Type,
                        Value = systemClaim.Value
                    })
                })))
                {
                    _systemRoleViewModels.AddRange(roles);
                }
                return _systemRoleViewModels;
            }
        }

        //public RoleViewModel RoleById(string id)
        //{
        //    var dbRole = _roleRepo.Get(x => x.Id == id);
        //    if (dbRole == null) return null;
        //    return new RoleViewModel();
        //}
    }
}