using System.Collections.Generic;
using System.Linq;
using Ubik.Web.Components;
using Ubik.Web.Components.Domain;

namespace Ubik.Web.EF.Components
{
    internal static class Mapper
    {
        public static Device<int> MapToDomain(PersistedDevice source)
        {
            var result = new Device<int>(source.Id, source.FriendlyName, source.Path);
            foreach (var persistedSection in source.Sections)
            {
                result.AddSection(MapToDomain(persistedSection));
            }
            return result;
        }

        public static Section<int> MapToDomain(PersistedSection source)
        {
            var result = new Section<int>(source.Id, source.Identifier, source.FriendlyName, (DeviceRenderFlavor)source.ForFlavor);

            foreach (var persistedSlot in source.Slots)
            {
                var module = Utility.XmlDeserializeFromString<BasePartialModule>(persistedSlot.ModuleInfo);
                result.DefineSlot(new SectionSlotInfo(source.Identifier, persistedSlot.Enabled, persistedSlot.Ordinal),
                    module);
            }
            return result;
        }

        public static Content<int> MapToDomain(PersistedContent source)
        {
            var result = new Content<int>(source.Id, MapToDomain(source.Textual), source.CanonicalURL);
            result.SetState((ComponentStateFlavor)source.ComponentStateFlavor);
            var metas = Utility.XmlDeserializeFromString<ICollection<Meta>>(source.MetasInfo);
            if (metas != null && metas.Any())
            {
                foreach (var meta in metas)
                {
                    result.Metas.Add(meta);
                }
            }
            return result;
        }

        public static Textual MapToDomain(PersistedTextual source)
        {
            var result = new Textual(source.Subject, source.Summary.ToUTF8(), source.Body.ToUTF8());
            return result;
        }
    }
}