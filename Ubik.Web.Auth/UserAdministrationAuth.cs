using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Ubik.Web.Auth.Contracts;

namespace Ubik.Web.Auth
{
    public class UserAdministrationAuth : SystemClaims, IResourceAuthProvider
    {
        private readonly Dictionary<string, List<Claim>> _rolesToClaims;
        private const string _adminRoleName = "UserAdmin";
        private const string _recourseGroup = "Users";

        public UserAdministrationAuth()
            : base()
        {
            _rolesToClaims = new Dictionary<string, List<Claim>>();
            Initialize();
        }

        public override sealed void Initialize()
        {
            base.Initialize();

            var userAdminClaims = new List<Claim>();
            foreach (var actionClaim in new Operations())
            {
                userAdminClaims.AddRange(ResourceNames.Select(entityClaim => OperationToResource(actionClaim, entityClaim)));
            }
            userAdminClaims.Add(new Claim(SystemRoles.RoleClaimType, _adminRoleName));
            InternalList.AddRange(userAdminClaims);
            _rolesToClaims.Add(_adminRoleName, userAdminClaims);
            _rolesToClaims.Add(SystemRoles.AppAdmin, userAdminClaims);
            _rolesToClaims.Add(SystemRoles.SysAdmin, userAdminClaims);
        }

        public override string[] RoleNames
        {
            get
            {
                return
                    InternalList.Where(x => x.Type == SystemRoles.RoleClaimType)
                        .Select(x => x.Value)
                        .Distinct()
                        .ToArray();
            }
        }

        public override IEnumerable<Claim> Claims(string role)
        {
            return (_rolesToClaims.ContainsKey(role)) ? new List<Claim>(_rolesToClaims[role]) : new List<Claim>();
        }

        protected override string[] ResourceNames
        {
            get { return new Resources().ToArray(); }
        }

        public override string ResourceGroup
        {
            get { return _recourseGroup; }
        }

        public class Resources : IEnumerable<string>
        {
            private readonly ICollection<string> _operationNames = new[]{
                "User" ,
                "Role" ,
                "Claim"   ,
            };

            public const string User = "User";
            public const string Role = "Role";
            public const string Claim = "Claim";

            public IEnumerator<string> GetEnumerator()
            {
                return _operationNames.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}