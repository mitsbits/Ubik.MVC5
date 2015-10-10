using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
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
            var config = new HttpConfiguration {DependencyResolver = new AutofacWebApiDependencyResolver(_container)};
            WebApiConfig.Register(config);
            app.UseAutofacMiddleware(_container);
            //app.UseAutofacWebApi(config);
            app.UseWebApi(config);
            app.UseAutofacMvc();
            app.ConfigureUbikAuth();

 
        }
    }
}
