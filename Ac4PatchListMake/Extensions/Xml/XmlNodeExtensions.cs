using System.IO;
using System.Xml;

namespace Ac4PatchListMake.Extensions.Xml
{
    public static class XmlNodeExtensions
    {
        #region Node

        public static XmlNode GetNodeOrThrow(this XmlNode node, string xpath)
            => node.SelectSingleNode(xpath) ?? throw new InvalidDataException($"Node {node.Name} does not contain: {xpath}");

        #endregion

        #region String

        public static string ReadString(this XmlNode node, string xpath)
            => node.GetNodeOrThrow(xpath).InnerText;

        public static string ReadStringOrDefault(this XmlNode node, string xpath, string defaultValue = "")
        {
            XmlNode? child = node.SelectSingleNode(xpath);
            return child?.InnerText ?? defaultValue;
        }

        public static string? ReadStringIfExists(this XmlNode node, string xpath)
            => node.SelectSingleNode(xpath)?.InnerText;

        public static string ReadStringOrThrowIfEmpty(this XmlNode node, string xpath)
        {
            string value = node.ReadString(xpath);
            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidDataException($"{xpath} cannot be null or empty.");
            }
            return value;
        }

        public static string ReadStringOrThrowIfWhiteSpace(this XmlNode node, string xpath)
        {
            string value = node.ReadString(xpath);
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidDataException($"{xpath} cannot be null, empty, or whitespace.");
            }
            return value;
        }

        #endregion

    }
}
