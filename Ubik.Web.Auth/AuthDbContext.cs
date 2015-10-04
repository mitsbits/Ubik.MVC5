using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Ubik.Web.Auth
{
    public class AuthDbContext : IdentityDbContext<ApplicationUser>
    {
        public AuthDbContext()
            : base("authconnectionstring", false)
        {
            
        }

        public DbSet<ApplicationClaim> RoleClaims { get; set; }

        public static AuthDbContext Create()
        {
            return new AuthDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ApplicationClaimConfig());
            base.OnModelCreating(modelBuilder);
        }

        internal class ApplicationClaimConfig : EntityTypeConfiguration<ApplicationClaim>
        {
            public ApplicationClaimConfig()
            {
                ToTable("AspNetRoleClaims");
                HasKey(x => new { x.ApplicationRoleId, x.ClaimType, x.Value });
                HasRequired(x => x.Role)
                    .WithMany(r => r.RoleClaims)
                    .HasForeignKey(x => x.ApplicationRoleId); 
            }
        }
    }
}