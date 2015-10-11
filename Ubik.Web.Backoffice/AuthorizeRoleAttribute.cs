using System;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Ubik.Web.Auth;

namespace Ubik.Web.Backoffice
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizeRoleAttribute : AuthorizeAttribute
    {
        public string RoleName { get; set; }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var claimsIdentity = httpContext.User as ClaimsPrincipal;
            return claimsIdentity != null && claimsIdentity.HasClaim(SystemRoles.RoleClaimType, RoleName);
        }
    }
}