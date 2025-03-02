using Ac4PatchListMake.Crypto;
using Ac4PatchListMake.Helpers;
using System;
using System.IO;
using System.Security.Cryptography;

namespace Ac4PatchListMake
{
    public class PatchFileListBuilder : IDisposable
    {
        private readonly PatchList Patches;
        private readonly string InputFolder;
        private readonly AesCipher Cipher;
        private readonly PatchFileList PatchFileList;
        private bool disposedValue;

        public PatchFileListBuilder(PatchList patchList, string inputFolder, AesCipher cipher)
        {
            Patches = patchList;
            InputFolder = inputFolder;
            Cipher = cipher;
            PatchFileList = new PatchFileList();
        }

        public PatchFileList Build()
        {
            string url = PathHelper.TrimTrailingForwardSlashes(Patches.Url);
            foreach (var file in Patches.Files)
            {
                string inputPath = Path.Combine(InputFolder, file.Path);
                if (!File.Exists(inputPath))
                {
                    throw new FileNotFoundException(inputPath);
                }

                byte[] inputBytes = File.ReadAllBytes(inputPath);
                string fileName = Path.GetFileName(file.Path);
                var encr = new ENCR(fileName, inputBytes);
                byte[] encBytes = Cipher.Encrypt(encr.Write());
                string hash = MD5.HashData(encBytes).ToHex();

                string outputPath = Path.Combine(InputFolder, file.Name);
                string path = PathHelper.ToForwardUnrootedPath(file.Name);
                string pathUrl = $"{url}/{path}";

                string? outputFolder = Path.GetDirectoryName(outputPath);
                if (string.IsNullOrWhiteSpace(outputFolder))
                {
                    throw new DirectoryNotFoundException($"Could not find folder for: {outputPath}");
                }

                Directory.CreateDirectory(outputFolder);
                IOHelper.BackupFile(outputPath);
                File.WriteAllBytes(outputPath, encBytes);
                var patch = new PatchFileList.PatchFile
                {
                    Dir = file.Directory,
                    MD5 = hash,
                    Path = pathUrl,
                    SizeEnc = encBytes.Length.ToString(),
                    SizeOrg = inputBytes.Length.ToString(),
                    Version = file.Version
                };

                PatchFileList.Files.Add(patch);
            }

            return PatchFileList;
        }

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Cipher.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
