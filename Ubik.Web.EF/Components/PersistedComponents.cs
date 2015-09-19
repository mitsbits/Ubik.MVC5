﻿using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Ubik.EF;

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
        }

        public virtual int Id { get; set; }

        public virtual string FriendlyName { get; set; }

        public virtual string Path { get; set; }

        public virtual ICollection<PersistedSection> Sections { get; set; }
    }

    public class PersistedSection
    {
        public PersistedSection()
        {
            Slots = new HashSet<PersistedSlot>();
        }

        public virtual int Id { get; set; }

        public virtual int DeviceId { get; set; }

        public virtual string Identifier { get; set; }

        public virtual string FriendlyName { get; set; }

        public virtual int ForFlavor { get; set; }

        public virtual ICollection<PersistedSlot> Slots { get; set; }
    }

    public class PersistedSlot
    {
        public virtual int SectionId { get; set; }

        public virtual bool Enabled { get; set; }

        public virtual int Ordinal { get; set; }

        public virtual string Flavor { get; set; }

        public virtual string ModuleInfo { get; set; }
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