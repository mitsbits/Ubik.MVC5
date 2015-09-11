using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNet.Identity;
using Ubik.Web.Auth.Contracts;
using Ubik.Web.Auth.Managers;

namespace Ubik.Web.Auth.Services
{
    internal class UserAdminstrationService : IUserAdminstrationService
    {
        private readonly IUserRepository _userRepo;
        private readonly IRoleRepository _roleRepo;
        private readonly IUserAdminstrationViewModelService _viewModels;
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationRoleManager _roleManager;

        public UserAdminstrationService(IUserRepository userRepository, IRoleRepository roleRepository, IUserAdminstrationViewModelService viewModels, ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            _userRepo = userRepository;
            _roleRepo = roleRepository;
            _viewModels = viewModels;
            _userManager = userManager;
            _roleManager = roleManager;
        }



        public IUserAdminstrationViewModelService ViewModels
        {
            get { return _viewModels; }
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
            var roles = _roleManager.Roles.ToList();
            foreach (var sarekRole in roles)
            {
                _userManager.RemoveFromRole(user.Id, sarekRole.Name);
            }
            _userManager.AddToRoles(user.Id, newRoles);
        }
    }
}