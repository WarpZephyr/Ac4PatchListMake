using Ac4PatchListMake.Extensions.Xml;
using System.Collections.Generic;
using System.Xml;

namespace Ac4PatchListMake
{
    public class PatchList
    {
        public List<PatchFile> Files { get; set; }

        public PatchList()
        {
            Files = [];
        }

        public static PatchList FromXml(string xmlPath)
        {
            var xml = new XmlDocument();
            xml.Load(xmlPath);

            XmlNode patchNode = xml.GetNodeOrThrow("patch");
            var patchList = new PatchList();

            XmlNodeList? fileNodes = patchNode.SelectNodes("file");
            if (fileNodes != null)
            {
                foreach (XmlNode fileNode in fileNodes)
                {
                    string path = fileNode.ReadStringOrThrowIfWhiteSpace("path");
                    string download = fileNode.ReadStringOrThrowIfWhiteSpace("download");
                    string dir = fileNode.ReadStringOrDefault("dir", "install:/");
                    string version = fileNode.ReadStringOrDefault("version", "1.0");
                    var patchFile = new PatchFile()
                    {
                        Path = path,
                        Download = download,
                        Directory = dir,
                        Version = version,
                    };

                    patchList.Files.Add(patchFile);
                }
            }

            return patchList;
        }

        public class PatchFile
        {
            public required string Path { get; init; }
            public required string Download { get; init; }
            public required string Directory { get; init; }
            public required string Version { get; init; }
        }
    }
}
