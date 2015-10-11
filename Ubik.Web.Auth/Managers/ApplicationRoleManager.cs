using System;
using System.Security;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Ubik.Web.Auth.Stores;

namespace Ubik.Web.Auth.Managers
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole, string> store)
            : base(store)
        {



        }

        public async Task<IdentityResult> ClearClaims(string role)
        {
            var store = Store as IRoleStoreWithCustomClaims;
            if (store == null) throw new ApplicationException("IRoleStoreWithCustomClaims missing");
            return await store.ClearAllRoleClaims(role);
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            var manager = new ApplicationRoleManager(new ApplicationRoleStore(context.Get<AuthDbContext>()));
            return manager;
        }
    }
}