using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using REMM.Common;

namespace REMM.Steam
{
    public class SteamApp
    {
        public string Id { get; }
        public string Label { get; }

        public DirectoryInfo CommonDirectory { get; }
        public DirectoryInfo WorkshopDirectory { get; }

        public SteamApp(string id)
        {
            Id = id;

            var manifest = GetManifestData() ?? throw new AppException($"No manifest found for app {Id}");
            Label = manifest.ContainsKey("name") ? manifest["name"] : throw new AppException($"No name found for app {Id}");

            var installPath = manifest.ContainsKey("installdir") ? manifest["installdir"] : throw new AppException($"No install directory found for app {Id}");

            CommonDirectory = Path.IsPathRooted(installPath) ? new DirectoryInfo(installPath) : Steam.CommonDirectory.GetSubdirectory(installPath);
            WorkshopDirectory = Steam.WorkshopDirectory.GetSubdirectory("content").GetSubdirectory(Id);
        }

        private Dictionary<string, string> GetManifestData()
        {
            var file = Steam.AppDirectory.GetFile($"appmanifest_{Id}.acf");
            if (!file.Exists) { return null; }

            var data = file.ReadAllText();
            var regex = new Regex("^\\s*\\\"([^\\\"]+)\\\"\\s*\\\"([^\\\"]+)\\\"\\s*$", RegexOptions.Multiline);
            var matches = regex.Matches(data).OfType<Match>();

            return matches.Where(match => match.Success).ToDictionary(match => match.Groups[1].Value, match => match.Groups[2].Value);
        }
    }
}
