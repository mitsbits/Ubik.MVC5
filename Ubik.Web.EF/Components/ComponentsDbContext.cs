using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ubik.Web.Components.Domain;

namespace Ubik.Web.EF.Components
{
    public class ComponentsDbContext : DbContext
    {
        public DbSet<Device<int>> Devices { get; set; }
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
            base.OnModelCreating(modelBuilder);
        }

        internal class DeviceConfig : EntityTypeConfiguration<Device<int>>
        {
            public DeviceConfig()
            {
                ToTable("Devices").
                    HasKey(x => new { x.Id }).
                    HasMany(x => x.Sections);

            }
        }

        internal class SectionConfig : EntityTypeConfiguration<Section<int>>
        {
            public SectionConfig()
            {
                ToTable("Sections").
                    HasKey(x => new { x.Id }).
                    HasMany(x => x.Slots);

            }
        }

        internal class SlotInfoConfig : EntityTypeConfiguration<SectionSlotInfo>
        {
            public SlotInfoConfig()
            {
                //ToTable("Slots").
                //    HasKey(x => new { x.}).
                //    HasMany(x => x.Slots);

            }
        }
    }
}
