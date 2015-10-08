using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using Ubik.Web.Auth;

[assembly: OwinStartupAttribute(typeof(Ubik.UI.MVC.Startup))]
namespace Ubik.UI.MVC
{
    public class Startup
    {

        protected IContainer _container;
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            _container = IoCConfig.RegisterDependencies(app);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(_container));
            app.UseAutofacMiddleware(_container);
            app.UseAutofacMvc();
            app.ConfigureUbikAuth();

 
        }
    }
}
