using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using Mehdime.Entity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ubik.Infra.Contracts;
using Ubik.Web.Auth.Contracts;
using Ubik.Web.Auth.Managers;
using Ubik.Web.Auth.ViewModels;

namespace Ubik.Web.Auth.Services
{
    public class UserAdminstrationService : IUserAdminstrationService, IUserAdminstrationViewModelService
    {
        private readonly IUserRepository _userRepo;
        private readonly IRoleRepository _roleRepo;

        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationRoleManager _roleManager;

        private readonly IViewModelBuilder<ApplicationUser, UserViewModel> _userBuilder;
        private readonly IViewModelBuilder<ApplicationUser, NewUserViewModel> _newUserBuilder;
        private readonly IViewModelBuilder<ApplicationRole, RoleViewModel> _roleBuilder;


        private readonly IViewModelCommand<RoleSaveModel> _roleCommand;
        private readonly IViewModelCommand<NewUserSaveModel> _newUserCommand;
        private readonly IViewModelCommand<UserSaveModel> _userCommand;


        private readonly IDbContextScopeFactory _dbContextScopeFactory;

        private readonly IEnumerable<IResourceAuthProvider> _authProviders;



        private readonly ICacheProvider _cache;

        private const string _roleViewModelsCacheKey = "UserAdminstrationService_RoleViewModels";

        public UserAdminstrationService(IUserRepository userRepository, IRoleRepository roleRepository, ApplicationUserManager userManager, ApplicationRoleManager roleManager, IViewModelCommand<RoleSaveModel> roleCommand, IViewModelCommand<NewUserSaveModel> newUserCommand, IDbContextScopeFactory dbContextScopeFactory, IEnumerable<IResourceAuthProvider> authProviders, ICacheProvider cache, IViewModelBuilder<ApplicationRole, RoleViewModel> roleBuilder, IViewModelCommand<UserSaveModel> userCommand)
        {
            _userRepo = userRepository;
            _roleRepo = roleRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _roleCommand = roleCommand;
            _newUserCommand = newUserCommand;
            _dbContextScopeFactory = dbContextScopeFactory;
            _authProviders = authProviders;
            _cache = cache;
            _roleBuilder = roleBuilder;
            _userCommand = userCommand;

            _userBuilder = new UserViewModelBuilder(_roleRepo, RoleViewModels);
            _newUserBuilder = new NewUserViewModelBuilder(RoleViewModels);
        }



        public void CopyRole(string source, string target)
        {
            var sourceIsSytemRole = SystemRoleViewModels.Any(x => x.Name == source);
            ApplicationRole copy;
            if (sourceIsSytemRole)
            {
                var sourceViewModel = RoleModels().First(x => x.Name == source);
                copy = new ApplicationRole() { Name = target };
                foreach (var roleClaimRowViewModel in sourceViewModel.Claims)
                {
                    copy.RoleClaims.Add(new ApplicationClaim(roleClaimRowViewModel.Type, roleClaimRowViewModel.Value));
                }
            }
            else
            {
                var original = _roleManager.FindByNameAsync(source).Result;
                if (original == null) throw new ApplicationException("source role not found");
                copy = new ApplicationRole(target);
                foreach (var applicationClaim in original.RoleClaims)
                {
                    copy.RoleClaims.Add(applicationClaim);
                }
            }
            var result = _roleManager.CreateAsync(copy).Result;
            if (!result.Succeeded) throw new ApplicationException(string.Join("\n", result.Errors));
            _cache.RemoveItem(_roleViewModelsCacheKey);
        }

        public void DeleteRole(string name)
        {
            var sourceIsSytemRole = SystemRoleViewModels.Any(x => x.Name == name);
            if (sourceIsSytemRole) throw new ApplicationException("can not delete a system role");
            var role = _roleManager.FindByName(name);
            if (role == null) throw new ApplicationException("role to delete not found");
            var result = _roleManager.DeleteAsync(role).Result;
            if (!result.Succeeded) throw new ApplicationException(string.Join("\n", result.Errors));
            _cache.RemoveItem(_roleViewModelsCacheKey);
        }

        public async Task LockUser(string userId, int days)
        {
            var results = new List<IdentityResult>
            {
              await  _userManager.SetLockoutEnabledAsync(userId, true),
              await   _userManager.SetLockoutEndDateAsync(userId, DateTime.UtcNow.AddDays(days)),
              await  _userManager.ResetAccessFailedCountAsync(userId)
            };
            if (!results.All(x => x.Succeeded)) throw new ApplicationException(string.Join("\n", results.SelectMany(x => x.Errors)));
        }

        public async Task UnockUser(string userId)
        {
            var results = new List<IdentityResult>
            {
              await _userManager.SetLockoutEnabledAsync(userId, false),
              await _userManager.ResetAccessFailedCountAsync(userId)
            };
            if (!results.All(x => x.Succeeded)) throw new ApplicationException(string.Join("\n", results.SelectMany(x => x.Errors)));
        }

