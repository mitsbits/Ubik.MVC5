using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Ubik.Web.EF.Components
{
    public class ComponentsDbContext : DbContext
    {
        public DbSet<PersistedDevice> Devices { get; set; }
        public DbSet<PersistedContent> Contents { get; set; }
        public DbSet<PersistedTextual> Textuals { get; set; }
        public ComponentsDbContext()
            : base("cmsconnectionstring")
        {
        }



        public static ComponentsDbContext Create()
        {
            return new ComponentsDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new DeviceConfig());
            modelBuilder.Configurations.Add(new SectionConfig());
            modelBuilder.Configurations.Add(new SlotInfoConfig());
            modelBuilder.Configurations.Add(new ContentConfig());
            base.OnModelCreating(modelBuilder);
        }

        internal class DeviceConfig : EntityTypeConfiguration<PersistedDevice>
        {
            public DeviceConfig()
            {
                ToTable("Devices").
                    HasKey(x => new { x.Id }).
                    HasMany(x => x.Sections);

            }
        }

        internal class SectionConfig : EntityTypeConfiguration<PersistedSection>
        {
            public SectionConfig()
            {
                ToTable("Sections").
                    HasKey(x => new { x.Id }).
                    HasMany(x => x.Slots);

            }
        }

        internal class SlotInfoConfig : EntityTypeConfiguration<PersistedSlot>
        {
            public SlotInfoConfig()
            {
                ToTable("Slots").
                    HasKey(x => new {x.SectionId, x.Ordinal});
              
            }
        }

        internal class ContentConfig : EntityTypeConfiguration<PersistedContent>
        {
            public ContentConfig()
            {
                ToTable("Contents").
                    HasKey(x => new {x.Id}).
                    HasRequired(x => x.Textual);

            }
        }
        internal class TextualConfig : EntityTypeConfiguration<PersistedTextual>
        {
            public TextualConfig()
            {
                ToTable("Textuals").
                    HasKey(x => new { x.Id }).
                    HasRequired(x => x.Subject);

            }
        }
    }
}
