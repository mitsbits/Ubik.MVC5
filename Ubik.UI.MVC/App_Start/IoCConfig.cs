using System;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Mehdime.Entity;
using Microsoft.AspNet.Identity;
using Ubik.Web.Auth;
using Ubik.Web.Auth.Contracts;
using Ubik.Web.Auth.Managers;
using Ubik.Web.Auth.Repositories;
using Ubik.Web.Auth.Services;
using Ubik.Web.Auth.Stores;
using Ubik.Web.Auth.ViewModels;


namespace Ubik.UI.MVC
{
    public class IoCConfig
    {
        private static readonly Assembly[] Asmbls;

        static IoCConfig()
        {
            Asmbls = ScopedAssemblies();
        }

        public static IContainer RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            WireUpInternals(builder);
            WireUpPubSub(builder);
            WireUpSso(builder);
            WireUpCms(builder);
            WireUpElmahAgents(builder);

            #region Register all controllers for the assembly

            // Note that ASP.NET MVC requests controllers by their concrete types, so registering them As<IController>() is incorrect.
            // Also, if you register controllers manually and choose to specify lifetimes, you must register them as InstancePerDependency() or InstancePerHttpRequest() -
            // ASP.NET MVC will throw an exception if you try to reuse a controller instance for multiple requests.

            builder.RegisterControllers(Asmbls);
            //builder.RegisterApiControllers(Asmbls);
            //builder.RegisterSource(new ViewRegistrationSource());

            #endregion Register all controllers for the assembly

            #region Register modules

            foreach (var module in ScopedAssemblies().Select(
                scopedAssembly => scopedAssembly.GetTypes()
                .Where(p => typeof(IModule).IsAssignableFrom(p)
                            && !p.IsAbstract
                            && p.GetConstructors().Any(c => !c.GetParameters().Any() && c.IsPublic))
                .Select(p => (IModule)Activator.CreateInstance(p))).SelectMany(modules => modules))
            {
                builder.RegisterModule(module);
            }


            #endregion Register modules

            builder.RegisterModelBinders(Asmbls);
            builder.RegisterModelBinderProvider();

            #region Inject HTTP Abstractions

            /*
         The MVC Integration includes an Autofac module that will add HTTP request lifetime scoped registrations for the HTTP abstraction classes. The following abstract classes are included:
        -- HttpContextBase
        -- HttpRequestBase
        -- HttpResponseBase
        -- HttpServerUtilityBase
        -- HttpSessionStateBase
        -- HttpApplicationStateBase
        -- HttpBrowserCapabilitiesBase
        -- HttpCachePolicyBase
        -- VirtualPathProvider

        To use these abstractions add the AutofacWebTypesModule to the container using the standard RegisterModule method.
        */
            builder.RegisterModule<AutofacWebTypesModule>();
            builder.RegisterFilterProvider();

            #endregion Inject HTTP Abstractions

            var container = builder.Build();
            return container;
        }

        private static void WireUpPubSub(ContainerBuilder builder)
        {
            //builder.RegisterType<RegisteredHandlersInMemoryPublisher>().As<IEventPublisher>().SingleInstance();
            //builder.RegisterType<RegisteredHandlersInMemoryProcessor>().As<IDomainCommandProcessor>().SingleInstance();
        }

        #region Wire Up

        private static void WireUpCms(ContainerBuilder builder)
        {
        }

        private static void WireUpSso(ContainerBuilder builder)
        {
                        builder.Register(c => new ApplicationUserManager(new ApplicationUserStore(new AuthDbContext())))
                .Named<ApplicationUserManager>("transient");
                        builder.Register(c => new ApplicationRoleManager(new ApplicationRoleStore(new AuthDbContext())))
                            .Named<ApplicationRoleManager>("transient");

                        builder.RegisterType<AuthDbContext>().As<AuthDbContext>().InstancePerRequest();
                        builder.RegisterType<ApplicationUserStore>().As<IUserStore<ApplicationUser>>().InstancePerRequest();
                        builder.RegisterType<ApplicationRoleStore>().As<IRoleStore<ApplicationRole, string>>().InstancePerRequest();
                        builder.RegisterType<ApplicationUserManager>().InstancePerRequest();
                        builder.RegisterType<ApplicationRoleManager>().InstancePerRequest();

                        builder.RegisterType<DbContextScopeFactory>().As<IDbContextScopeFactory>().SingleInstance();
                        builder.RegisterType<AmbientDbContextLocator>().As<IAmbientDbContextLocator>().SingleInstance();

                        builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerRequest();
                        builder.RegisterType<RoleRepository>().As<IRoleRepository>().InstancePerRequest();

                        builder.RegisterType<UserAdminstrationService>().As<IUserAdminstrationService>().InstancePerRequest();
                        builder.RegisterType<UserAdminstrationViewModelService>()
                            .As<IUserAdminstrationViewModelService>()
                            .InstancePerRequest();

                        builder.RegisterType<UserViewModelBuilder>().As<IViewModelBuilder<ApplicationUser, UserViewModel>>().InstancePerRequest();
                        builder.RegisterType<RoleViewModelBuilder>().As<IViewModelBuilder<ApplicationRole, RoleViewModel>>().InstancePerRequest();
                        builder.RegisterType<AddUserViewModelBuilder>().As<IViewModelBuilder<ApplicationUser, AddUserViewModel>>().InstancePerRequest();
        }

        private static void WireUpInternals(ContainerBuilder builder)
        {
            //builder.RegisterType<Resident>().As<IResident>().SingleInstance();
            //builder.RegisterType<ResidentSecurity>().As<IResidentSecurity>().SingleInstance();
            //builder.RegisterType<ResidentAdministration>().As<IResidentAdministration>().SingleInstance();
           // builder.RegisterType<ResidentPubSub>().As<IResidentPubSub>().SingleInstance();
        }

        private static void WireUpElmahAgents(ContainerBuilder builder)
        {
        }

        #endregion Wire Up

        private static Assembly[] ScopedAssemblies()
        {
            return
                BuildManager.GetReferencedAssemblies()
                .Cast<Assembly>()
                .Where(x => x.FullName.StartsWith("Sarek"))
                .ToArray();
        }
    }
}