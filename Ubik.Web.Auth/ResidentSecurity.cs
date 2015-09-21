using System;
using System.Collections.Generic;
using System.Security.Claims;
using Ubik.Web.Cms.Contracts;

namespace Ubik.Web.Auth
{
    public class ResidentSecurity : IResidentSecurity
    {
        private readonly ICollection<Claim> _systemRoles;

        public ResidentSecurity()
        {
            var rolesFromSystem = new SystemRoles();
            _systemRoles = new HashSet<Claim>(rolesFromSystem);
        }

        public IEnumerable<Claim> SystemRoles { get { return _systemRoles; } }

        public IEnumerable<Claim> SystemRoleClaims(string role)
        {
            throw new NotImplementedException();
        }
    }
}