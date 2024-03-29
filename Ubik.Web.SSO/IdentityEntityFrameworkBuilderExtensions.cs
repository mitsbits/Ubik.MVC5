using Microsoft.AspNet.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Data.Entity;
using Ubik.Web.SSO.Contracts;
using Ubik.Web.SSO.Stores;

namespace Ubik.Web.SSO
{
    public static class IdentityEntityFrameworkBuilderExtensions
    {


        public static IdentityBuilder AddUbikStores(this IdentityBuilder builder)
        {
            return builder.AddEntityFrameworkStores<AuthDbContext, int>();
        }

        private static IdentityBuilder AddEntityFrameworkStores<TContext, TKey>(this IdentityBuilder builder)
            where TContext : DbContext
            where TKey : IEquatable<TKey>
        {
            builder.Services.TryAdd(GetDefaultServices(builder.UserType, builder.RoleType, typeof(TContext), typeof(TKey)));
            return builder;
        }

        private static IServiceCollection GetDefaultServices(Type userType, Type roleType, Type contextType, Type keyType = null)
        {
            Type userStoreType;
            Type roleStoreType;
            if (keyType != null)
            {
                userStoreType = typeof(UserStore).MakeGenericType(userType, roleType, contextType, keyType);
                roleStoreType = typeof(RoleStore).MakeGenericType(roleType, contextType, keyType);
            }
            else
            {
                userStoreType = typeof(UserStore).MakeGenericType(userType, roleType, contextType);
                roleStoreType = typeof(RoleStore).MakeGenericType(roleType, contextType);
            }

            var services = new ServiceCollection();
            services.AddScoped(
                typeof(IUserStore<>).MakeGenericType(userType),
                userStoreType);
            services.AddScoped(
                typeof(IRoleStore<>).MakeGenericType(roleType),
                roleStoreType);
            services.AddScoped(
                typeof(IUserStoreWithCustomClaims<int>), userStoreType);
            services.AddScoped(
                typeof(IRoleStoreWithCustomClaims), roleStoreType);
            return services;
        }

        private static IdentityBuilder AddEntityFrameworkStores<TContext>(this IdentityBuilder builder)
            where TContext : DbContext
        {
            builder.Services.TryAdd(GetDefaultServices(builder.UserType, builder.RoleType, typeof(TContext)));
            return builder;
        }
    }
}