using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using Ubik.Web.Components.Query;

namespace Ubik.Web.EF.Components
{
    public class ComponentsDbContext : DbContext
    {
        public DbSet<PersistedDevice> Devices { get; set; }

        //public DbSet<PersistedContent> Contents { get; set; }

        //public DbSet<PersistedTextual> Textuals { get; set; }

        //public DbSet<PersistedHtmlHead> HtmlHeads { get; set; }

        //public DbSet<PersistedTag> Tags { get; set; }

        public ComponentsDbContext()
            : base("cmsconnectionstring")
        {
        }

        public ComponentsDbContext(string connString)
            : base(connString)
        {
        }

        public static ComponentsDbContext Create()
        {
            return new ComponentsDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region Device
            modelBuilder.Configurations.Add(new DeviceConfig());
            modelBuilder.Configurations.Add(new SectionConfig());
            modelBuilder.Configurations.Add(new SlotInfoConfig());
            #endregion
            modelBuilder.Configurations.Add(new ContentConfig());
            modelBuilder.Configurations.Add(new HtmlHeadConfig());
            modelBuilder.Configurations.Add(new TagConfig());
            modelBuilder.Configurations.Add(new TextualConfig());
            base.OnModelCreating(modelBuilder);
        }

        #region Device
        private class DeviceConfig : EntityTypeConfiguration<PersistedDevice>
        {
            public DeviceConfig()
            {
                ToTable("Devices").
                    HasKey(x => new { x.Id })
                    .HasMany(x => x.Sections)
                    .WithOptional()
                    .HasForeignKey(s => s.DeviceId);
                Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            }
        }

        private class SectionConfig : EntityTypeConfiguration<PersistedSection>
        {
            public SectionConfig()
            {
                ToTable("Sections").
                    HasKey(x => new { x.Id })
                    .HasMany(x => x.Slots)
                    .WithOptional()
                    .HasForeignKey(s => new { s.SectionId });
                Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            }
        }

        private class SlotInfoConfig : EntityTypeConfiguration<PersistedSlot>
        {
            public SlotInfoConfig()
            {
                ToTable("Slots").
                    HasKey(x => new {x.SectionId, x.Ordinal});
                
            }
        }
        #endregion

        private class HtmlHeadConfig : EntityTypeConfiguration<PersistedHtmlHead>
        {
            public HtmlHeadConfig()
            {
                ToTable("BrowserAddresses").
                    HasKey(x => new { x.Id });
            }
        }
        private class TagConfig : EntityTypeConfiguration<PersistedTag>
        {
            public TagConfig()
            {
                ToTable("Tags").
                    HasKey(x => new { x.Id });
                HasMany(x => x.Contents)
            .WithMany(x => x.Tags)
               .Map(x =>
               {
                   x.ToTable("Contents_Tags");
                   x.MapLeftKey("TagId");
                   x.MapRightKey("ContentId");
               });
            }
        }
        private class ContentConfig : EntityTypeConfiguration<PersistedContent>
        {
            public ContentConfig()
            {
                ToTable("Contents").
                    HasKey(x => new { x.Id });
                HasRequired(x => x.Textual);
                HasRequired(x => x.HtmlHead);
                HasMany(x => x.Tags)
        .WithMany(x => x.Contents)
            .Map(x =>
            {
                x.ToTable("Contents_Tags");
                x.MapLeftKey("ContentId");
                x.MapRightKey("TagId");
            });
            }
        }

        private class TextualConfig : EntityTypeConfiguration<PersistedTextual>
        {
            public TextualConfig()
            {
                ToTable("Textuals").
                    HasKey(x => new { x.Id }).
                    Property(x => x.Subject).IsRequired();
            }
        }
    }

    public class ComponentsQueryDbContext : DbContext
    {
        public DbSet<DeviceProjection<int>> Devices { get; set; }
        public ComponentsQueryDbContext()
            : base("cmsconnectionstring")
        {
        }

        public static ComponentsQueryDbContext Create()
        {
            return new ComponentsQueryDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new DeviceConfig());
            base.OnModelCreating(modelBuilder);
        }

        private class DeviceConfig : EntityTypeConfiguration<DeviceProjection<int>>
        {
            public DeviceConfig()
            {
                ToTable("DeviceProjections").
                    HasKey(x => new { x.Id });
            }
        }
    }
}