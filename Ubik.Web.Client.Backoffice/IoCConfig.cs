using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mehdime.Entity;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Owin.Security;
using Owin;
using Ubik.Cache.Runtime;
using Ubik.EF;
using Ubik.Infra.Contracts;
using Ubik.Web.Basis.Contracts;
using Ubik.Web.Basis.Services;
using Ubik.Web.BuildingBlocks.Contracts;
using Ubik.Web.Client.Backoffice.Contracts;
using Ubik.Web.Components.AntiCorruption.Contracts;
using Ubik.Web.Components.AntiCorruption.Services;
using Ubik.Web.Components.AntiCorruption.ViewModels.Devices;
using Ubik.Web.Components.AntiCorruption.ViewModels.Taxonomies;
using Ubik.Web.Components.Contracts;
using Ubik.Web.EF;
using Ubik.Web.EF.Components;
using Ubik.Web.EF.Components.Contracts;
using Ubik.Web.Membership;
using Ubik.Web.Membership.Contracts;
using Ubik.Web.Membership.Managers;
using Ubik.Web.Membership.Repositories;
using Ubik.Web.Membership.Services;
using Ubik.Web.Membership.Stores;
using Ubik.Web.Membership.ViewModels;

namespace Ubik.Web.Client.Backoffice
{
    public static class IoCConfigExtensions
    {
        private static readonly Assembly[] _asmbls = AppDomain.CurrentDomain.GetAssemblies();

        public static void ConfigureBackoffice(this IServiceCollection services, IConfiguration configuration)
        {
            WireUpInternals(services);
            WireUpDbContexts(services, configuration);
            WireUpSso(services);
            WireUpCms(services);
        }

        private static void WireUpInternals(IServiceCollection services)
        {
            services.AddSingleton<IResident, Resident>();
            var backofficeMenu = XmlBackOfficeMenuProvider.FromInternalConfig();
            services.AddInstance(typeof(IBackOfficeMenuProvider), backofficeMenu);
            services.AddInstance(typeof(IResidentAdministration), new ResidentAdministration(backofficeMenu));

            services.AddSingleton<ICacheProvider, MemoryDefaultCacheProvider>();
            services.AddSingleton<IModuleDescovery, ModuleDescovery>();

            var moduleDescriptors = _asmbls
                .SelectMany(a => a.GetTypes())
                .Where(t => t.GetInterfaces().Any(i => i == typeof(IModuleDescriptor)) && !t.IsAbstract);
            foreach (var moduleDescriptor in moduleDescriptors)
            {
                services.AddSingleton(typeof(IModuleDescriptor), moduleDescriptor);
            }

            services.AddScoped<ICRUDRespoditory<PersistedExceptionLog>, PersistedExceptionLogRepository>();
            services.AddScoped<IErrorLogManager, ErrorLogManager>();
        }

        private static void WireUpDbContexts(IServiceCollection services, IConfiguration configuration)
        {
            var cmsConnString = configuration["Data:cmsconnection:ConnectionString"];
            var authConnString = configuration["Data:authconnection:ConnectionString"]; 

            var connectionStrings = new Dictionary<Type, string>
            {
                {typeof (AuthDbContext), authConnString},
                {typeof (ElmahDbContext), cmsConnString},
                {typeof (ComponentsDbContext), cmsConnString}
            };


            var serviceDescriptor = new ServiceDescriptor(typeof(IDbContextFactory), new DbContextFactory(connectionStrings));
            services.Add(serviceDescriptor);

            services.AddSingleton<IDbContextScopeFactory, DbContextScopeFactory>();
            services.AddSingleton<IAmbientDbContextLocator, AmbientDbContextLocator>();
        }

        private static void WireUpSso(IServiceCollection services)
        {
            services.AddScoped<AuthDbContext, AuthDbContext>();

    
            services.AddScoped<IUserStore<ApplicationUser>, ApplicationUserStore>();
            services.AddScoped<IRoleStore<ApplicationRole, string>, ApplicationRoleStore>();
            services.AddScoped<ApplicationUserManager, ApplicationUserManager>();
            services.AddScoped<ApplicationRoleManager, ApplicationRoleManager>();
            //TODO: IDataProtectionProvider
            services.AddScoped<ApplicationSignInManager, ApplicationSignInManager>();
            services.AddScoped<SignInHelper, SignInHelper>();
            services.AddScoped<ApplicationRoleManager, ApplicationRoleManager>();
            services.AddScoped<ApplicationRoleManager, ApplicationRoleManager>();
            //TODO: IAuthenticationManager

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddScoped<IUserAdminstrationService, UserAdminstrationService>();
            services.AddScoped<IUserAdminstrationViewModelService, UserAdminstrationService>();

            services.AddScoped<IViewModelCommand<RoleSaveModel>, RoleViewModelCommand>();
            services.AddScoped<IViewModelCommand<NewUserSaveModel>, NewUserViewModelCommand>();
            services.AddScoped<IViewModelCommand<UserSaveModel>, UserViewModelCommand>();

            var authProviders = _asmbls
                .SelectMany(a => a.GetTypes())
                .Where(t => t.GetInterfaces().Any(i => i == typeof(IResourceAuthProvider)) && !t.IsAbstract);
            foreach (var authProvider in authProviders)
            {
                services.AddSingleton(typeof(IResourceAuthProvider), authProvider);
            }

            services.AddSingleton<IResidentSecurity, ResidentSecurity>();

        }

        private static void WireUpCms(IServiceCollection services)
        {
            services.AddScoped<IPersistedTextualRepository, PersistedTextualRepository>();
            services.AddScoped<ICRUDRespoditory<PersistedDevice>, PersistedDeviceRepository>();
            services.AddScoped<ICRUDRespoditory<PersistedSection>, PersistedSectionRepository>();
            services.AddScoped<IPersistedTaxonomyDivisionRepository, PersistedTaxonomyDivisionRepository>();
            services.AddScoped<IPersistedTaxonomyElementRepository, PersistedTaxonomyElementRepository>();

            services.AddScoped<IDeviceAdministrationService<int>, DeviceAdministrationService>();
            services.AddScoped<IDeviceAdministrationViewModelService, DeviceAdministrationService>();
            services.AddScoped<ITaxonomiesViewModelService, TaxonomiesViewModelService>();

            services.AddScoped<IViewModelCommand<DeviceSaveModel>, DeviceViewModelCommand>();
            services.AddScoped<IViewModelCommand<SectionSaveModel>, SectionViewModelCommand>();
            services.AddScoped<IViewModelCommand<DivisionSaveModel>, DivisionViewModelCommand>();
            services.AddScoped<IViewModelCommand<ElementSaveModel>, ElementViewModelCommand>();

            services.AddSingleton<ISlugifier, SystemSlugService>();
            services.AddSingleton<ISlugWordReplacer, SystemSlugWordRplacer>();
            services.AddSingleton<ISlugCharOmmiter, SystemSlugCharReplacer>();
            services.AddSingleton<IInternationalCharToAsciiProvider, GreekToAsciiProvider>();
        }
    }
}