        public async Task SetPassword(string userId, string newPassword)
        {
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(userId);
            var result = await _userManager.ResetPasswordAsync(userId, resetToken, newPassword);
            if (!result.Succeeded) throw new ApplicationException(string.Join("\n", result.Errors));
        }

        public IEnumerable<ApplicationUser> Find(Expression<Func<ApplicationUser, bool>> predicate, int pageNumber, int pageSize, out int totalRecords)
        {
            return _userRepo.Find(predicate, user => user.UserName, false, pageNumber, pageSize, out  totalRecords);
        }

        public IEnumerable<ApplicationRole> Find(Expression<Func<ApplicationRole, bool>> predicate, int pageNumber, int pageSize, out int totalRecords)
        {
            return _roleRepo.Find(predicate, role => (role != null) ? role.Name : string.Empty, false, pageNumber, pageSize, out  totalRecords);
        }

        public ApplicationUser CreateUser(ApplicationUser user, string password)
        {
            var result = _userManager.CreateAsync(user, password).Result;
            if (!result.Succeeded) throw new ApplicationException("you should handle this looser");
            return user;
        }

        public void SetRoles(ApplicationUser user, string[] newRoles)
        {
            user.Roles.Clear();
            _userManager.AddToRoles(user.Id, newRoles);
        }

        public UserViewModel UserModel(string id)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                ApplicationUser entity;
                if (string.IsNullOrWhiteSpace(id))
                {
                    entity = new ApplicationUser();
                }
                else
                {
                    Expression<Func<ApplicationUser, bool>> predicate = user => user.Id == id;
                    entity = _userRepo.Get(predicate);
                }
                var model = _userBuilder.CreateFrom(entity);
                _userBuilder.Rebuild(model);
                return model;
            }
        }

        public NewUserViewModel NewUserModel()
        {
            var entity = new ApplicationUser();
            var model = _newUserBuilder.CreateFrom(entity);
            _newUserBuilder.Rebuild(model);
            return model;
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
                else
                {

                    model = _roleBuilder.CreateFrom(roleEntity);
                }
                _roleBuilder.Rebuild(model);
                model.IsSytemRole = SystemRoleViewModels.Any(x => x.Name == name);
                return model;
            }
        }

        public IEnumerable<UserRowViewModel> UserModels()
        {


            var dbCollection = _userManager.Users.ToList();
            return dbCollection.Select(appUser => new UserRowViewModel
            {
                UserId = appUser.Id,
                UserName = appUser.UserName,
                Email = appUser.Email,
                IsLockedOut = appUser.LockoutEnabled,
                LockedOutEndUtc = appUser.LockoutEndDateUtc,
                Roles = appUser.Roles.Select(
                    role => RoleViewModels.FirstOrDefault(x => x.RoleId == role.RoleId)
                       ).ToList()
            }).ToList();

        }

        public IEnumerable<RoleViewModel> RoleModels()
        {
            return RoleViewModels;

        }



        public void Execute(NewUserSaveModel model)
        {
            using (var tran = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.Serializable))
            {
                _newUserCommand.Execute(model);
                tran.SaveChanges();
                //TODO: publish message for new role
            }
        }

        public async Task Execute(UserSaveModel model)
        {
            using (var tran = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.Serializable))
            {
              await  _userCommand.Execute(model);
                tran.SaveChanges();
                //TODO: publish message for new role
            }
        }

        private List<RoleViewModel> _systemRoleViewModels;
        public virtual IEnumerable<RoleViewModel> SystemRoleViewModels
        {
            get
            {
                if (_systemRoleViewModels != null) return _systemRoleViewModels;
                _systemRoleViewModels = new List<RoleViewModel>(_authProviders.RoleModels());
                return _systemRoleViewModels;
            }
        }

        private IEnumerable<RoleViewModel> RoleViewModels
        {
            get
            {

                if (_cache.GetItem(_roleViewModelsCacheKey) as IEnumerable<RoleViewModel> != null) return _cache.GetItem(_roleViewModelsCacheKey) as IEnumerable<RoleViewModel>;
                using (_dbContextScopeFactory.CreateReadOnly())
                {
                    _cache.SetItem(_roleViewModelsCacheKey, new List<RoleViewModel>(_authProviders.RoleModelsCheckDB(_roleRepo)));
                    return _cache.GetItem(_roleViewModelsCacheKey) as IEnumerable<RoleViewModel>;
                }
            }
        }




        public void Execute(RoleSaveModel model)
        {
            using (var tran = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.Serializable))
            {
                _roleCommand.Execute(model);
                tran.SaveChanges();
                _cache.RemoveItem(_roleViewModelsCacheKey); // force cache to invalidate
                //TODO: publish message for new role
            }
        }

    }
}