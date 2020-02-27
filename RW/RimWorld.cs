using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using REMM.Common;
using REMM.Steam;

namespace REMM.RW
{
    public class RimWorld : ViewModel
    {
        private static readonly Regex VersionRegex = new Regex("^(\\d+\\.\\d+)");

        public SteamApp SteamApp { get; }
        public bool IsSteam => SteamApp != null;

        public string FullVersion { get; }
        public string Version { get; }

        public DirectoryInfo Directory { get; }
        public DirectoryInfo LocalModDirectory { get; }

        public FileInfo ModsConfigFile => Static.G.Settings.DataDirectory.GetSubdirectory("Config").GetFile("ModsConfig.xml");

        private ObservableCollection<Mod> _mods;
        public ObservableCollection<Mod> Mods { get => _mods; set => SetField(ref _mods, value); }

        private RimWorld(DirectoryInfo directory)
        {
            if (!directory.Exists) { throw new AppException("RimWorld directory not found"); }

            Directory = directory;
            LocalModDirectory = directory.GetSubdirectory("Mods");

            var versionFile = Directory.GetFile("Version.txt");
            FullVersion = versionFile.Exists ? versionFile.ReadAllText().Trim() : throw new AppException("Could not find RimWorld version file");
            Version = TrimVersion(FullVersion);
        }

        private RimWorld(SteamApp steamApp) : this(steamApp.CommonDirectory) => SteamApp = steamApp;

        private RimWorld() : this(GetSteamApp()) { }

        private static SteamApp GetSteamApp() => new SteamApp("294100");

        public static RimWorld Init(DirectoryInfo directory = null)
        {
            try { return directory == null ? new RimWorld() : new RimWorld(directory); }
            catch { return null; }
        }

        public void Refresh()
        {
            Mods = new ObservableCollection<Mod>(GetAllMods());
            RefreshActiveMods();
        }

        private IEnumerable<Mod> GetAllMods() => GetLocalMods().Concat(GetWorkshopMods()).OrderByDescending(mod => mod.Id == "Core").ThenBy(mod => mod.Label);

        private IEnumerable<Mod> GetLocalMods() => GetModsFromDirectory(LocalModDirectory);
        private IEnumerable<Mod> GetWorkshopMods() => GetModsFromDirectory(SteamApp.WorkshopDirectory);

        private static IEnumerable<Mod> GetModsFromDirectory(DirectoryInfo root)
        {
            var list = new List<Mod>();
            if (!root?.Exists ?? true) { return list; }

            foreach (var directory in root.GetDirectories())
            {
                try { list.Add(new Mod(directory)); }
                catch { }
            }
            return list;
        }

        public void RefreshActiveMods()
        {
            var modsConfigFile = ModsConfigFile;
            if (!modsConfigFile.Exists) { throw new AppException("Could not load active mods list"); }

            foreach (var mod in Mods) { mod.Order = -1; }

            var x = XDocument.Load(modsConfigFile.FullName).Root?.Element("activeMods");
            if (x == null) { throw new AppException("Invalid mods config file"); }

            var order = 0;

            foreach (var id in x.Elements().Select(element => element.Value))
            {
                var activeMod = Mods.FirstOrDefault(mod => mod.Id == id);
                if (activeMod == null) { continue; }

                activeMod.Order = order;
                order++;
            }
        }

        public bool VersionMatches(string version) => Version.StartsWith(version);

        public static string TrimVersion(string version)
        {
            var match = VersionRegex.Match(version);
            return match.Success ? match.Groups[1].Value : "?";
        }
    }
}
