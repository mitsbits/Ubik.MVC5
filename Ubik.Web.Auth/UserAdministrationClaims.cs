using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Ubik.Web.Auth
{
    public class UserAdministrationClaims : SystemClaims
    {
        private readonly Dictionary<string, List<Claim>> _rolesToClaims;

        public UserAdministrationClaims()
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
                userAdminClaims.AddRange(EntityClaims.Select(entityClaim => OperationToResource(actionClaim, entityClaim)));
            }
            _rolesToClaims.Add("UserAdmin", userAdminClaims);
        }

        protected override string[] RoleClaims
        {
            get { return new[] { "UserAdmin" }; }
        }

        public override IEnumerable<Claim> Claims(string role)
        {
            return (_rolesToClaims.ContainsKey(role)) ? new List<Claim>(_rolesToClaims[role]) : new List<Claim>();
        }

        protected override string[] EntityClaims
        {
            get { return new Resources().ToArray(); }
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