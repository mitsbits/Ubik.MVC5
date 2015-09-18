using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Mehdime.Entity;
using Ubik.EF;
using Ubik.Web.Components;
using Ubik.Web.Components.Domain;

namespace Ubik.Web.EF.Components
{
    public abstract class PersistedComponent
    {
        public int Id { get; set; }
        public long ComponentStateFlavor { get; set; }
    }

    public class PersistedTextual
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public byte[] Summary { get; set; }
        public byte[] Body { get; set; }
    }

    public class PersistedContent : PersistedComponent
    {
        public int TextualId { get; set; }
        public virtual PersistedTextual Textual { get; set; }
        public virtual PersistedBrowserAddress BrowserAddress { get; set; }

        
    }

    public class PersistedBrowserAddress 
    {
        public int Id { get; set; }
        public string MetasInfo { get; set; }
        public string CanonicalURL { get; set; }
        public string Slug { get; set; }

    }

    public class PersistedDevice
    {
        public PersistedDevice()
        {
            Sections = new HashSet<PersistedSection>();
        }
        public int Id { get; set; }
        public string FriendlyName { get; set; }
        public string Path { get; set; }
        public ICollection<PersistedSection> Sections { get; set; }
    }

    public class PersistedSection
    {
        public PersistedSection()
        {
            Slots = new HashSet<PersistedSlot>();
        }
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public string Identifier { get; set; }
        public string FriendlyName { get; set; }
        public int ForFlavor { get; set; }
        public ICollection<PersistedSlot> Slots { get; set; }
    }

    public class PersistedSlot
    {
        public int SectionId { get; set; }
        public bool Enabled { get; set; }
        public int Ordinal { get; set; }
        public string Flavor { get; set; }
        public string ModuleInfo { get; set; }
    }

    internal class PersistedContentRepository : ReadWriteRepository<PersistedContent, ComponentsDbContext>
    {
        public PersistedContentRepository(IAmbientDbContextLocator ambientDbContextLocator)
            : base(ambientDbContextLocator)
        {
        }
    }

    internal class PersistedDeviceRepository : ReadWriteRepository<PersistedDevice, ComponentsDbContext>
    {
        public PersistedDeviceRepository(IAmbientDbContextLocator ambientDbContextLocator)
            : base(ambientDbContextLocator)
        {
        }
    }

    internal static class Utility
    {
        /// <summary>
        /// Function to serialize object to xml string using the XmlSerializer
        /// </summary>
        /// <param name="objectInstance">ISerializable: the object to serialize</param>
        /// <returns>string: the xml string with the objects public properties</returns>
        public static string GetXmlString(object objectInstance)
        {
            var serializer = new XmlSerializer(objectInstance.GetType());
            var sb = new StringBuilder();

            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, objectInstance);
            }

            return sb.ToString();
        }



        public static string GetXmlString(object objectInstance, Type[] types)
        {
            var serializer = new XmlSerializer(objectInstance.GetType(), types);
            var sb = new StringBuilder();

            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, objectInstance);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Deserialize an object from xml string
        /// </summary>
        /// <typeparam name="T">Type: the target of invocation</typeparam>
        /// <param name="objectData">string: Xml string</param>
        /// <returns>Object of Type T</returns>
        public static T XmlDeserializeFromString<T>(string objectData)
        {
            return (T)XmlDeserializeFromString(objectData, typeof(T));
        }

        /// <summary>
        /// Deserialize an object from xml string
        /// </summary>
        /// <param name="objectData">string: Xml string</param>
        /// <param name="type">Type: the target of invocation</param>
        /// <returns>Object of Type type</returns>
        public static object XmlDeserializeFromString(string objectData, Type type)
        {
            if (String.IsNullOrWhiteSpace(objectData)) return null;
            var serializer = new XmlSerializer(type);
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }

        /// <summary>
        /// Return a string representation of the buffer data
        /// using  <see cref="System.Text.Encoding.UTF8"/>
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        public static string ToUTF8(this byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return string.Empty;
            return System.Text.Encoding.UTF8.GetString(buffer);
        }
        /// <summary>
        /// Converts a string to a binary value
        /// using UTF8 Encoding
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>buffer</returns>
        public static byte[] ConvertUTF8ToBinary(this string str)
        {
            var encoding = new UTF8Encoding();
            return encoding.GetBytes(str);
        }
    }
}
