﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ubik.Web.Auth.Contracts
{
    public interface IUserAdminstrationService
    {
        IEnumerable<ApplicationUser> Find(Expression<Func<ApplicationUser, bool>> predicate, int pageNumber, int pageSize, out int totalRecords);

        IEnumerable<ApplicationRole> Find(Expression<Func<ApplicationRole, bool>> predicate, int pageNumber, int pageSize, out int totalRecords);

        ApplicationUser CreateUser(ApplicationUser user, string password);

        void SetRoles(ApplicationUser user, string[] newRoles);

        void CopyRole(string source, string target);

        void DeleteRole(string name);

        Task LockUser(string userId, int days);

        Task UnockUser(string userId);

        Task SetPassword(string userId, string newPassword);
    }
}