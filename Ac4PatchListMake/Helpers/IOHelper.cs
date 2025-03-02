using System.IO;

namespace Ac4PatchListMake.Helpers
{
    internal static class IOHelper
    {
        internal static void BackupFile(string path)
        {
            if (File.Exists(path))
            {
                string backupPath = path + ".bak";
                if (!File.Exists(backupPath))
                {
                    File.Move(path, backupPath);
                }
            }
        }
    }
}
