using Ac4PatchListMake.Crypto;
using Ac4PatchListMake.Helpers;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Ac4PatchListMake
{
    internal class Program
    {
        const string PatchFileName = "patch.xml";
        const string FileListName = "FileList.properties";
        const string EncFileListName = "Contents.bin";
        const string KeyFileName = "key.txt";
        const string IvFileName = "iv.txt";

        static bool ShowWarning;
        static bool ShowError;

        static void Main(string[] args)
        {
#if !DEBUG
            try
            {
#endif
            Process(args);
#if !DEBUG
            }
            catch (Exception ex)
            {
                Error($"An error occurred: {ex}");
            }
#endif

            Console.WriteLine("Finished.");
            if (ShowWarning || ShowError)
            {
                Console.ReadKey();
            }
        }

        #region Processing

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void Process(string[] args)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string? programFolder = Path.GetDirectoryName(path);
            if (string.IsNullOrWhiteSpace(programFolder))
            {
                Error("Could not find program folder.");
                return;
            }

            string keyPath = Path.Combine(programFolder, KeyFileName);
            if (!File.Exists(keyPath))
            {
                Error($"Could not find {KeyFileName} in program folder.");
                return;
            }

            string ivPath = Path.Combine(programFolder, IvFileName);
            if (!File.Exists(ivPath))
            {
                Error($"Could not find {IvFileName} in program folder.");
                return;
            }

            byte[] key = File.ReadAllText(keyPath).HexToBytes();
            byte[] iv = File.ReadAllText(ivPath).HexToBytes();
            var cipher = new AesCipher(key, iv, System.Security.Cryptography.CipherMode.CBC, System.Security.Cryptography.PaddingMode.Zeros, 128, 128);
            foreach (string arg in args)
            {
#if !DEBUG
                try
                {
#endif
                ProcessArgument(arg, cipher);
#if !DEBUG
                }
                catch (Exception ex)
                {
                    Error($"An unknown error occurred while processing: {arg}\nError: {ex}");
                }
#endif
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void ProcessArgument(string arg, AesCipher cipher)
        {
            if (Directory.Exists(arg))
            {
                ProcessDirectory(arg, cipher);
            }
            else
            {
                Warn($"Folder not could be found at: {arg}");
            }
        }

        static void ProcessDirectory(string folder, AesCipher cipher)
        {
            string patchPath = Path.Combine(folder, PatchFileName);
            if (!File.Exists(patchPath))
            {
                Error($"Could not find {PatchFileName} in folder: {folder}");
                return;
            }

            var patchList = PatchList.FromXml(patchPath);
            var builder = new PatchFileListBuilder(patchList, folder, cipher);
            var fileList = builder.Build();
            var fileListText = fileList.Write();

            var outPath = Path.Combine(folder, FileListName);
            IOHelper.BackupFile(outPath);
            File.WriteAllText(outPath, fileListText);

            var encr = new ENCR(FileListName, Encoding.UTF8.GetBytes(fileListText));
            var encOutPath = Path.Combine(folder, EncFileListName);
            IOHelper.BackupFile(encOutPath);
            File.WriteAllBytes(encOutPath, cipher.Encrypt(encr.Write()));
        }

        #endregion

        #region Logging

        static void Warn(string message)
        {
            Console.WriteLine($"Warn: {message}");
            ShowWarning = true;
        }

        static void Error(string message)
        {
            Console.WriteLine($"Error: {message}");
            ShowError = true;
        }

        #endregion
    }
}
