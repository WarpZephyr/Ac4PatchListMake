using System.IO;
using System.Runtime.CompilerServices;

namespace Ac4PatchListMake.Helpers
{
    internal static class PathHelper
    {
        internal static string RemoveRoot(string path)
        {
            int rootIndex = path.IndexOf(':');
            if (rootIndex > -1)
            {
                int nextIndex = rootIndex + 1;
                if (nextIndex < path.Length)
                {
                    return path[(nextIndex)..];
                }
                else
                {
                    return string.Empty;
                }
            }

            return path;
        }

        internal static string ToForwardUnrootedPath(string path)
            => TrimForwardSlashes(NormalizeForwardSlashes(ToForwardSlashes(RemoveRoot(path))));

        internal static string NormalizeForwardSlashes(string path)
        {
            while (path.Contains("//"))
            {
                path = path.Replace("//", "/");
            }

            return path;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string ToForwardSlashes(string path)
            => path.Replace('\\', '/');

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string TrimForwardSlashes(string path)
            => path.Trim('/');

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string TrimTrailingForwardSlashes(string path)
            => path.TrimEnd('/');

        // This also helps for URLs since we don't care about any kind of starting context
        // Only the final bit of the path
        internal static string GetFileName(string path)
        {
            // Try to find last separator
            int index = path.LastIndexOf('/');
            if (index == -1)
                index = path.LastIndexOf('\\');
            if (index == -1)
                index = path.LastIndexOf(Path.DirectorySeparatorChar);
            if (index == -1)
                index = path.LastIndexOf(Path.AltDirectorySeparatorChar);

            if (index > -1)
            {
                // If the last separator is the final thing in the path, return empty
                int finalIndex = index + 1;
                if (finalIndex >= path.Length)
                    return string.Empty;

                return path[finalIndex..];
            }

            // If we didn't find any separators just return the path
            return path;
        }
    }
}
