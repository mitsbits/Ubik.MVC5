using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ubik.Web.Auth.Contracts;
using Ubik.Web.Auth.Managers;

namespace Ubik.Web.Auth.Services
{
    public class UserAdminstrationService : IUserAdminstrationService
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

        public void CopyRole(string source, string target)
        {
            var sourceIsSytemRole = _viewModels.SystemRoleViewModels.Any(x => x.Name == source);
            ApplicationRole copy;
            if (sourceIsSytemRole)
            {
                var sourceViewModel = _viewModels.Roles().First(x => x.Name == source);
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
            if (result.Succeeded) return;
            throw new ApplicationException(string.Join("\n", result.Errors));
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