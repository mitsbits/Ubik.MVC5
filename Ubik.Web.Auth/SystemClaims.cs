using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;

namespace Ubik.Web.Auth
{
    public abstract class SystemClaims : IEnumerable<Claim>
    {
        public const string OperationClaimType = @"http://schemas.ubik/framework/identity/claims/operation";
        public const string ResourceClaimType = @"http://schemas.ubik/framework/identity/claims/resource";
        public const string OperationToResourceClaimType = @"http://schemas.ubik/framework/identity/claims/operation-to-resource";

        public static Claim OperationToResource(string operation, string resource)
        {
            return new Claim(OperationToResourceClaimType, string.Format("{0}|{1}", operation, resource));
        }

        public class Operations : IEnumerable<string>
        {
            private readonly ICollection<string> _operationNames = new[]{
                    "Create" ,
                    "Delete" ,
                    "Edit"   ,
                    "Publish",
                    "Suspend",
                    "Approve",
                    "Submit" ,
                    "Schedule"
                };

            public const string Create = "Create";
            public const string Delete = "Delete";
            public const string Edit = "Edit";
            public const string Publish = "Publish";
            public const string Suspend = "Suspend";
            public const string Approve = "Approve";
            public const string Submit = "Submit";
            public const string Schedule = "Schedule";

            public IEnumerator<string> GetEnumerator()
            {
                return _operationNames.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        protected List<Claim> InternalList = new List<Claim>();

        /// <summary>
        /// Call this method to populate the internal list
        /// </summary>
        public virtual void Initialize()
        {
            foreach (var actionClaim in new Operations())
            {
                foreach (var entityClaim in EntityClaims)
                {
                    var claim = OperationToResource(actionClaim, entityClaim);
                    InternalList.Add(claim);
                }
            }

            foreach (var claim in RoleClaims)
            {
                InternalList.Add(new Claim(SystemRoles.RoleClaimType, claim));
            }
        }

        protected abstract string[] RoleClaims { get; }

        protected abstract string[] EntityClaims { get; }

        public abstract IEnumerable<Claim> Claims(string role);

        public IEnumerator<Claim> GetEnumerator()
        {
            return InternalList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return InternalList.GetEnumerator();
        }
    }
}