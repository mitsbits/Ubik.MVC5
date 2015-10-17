using System.Web;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Mehdime.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using Ubik.Cache.Runtime;
using Ubik.EF;
using Ubik.Infra.Contracts;
using Ubik.UI.MVC.Models;
using Ubik.Web.Auth;
using Ubik.Web.Auth.Contracts;
using Ubik.Web.Auth.Managers;
using Ubik.Web.Auth.Repositories;
using Ubik.Web.Auth.Services;
using Ubik.Web.Auth.Stores;
using Ubik.Web.Auth.ViewModels;
using Ubik.Web.Backoffice;
using Ubik.Web.Backoffice.Contracts;
using Ubik.Web.Cms.Contracts;
using Ubik.Web.Components.AntiCorruption.Contracts;
using Ubik.Web.Components.AntiCorruption.Services;
using Ubik.Web.Components.AntiCorruption.ViewModels;
using Ubik.Web.Components.Contracts;
using Ubik.Web.EF.Components;
using Ubik.Web.Infra.Navigation.Contracts;

namespace Ubik.UI.MVC
{
    public class IoCConfig
    {
        private static readonly Assembly[] Asmbls;

        static IoCConfig()
        {
            Asmbls = ScopedAssemblies();
        }

        public static IContainer RegisterDependencies(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            WireUpInternals(builder);
            WireUpPubSub(builder);
            WireUpSso(builder, app);
            WireUpCms(builder);
            WireUpElmahAgents(builder);

            #region Register all controllers for the assembly

            // Note that ASP.NET MVC requests controllers by their concrete types, so registering them As<IController>() is incorrect.
            // Also, if you register controllers manually and choose to specify lifetimes, you must register them as InstancePerDependency() or InstancePerHttpRequest() -
            // ASP.NET MVC will throw an exception if you try to reuse a controller instance for multiple requests.

            builder.RegisterControllers(Asmbls);
            builder.RegisterApiControllers(Asmbls);
            builder.RegisterSource(new ViewRegistrationSource());

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
            builder.RegisterType<PersistedDeviceRepository>().As<ICRUDRespoditory<PersistedDevice>>().InstancePerRequest();
            builder.RegisterType<PersistedSectionRepository>().As<ICRUDRespoditory<PersistedSection>>().InstancePerRequest();
            builder.RegisterType<DeviceAdministrationService>().As<IDeviceAdministrationService<int>>().InstancePerRequest();
            builder.RegisterType<DeviceAdministrationService>().As<IDeviceAdministrationViewModelService>().InstancePerRequest();

            builder.RegisterType<DeviceViewModelCommand>().As<IViewModelCommand<DeviceSaveModel>>().InstancePerRequest();
            builder.RegisterType<SectionViewModelCommand>().As<IViewModelCommand<SectionSaveModel>>().InstancePerRequest();
        }

        private static void WireUpSso(ContainerBuilder builder, IAppBuilder app)
        {
            builder.Register(c => new ApplicationUserManager(new ApplicationUserStore(new AuthDbContext())))
                    .Named<ApplicationUserManager>("transient");
            builder.Register(c => new ApplicationRoleManager(new ApplicationRoleStore(new AuthDbContext())))
                .Named<ApplicationRoleManager>("transient");

            builder.RegisterType<AuthDbContext>().As<AuthDbContext>().InstancePerRequest();
               builder.RegisterType<ComponentsDbContext>().As<ComponentsDbContext>().InstancePerRequest();
            builder.RegisterType<ApplicationUserStore>().As<IUserStore<ApplicationUser>>().InstancePerRequest();
            builder.RegisterType<ApplicationRoleStore>().As<IRoleStore<ApplicationRole, string>>().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().InstancePerRequest();
            builder.RegisterType<ApplicationRoleManager>().InstancePerRequest();
            builder.Register<IDataProtectionProvider>(c => app.GetDataProtectionProvider()).InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().InstancePerRequest();
            builder.RegisterType<SignInHelper>().InstancePerRequest();
            builder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();

            builder.RegisterType<DbContextScopeFactory>().As<IDbContextScopeFactory>().SingleInstance();
            builder.RegisterType<AmbientDbContextLocator>().As<IAmbientDbContextLocator>().SingleInstance();

            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerRequest();
            builder.RegisterType<RoleRepository>().As<IRoleRepository>().InstancePerRequest();

            builder.RegisterType<UserAdminstrationService>().As<IUserAdminstrationService>().InstancePerRequest();
            builder.RegisterType<UserAdminstrationService>().As<IUserAdminstrationViewModelService>().InstancePerRequest();

            builder.RegisterType<RoleViewModelCommand>().As<IViewModelCommand<RoleSaveModel>>().InstancePerRequest();
            builder.RegisterType<NewUserViewModelCommand>().As<IViewModelCommand<NewUserSaveModel>>().InstancePerRequest();
            builder.RegisterType<UserViewModelCommand>().As<IViewModelCommand<UserSaveModel>>().InstancePerRequest();



            builder.RegisterAssemblyTypes(Asmbls)
                   .Where(t => t.GetInterfaces().Any(x => x == typeof(IResourceAuthProvider)) && !t.IsAbstract)
                   .AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<ResidentSecurity>().As<IResidentSecurity>().SingleInstance();
        }

        private static void WireUpInternals(ContainerBuilder builder)
        {
            builder.RegisterType<MemoryDefaultCacheProvider>().As<ICacheProvider>().SingleInstance();

            builder.RegisterType<Resident>().As<IResident>().SingleInstance();

            //var backofficeMenuProvider = XmlBackOfficeMenuProvider.FromInternalConfig();
            builder.Register((c, p) => XmlBackOfficeMenuProvider.FromInternalConfig())
                .As<IBackOfficeMenuProvider>()
                .SingleInstance();
            builder.Register((c, p) => new ResidentAdministration(c.Resolve<IBackOfficeMenuProvider>() as IMenuProvider<INavigationElements<int>>))
                .As<IResidentAdministration>()
                .SingleInstance();
            builder.RegisterType<ModuleDescovery>().As<IModuleDescovery>().SingleInstance();

            builder.RegisterAssemblyTypes(ScopedAssemblies())
                .Where(t => t.GetInterfaces().Any(i=>i == typeof (IModuleDescriptor)) && !t.IsAbstract)
                .As<IModuleDescriptor>();
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
                .Where(x => x.FullName.StartsWith("Ubik"))
                .ToArray();
        }
    }
}