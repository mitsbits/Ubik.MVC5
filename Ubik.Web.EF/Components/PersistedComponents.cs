using System.Data.Entity;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Xml.Schema;
using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Ubik.EF;
using Ubik.Infra.Contracts;
using System.Linq;
using Ubik.Web.Components;

namespace Ubik.Web.EF.Components
{
    public abstract class PersistedComponent
    {
        public virtual int Id { get; set; }

        public virtual long ComponentStateFlavor { get; set; }
    }

    public class PersistedTextual
    {
        public virtual int Id { get; set; }

        public virtual string Subject { get; set; }

        public virtual byte[] Summary { get; set; }

        public virtual byte[] Body { get; set; }
    }

    public class PersistedContent : PersistedComponent
    {
        public PersistedContent()
        {
            Tags = new HashSet<PersistedTag>();
        }

        public virtual int TextualId { get; set; }

        public virtual PersistedTextual Textual { get; set; }

        public virtual PersistedHtmlHead HtmlHead { get; set; }

        public virtual ICollection<PersistedTag> Tags { get; set; }
    }

    public class PersistedTag : PersistedComponent
    {
        public PersistedTag()
        {
            Contents = new HashSet<PersistedContent>();
        }
        public virtual string Value { get; set; }
        public virtual ICollection<PersistedContent> Contents { get; set; } 
    }

    public class PersistedHtmlHead
    {
        public virtual int Id { get; set; }

        public virtual string MetasInfo { get; set; }

        public virtual string CanonicalURL { get; set; }

        public virtual string Slug { get; set; }
    }

    public class PersistedDevice
    {
        public PersistedDevice()
        {
            Sections = new HashSet<PersistedSection>();
            Flavor = DeviceRenderFlavor.Empty;
        }

        public  int Id { get; set; }

        public  string FriendlyName { get; set; }

        public  string Path { get; set; }

        public  DeviceRenderFlavor Flavor { get; set; }

        public  ICollection<PersistedSection> Sections { get; set; }
    }

    public class PersistedSection
    {
        public PersistedSection()
        {
            Slots = new HashSet<PersistedSlot>();
        }

        public  int Id { get; set; }

        public  int DeviceId { get; set; }

        public  string Identifier { get; set; }

        public  string FriendlyName { get; set; }

        public  DeviceRenderFlavor ForFlavor { get; set; }

        public  ICollection<PersistedSlot> Slots { get; set; }
    }

    public class PersistedSlot
    {
        public  int SectionId { get; set; }

        public  bool Enabled { get; set; }

        public  int Ordinal { get; set; }

        public  string ModuleType { get; set; }

        public  string ModuleInfo { get; set; }

        public virtual PersistedSection Section { get; set; }
    }

    internal class PersistedContentRepository : ReadWriteRepository<PersistedContent, ComponentsDbContext>
    {
        public PersistedContentRepository(IAmbientDbContextLocator ambientDbContextLocator)
            : base(ambientDbContextLocator)
        {
        }
    }

    public class PersistedDeviceRepository : ReadWriteRepository<PersistedDevice, ComponentsDbContext>, ICRUDRespoditory<PersistedDevice>
    {
        public PersistedDeviceRepository(IAmbientDbContextLocator ambientDbContextLocator)
            : base(ambientDbContextLocator)
        {
        }

        public override async Task<PersistedDevice> GetAsync(Expression<Func<PersistedDevice, bool>> predicate)
        {
            var db = DbContext.Set<PersistedDevice>();
            return await db.Include("Sections").Include("Sections.Slots").FirstOrDefaultAsync(predicate);
        }
    }

    public class PersistedSectionRepository : ReadWriteRepository<PersistedSection, ComponentsDbContext>, ICRUDRespoditory<PersistedSection>
    {
        public PersistedSectionRepository(IAmbientDbContextLocator ambientDbContextLocator)
            : base(ambientDbContextLocator)
        {
        }

        public override async Task<PersistedSection> GetAsync(Expression<Func<PersistedSection, bool>> predicate)
        {
            var db = DbContext.Set<PersistedSection>();
            return await db.Include("Slots").FirstOrDefaultAsync(predicate);
        }
    }


}