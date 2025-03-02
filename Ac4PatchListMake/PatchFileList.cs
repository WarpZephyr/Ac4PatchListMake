using System.Collections.Generic;
using System.Text;

namespace Ac4PatchListMake
{
    public class PatchFileList
    {
        private const string ClassName = "Patch.List";
        public List<PatchFile> Files { get; set; }
        public PatchFileList()
        {
            Files = [];
        }

        public string Write()
        {
            var sb = new StringBuilder();
            sb.Append(WriteProperty("Count", Files.Count.ToString()));

            for (int i = 0; i < Files.Count; i++)
            {
                var file = Files[i];
                sb.Append(WriteFileProperty("Dir", file.Dir, i));
                sb.Append(WriteFileProperty("MD5", file.MD5, i));
                sb.Append(WriteFileProperty("Path", file.Path, i));
                sb.Append(WriteFileProperty("SizeEnc", file.SizeEnc, i));
                sb.Append(WriteFileProperty("SizeOrg", file.SizeOrg, i));
                sb.Append(WriteFileProperty("Version", file.Version, i));
            }

            return sb.ToString();
        }

        private static string WriteFileProperty(string name, string value, int index)
            => WriteProperty($"File{index}.{name}", value);

        private static string WriteProperty(string name, string value)
            => $"{ClassName}.{name}\t = \t{value}\n";

        public class PatchFile
        {
            public required string Dir { get; init; }
            public required string MD5 { get; init; }
            public required string Path { get; init; }
            public required string SizeEnc { get; init; }
            public required string SizeOrg { get; init; }
            public required string Version { get; init; }
        }
    }
}
