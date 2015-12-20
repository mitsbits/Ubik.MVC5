

using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;


namespace Ubik.Web.SSO
{
    /// <summary>
    /// Base class for the Entity Framework database context used for identity.
    /// </summary>
    public class AuthDbContext : IdentityDbContext<UbikUser, UbikRole, int> { }

    /// <summary>
    /// Base class for the Entity Framework database context used for identity.
    /// </summary>
    /// <typeparam name="TUser">The type of the user objects.</typeparam>
    public class IdentityDbContext<TUser> : IdentityDbContext<TUser, UbikRole, int> where TUser : UbikUser { }

    /// <summary>
    /// Base class for the Entity Framework database context used for identity.
    /// </summary>
    /// <typeparam name="TUser">The type of user objects.</typeparam>
    /// <typeparam name="TRole">The type of role objects.</typeparam>
    /// <typeparam name="TKey">The type of the primary key for users and roles.</typeparam>
    public class IdentityDbContext<TUser, TRole, TKey> : DbContext
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        public IdentityDbContext()
            : base("authconnectionstring")
        {
        }

        public IdentityDbContext(string connString)
            : base(connString)
        {
        }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of Users.
        /// </summary>
        public DbSet<TUser> Users { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of User claims.
        /// </summary>
        public DbSet<IdentityUserClaim<TKey>> UserClaims { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of User logins.
        /// </summary>
        public DbSet<IdentityUserLogin<TKey>> UserLogins { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of User roles.
        /// </summary>
        public DbSet<IdentityUserRole<TKey>> UserRoles { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of roles.
        /// </summary>
        public DbSet<TRole> Roles { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of role claims.
        /// </summary>
        public DbSet<IdentityRoleClaim<TKey>> RoleClaims { get; set; }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Configurations.Add(new UserConfig());
            builder.Configurations.Add(new RoleConfig());
            builder.Configurations.Add(new ApplicationClaimConfig());
            builder.Configurations.Add(new IdentityUserClaimConfig());
            builder.Configurations.Add(new IdentityRoleClaimConfig());
            builder.Configurations.Add(new IdentityUserRoleConfig());
            builder.Configurations.Add(new IdentityUserLoginConfig());
            base.OnModelCreating(builder);
        }

        internal class UserConfig : EntityTypeConfiguration<TUser>
        {
            public UserConfig()
            {
                ToTable("UbikUsers");
                HasKey(u => u.Id);
                Property(u => u.ConcurrencyStamp).IsConcurrencyToken();
                Property(u => u.UserName).HasMaxLength(256);
                Property(u => u.NormalizedUserName).HasMaxLength(256);
                Property(u => u.Email).HasMaxLength(256);
                Property(u => u.NormalizedEmail).HasMaxLength(256);
                HasMany(u => u.Claims);
                HasMany(u => u.Logins);
                HasMany(u => u.Roles);
            }
        }

        internal class RoleConfig : EntityTypeConfiguration<TRole>
        {
            public RoleConfig()
            {
                ToTable("UbikRoles");
                HasKey(r => r.Id);
                Property(r => r.ConcurrencyStamp).IsConcurrencyToken();
                Property(u => u.Name).HasMaxLength(256);
                Property(u => u.NormalizedName).HasMaxLength(256);
                HasMany(r => r.Users);
                HasMany(r => r.Claims);
            }
        }

        internal class ApplicationClaimConfig : EntityTypeConfiguration<ApplicationClaim>
        {
            public ApplicationClaimConfig()
            {
                ToTable("UbikRoleClaims");
                HasKey(x => new { x.ApplicationRoleId, x.ClaimType, x.Value });
                HasRequired(x => x.Role)
                    .WithMany(r => r.RoleClaims)
                    .HasForeignKey(x => x.ApplicationRoleId);
            }
        }

        internal class IdentityUserClaimConfig : EntityTypeConfiguration<IdentityUserClaim<TKey>>
        {
            public IdentityUserClaimConfig()
            {
                HasKey(uc => uc.Id);
                ToTable("UbikUserClaims");
            }
        }

        internal class IdentityRoleClaimConfig : EntityTypeConfiguration<IdentityRoleClaim<TKey>>
        {
            public IdentityRoleClaimConfig()
            {
                HasKey(rc => rc.Id);
                ToTable("UbikRoleClaims");
            }
        }

        internal class IdentityUserRoleConfig : EntityTypeConfiguration<IdentityUserRole<TKey>>
        {
            public IdentityUserRoleConfig()
            {
                HasKey(r => new { r.UserId, r.RoleId });
                ToTable("UbikUserRoles");
            }
        }

        internal class IdentityUserLoginConfig : EntityTypeConfiguration<IdentityUserLogin<TKey>>
        {
            public IdentityUserLoginConfig()
            {
                HasKey(l => new { l.LoginProvider, l.ProviderKey });
                ToTable("UbikUserLogins");
            }
        }
    }
}