using Microsoft.Owin;
using Owin;
using Ubik.Web.Auth;

[assembly: OwinStartup(typeof(Ubik.Web.Presenatation.Admin.Startup))]
namespace Ubik.Web.Presenatation.Admin
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            app.ConfigureUbikAuth();
        }
    }
}
