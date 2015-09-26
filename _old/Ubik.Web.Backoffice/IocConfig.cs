//using Autofac;
//using Mehdime.Entity;
//using Microsoft.AspNet.Identity;
//using Ubik.Web.Auth;
//using Ubik.Web.Auth.Contracts;
//using Ubik.Web.Auth.Managers;
//using Ubik.Web.Auth.Repositories;
//using Ubik.Web.Auth.Services;
//using Ubik.Web.Auth.Stores;
//using Ubik.Web.Auth.ViewModels;

//namespace Ubik.Web.Backoffice
//{
//    public class IocConfig : Module
//    {
//        protected override void Load(ContainerBuilder builder)
//        {
//            builder.Register(c => new ApplicationUserManager(new ApplicationUserStore(new AuthDbContext())))
//                .Named<ApplicationUserManager>("transient");
//            builder.Register(c => new ApplicationRoleManager(new ApplicationRoleStore(new AuthDbContext())))
//                .Named<ApplicationRoleManager>("transient");

//            builder.RegisterType<AuthDbContext>().As<AuthDbContext>().InstancePerRequest();
//            builder.RegisterType<ApplicationUserStore>().As<IUserStore<ApplicationUser>>().InstancePerRequest();
//            builder.RegisterType<ApplicationRoleStore>().As<IRoleStore<ApplicationRole, string>>().InstancePerRequest();
//            builder.RegisterType<ApplicationUserManager>().InstancePerRequest();
//            builder.RegisterType<ApplicationRoleManager>().InstancePerRequest();

//            builder.RegisterType<DbContextScopeFactory>().As<IDbContextScopeFactory>().SingleInstance();
//            builder.RegisterType<AmbientDbContextLocator>().As<IAmbientDbContextLocator>().SingleInstance();

//            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerRequest();
//            builder.RegisterType<RoleRepository>().As<IRoleRepository>().InstancePerRequest();

//            builder.RegisterType<UserAdminstrationService>().As<IUserAdminstrationService>().InstancePerRequest();
//            builder.RegisterType<UserAdminstrationViewModelService>()
//                .As<IUserAdminstrationViewModelService>()
//                .InstancePerRequest();

//            builder.RegisterType<UserViewModelBuilder>().As<IViewModelBuilder<ApplicationUser, UserViewModel>>().InstancePerRequest();
//            builder.RegisterType<RoleViewModelBuilder>().As<IViewModelBuilder<ApplicationRole, RoleViewModel>>().InstancePerRequest();
//            builder.RegisterType<AddUserViewModelBuilder>().As<IViewModelBuilder<ApplicationUser, AddUserViewModel>>().InstancePerRequest();
//        }
//    }
//}