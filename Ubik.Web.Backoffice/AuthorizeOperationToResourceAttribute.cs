using System;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Ubik.Web.Auth;

namespace Ubik.Web.Backoffice
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizeOperationToResourceAttribute : AuthorizeAttribute
    {
        public string ResourceKey { get; set; }

        public string OperationKey { get; set; }

        protected virtual Claim InternalClaim
        {
            get { return SystemClaims.OperationToResource(OperationKey, ResourceKey); }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var claimsIdentity = httpContext.User as ClaimsPrincipal;
            return claimsIdentity != null && claimsIdentity.HasClaim(SystemClaims.OperationToResourceClaimType, InternalClaim.Value);
        }
    }
}