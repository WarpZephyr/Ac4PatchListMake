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
    }
}
