using Ac4PatchListMake.Extensions.Xml;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Ac4PatchListMake
{
    public class PatchList
    {
        private const string EncrName = "_enc.bin";
        public List<PatchFile> Files { get; set; }
        public string Url { get; set; }

        public PatchList(string url)
        {
            Url = url;
            Files = [];
        }

        public static PatchList FromXml(string xmlPath)
        {
            var xml = new XmlDocument();
            xml.Load(xmlPath);

            XmlNode patchNode = xml.GetNodeOrThrow("patch");
            string url = patchNode.ReadStringOrThrowIfWhiteSpace("url");
            var patchList = new PatchList(url);

            XmlNodeList? fileNodes = patchNode.SelectNodes("file");
            if (fileNodes != null)
            {
                foreach (XmlNode fileNode in fileNodes)
                {
                    string path = fileNode.ReadStringOrThrowIfWhiteSpace("path");
                    string name = fileNode.ReadStringOrDefault("name", $"{Path.GetFileNameWithoutExtension(path)}{EncrName}");
                    string dir = fileNode.ReadStringOrDefault("dir", "install:/");
                    string version = fileNode.ReadStringOrDefault("version", "1.0");

                    patchList.Files.Add(new PatchFile(path, name, dir, version));
                }
            }

            return patchList;
        }

        public class PatchFile
        {
            public string Path { get; set; }
            public string Name { get; set; }
            public string Directory { get; set; }
            public string Version { get; set; }

            public PatchFile(string path, string name, string dir, string version)
            {
                Path = path;
                Name = name;
                Directory = dir;
                Version = version;
            }
        }
    }
}
