using System.IO;
using Microsoft.Win32;
using REMM.Common;

namespace REMM.Steam
{
    public static class Steam
    {
        public static DirectoryInfo InstallDirectory { get; }
        public static DirectoryInfo AppDirectory { get; }
        public static DirectoryInfo CommonDirectory { get; }
        public static DirectoryInfo WorkshopDirectory { get; }

        static Steam()
        {
            InstallDirectory = new DirectoryInfo(GetInstallPathFromRegistry() ?? throw new AppException("Steam install path not found in registry"));
            AppDirectory = InstallDirectory.GetSubdirectory("steamapps");
            CommonDirectory = AppDirectory.GetSubdirectory("common");
            WorkshopDirectory = AppDirectory.GetSubdirectory("workshop");
        }

        private static string GetInstallPathFromRegistry()
        {
            var x64 = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Valve\\Steam", "InstallPath", null)?.ToString();
            var x86 = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Valve\\Steam", "InstallPath", null)?.ToString();

            return x64 ?? x86;
        }
    }
}
