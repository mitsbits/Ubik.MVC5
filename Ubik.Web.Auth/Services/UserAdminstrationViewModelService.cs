using System.Data;
using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ubik.Infra.Contracts;
using Ubik.Web.Auth.Contracts;
using Ubik.Web.Auth.ViewModels;

namespace Ubik.Web.Auth.Services
{

    public class UserAdminstrationViewModelService : IUserAdminstrationViewModelService
    {
        private readonly IViewModelBuilder<ApplicationUser, UserViewModel> _userBuilder;
        private readonly IViewModelBuilder<ApplicationUser, NewUserViewModel> _newUserBuilder;
        private readonly IViewModelBuilder<ApplicationRole, RoleViewModel> _roleBuilder;

        private readonly IUserRepository _userRepo;
        private readonly IRoleRepository _roleRepo;

        private readonly IViewModelCommand<RoleSaveModel> _roleCommand;
        private readonly IViewModelCommand<NewUserSaveModel> _newUserCommand;

        private readonly IDbContextScopeFactory _dbContextScopeFactory;

        private readonly IEnumerable<IResourceAuthProvider> _authProviders;



        private readonly ICacheProvider _cache;

        private const string _roleViewModelsCacheKey = "UserAdminstrationViewModelService_RoleViewModels";

        public UserAdminstrationViewModelService(
            IViewModelBuilder<ApplicationRole, RoleViewModel> roleBuilder,
            IUserRepository userRepo, IRoleRepository roleRepo,
            IDbContextScopeFactory dbContextScopeFactory,
            IEnumerable<IResourceAuthProvider> authProviders,
            IViewModelCommand<RoleSaveModel> roleCommand,
            IViewModelCommand<NewUserSaveModel> newUserCommand, 
            ICacheProvider cache)
        {
        
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _dbContextScopeFactory = dbContextScopeFactory;
            _authProviders = authProviders;
            _roleCommand = roleCommand;
            _roleBuilder = roleBuilder;
            _newUserCommand = newUserCommand;
            _cache = cache;
            _userBuilder = new UserViewModelBuilder(_roleRepo, RoleViewModels);
            _newUserBuilder = new NewUserViewModelBuilder(RoleViewModels);
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
                if (roleEntity == null  && SystemRoleViewModels.Any(x => x.Name == name))
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
                        new RoleViewModel()
                        {
                            Name = dbRoles.Single(x => x.Id == role.RoleId).Name,
                            RoleId = role.RoleId
                        }).ToList()
            }).ToList();
        }

        public IEnumerable<RoleViewModel> Roles()
        {
            return RoleViewModels;

        }

        public void Execute(RoleViewModel model)
        {
            using (var tran = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.Serializable))
            {
                _roleCommand.Execute(model);
                tran.SaveChanges();
                _cache.RemoveItem(_roleViewModelsCacheKey); // force cache to invalidate
                //TODO: publish message for new role
            }
        }

        public void Execute(NewUserViewModel model)
        {
            using (var tran = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.Serializable))
            {
                _newUserCommand.Execute(model);
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


    }
}