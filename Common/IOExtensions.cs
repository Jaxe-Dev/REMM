using System;
using System.Diagnostics;
using System.IO;

namespace REMM.Common
{
    public static class IOExtensions
    {
        public static DirectoryInfo GetSubdirectory(this DirectoryInfo self, string path, bool create = false)
        {
            var directory = new DirectoryInfo(Path.Combine(self.FullName, path));
            if (!create || directory.Exists) { return directory; }

            directory.Create();

            return directory;
        }

        public static T ExistsOrNull<T>(this T self) where T : FileSystemInfo => self.Exists ? self : null;

        public static FileInfo GetFile(this DirectoryInfo self, string path) => new FileInfo(Path.Combine(self.FullName, path));

        public static string GetRelativePath(this FileInfo self, DirectoryInfo root) => !self.IsInside(root) ? null : self.FullName.Substring(root.FullName.Length + 1);

        public static void CopyTo(this FileInfo self, DirectoryInfo root, DirectoryInfo destination, bool overwrite = false)
        {
            var newPath = destination.GetFile(self.GetRelativePath(root) ?? throw new AppException($"File '{self.FullName}' not inside root '{root.FullName}'"));

            newPath.Directory?.Create();
            try { self.CopyTo(newPath.FullName, overwrite); }
            catch { }
        }

        public static string ReadAllText(this FileInfo self) => File.ReadAllText(self.FullName);

        public static T Refreshed<T>(this T self) where T : FileSystemInfo
        {
            self.Refresh();
            return self;
        }

        public static bool IsInside(this FileSystemInfo self, DirectoryInfo other) => self.FullName.StartsWith(other.FullName, StringComparison.OrdinalIgnoreCase);
        public static void OpenInExplorer(this DirectoryInfo self) => Process.Start(self.FullName);
    }
}
