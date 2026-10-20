using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Scenes.SessionRework.Scripts.Network.Meta;
using Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Hitboxes;
using Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Hitboxes.Enums;
using Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Hitboxes.Model.Base;
using Scenes.SessionRework.Scripts.Network.Meta.Enums;
using Scenes.SessionRework.Scripts.Network.Tools.NetObjectSerializer.Interfaces;


namespace Scenes.SessionRework.Scripts.Network.Tools.NetObjectSerializer
{
    public class XmlNetObjectSerializer : INetObjectSerializer
    {
        public void Serialize(NetObjectDefinition prefab, string outputPath)
        {
            if (prefab == null)
                throw new ArgumentNullException(nameof(prefab));

            var dto = new NetObjectDto
            {
                Metadata = new NetObjectMetadataDto
                {
                    ObjectType = prefab.ObjectType.ToString(),
                    ObjectName = prefab.ObjectName,
                    Version = 1
                }
            };

            var hitboxes = prefab.GetComponentsInChildren<HitboxMeta>(true);

            var components = new List<ComponentDto>(hitboxes.Length);

            foreach (var hitbox in hitboxes)
            {
                var (shapeType, shapeData) = hitbox.Shape switch
                {
                    HitboxShape.Sphere =>
                        ("Sphere", (object)hitbox.Data.Sphere),

                    HitboxShape.Capsule =>
                        ("Capsule", (object)hitbox.Data.Capsule),

                    HitboxShape.Box =>
                        ("Box", (object)hitbox.Data.Box),

                    _ => throw new NotSupportedException(
                        $"Unsupported hitbox shape: {hitbox.Shape}")
                };

                // Метаданные конкретного компонента.
                var metadata = new HitboxMetadataDto
                {
                    ShapeType = shapeType,
                    Version = 1
                };

                // Создаём отдельный компонент для каждого хитбокса.
                components.Add(new ComponentDto
                {
                    Type = "Hitbox",
                    Data = new[]
                    {
                        SerializeToElement(metadata),
                        SerializeChildrenToElement("Data", shapeData)
                    }
                });
            }

            dto.Components = components.ToArray();

            Directory.CreateDirectory(outputPath);

            var filePath = Path.Combine(
                outputPath,
                prefab.ObjectName + ".xml");

            var serializer = new XmlSerializer(typeof(NetObjectDto));

            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "    ",
                OmitXmlDeclaration = false
            };

            using var writer = XmlWriter.Create(filePath, settings);
            serializer.Serialize(writer, dto);
        }

        private static XmlElement SerializeToElement(object value)
        {
            var document = new XmlDocument();
            var serializer = new XmlSerializer(value.GetType());

            using (var writer = document.CreateNavigator()!.AppendChild())
            {
                serializer.Serialize(writer, value);
            }

            return document.DocumentElement!;
        }

        private static XmlElement SerializeChildrenToElement(
            string elementName,
            object value)
        {
            var source = SerializeToElement(value);

            var document = new XmlDocument();
            var result = document.CreateElement(elementName);

            document.AppendChild(result);

            foreach (XmlNode child in source.ChildNodes)
            {
                result.AppendChild(document.ImportNode(child, true));
            }

            return result;
        }
    }

    [XmlRoot("NetObject")]
    public sealed class NetObjectDto
    {
        public NetObjectMetadataDto Metadata { get; set; } = new();

        [XmlArray("Components")]
        [XmlArrayItem("Component")]
        public ComponentDto[] Components { get; set; }
            = Array.Empty<ComponentDto>();
    }

    [Serializable]
    public sealed class NetObjectMetadataDto
    {
        public string ObjectType { get; set; } = string.Empty;

        public string ObjectName { get; set; } = string.Empty;

        public int Version { get; set; }
    }

    [Serializable]
    public sealed class ComponentDto
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = string.Empty;

        [XmlAnyElement]
        public XmlElement[] Data { get; set; }
            = Array.Empty<XmlElement>();
    }

    [Serializable]
    [XmlRoot("Metadata")]
    public sealed class HitboxMetadataDto
    {
        public string ShapeType { get; set; } = string.Empty;

        public int Version { get; set; }
    }
}
