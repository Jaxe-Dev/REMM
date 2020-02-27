using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using REMM.Common;

namespace REMM.RW
{
    public class Mod : ViewModel
    {
        private static readonly Regex TagRegex = new Regex(@"</?(?:h1|b|i|strike|spoiler|noparse|list|olist|quote=[^>]*|code)>");
        private static readonly Regex DirectoryNameRegex = new Regex("^\\d+\\.\\d+$");

        public string Id { get; }

        public DirectoryInfo Directory { get; }

        public DirectoryInfo AboutDirectory { get; }
        public FileInfo AboutFile { get; }

        public DirectoryInfo AssembliesDirectory { get; }
        public DirectoryInfo DefsDirectory { get; }
        public DirectoryInfo PatchesDirectory { get; }
        public DirectoryInfo TexturesDirectory { get; }
        public DirectoryInfo SoundsDirectory { get; }
        public DirectoryInfo LanguagesDirectory { get; }

        public bool IsFromWorkshop { get; }
        public bool IsValidVersion { get; }

        public bool IsActive => Order >= 0;
        public bool IsOfficial { get; }

        private int _order = -1;
        public int Order { get => _order; set => SetFields(ref _order, value, new []{ nameof(IsActive)}); }

        public DateTime Installed { get; }
        public string InstalledText => Installed.ToShortDateString();
        public DateTime Updated { get; set; }
        public string UpdatedText => Updated.ToShortDateString();

        public string Label { get; }
        public string Author { get; }
        public string Url { get; }
        public string Description { get; }

        public FileInfo PreviewImageFile { get; }
        public ImageSource PreviewImage => PreviewImageFile == null ? null : new BitmapImage(new Uri(PreviewImageFile.FullName));

        public bool HasAssemblies => AssembliesDirectory != null;
        public bool HasDefs => DefsDirectory != null;
        public bool HasPatches => PatchesDirectory != null;
        public bool HasTextures => TexturesDirectory != null;
        public bool HasSounds => SoundsDirectory != null;

        public SourceBadge SourceBadge { get; }
        public string[] SupportedVersions { get; }
        public VersionBadge[] VersionBadges { get; }
        public Badge[] Badges { get; }

        public SolidColorBrush Brush { get; }

        public Mod(DirectoryInfo directory)
        {
            Directory = directory;

            Id = Directory.Name;

            AboutDirectory = Directory.GetSubdirectory("About").ExistsOrNull() ?? throw new AppException($"No About folder found for mod '{Id}'");
            AboutFile = AboutDirectory.GetFile("About.xml").ExistsOrNull() ?? throw new AppException($"No About.xml exists for mod '{Id}'");

            AssembliesDirectory = GetContentDirectory("Assemblies");
            DefsDirectory = GetContentDirectory("Defs");
            PatchesDirectory = GetContentDirectory("Patches");
            TexturesDirectory = GetContentDirectory("Textures");
            SoundsDirectory = GetContentDirectory("Sounds");
            LanguagesDirectory = GetContentDirectory("Languages");

            var metaData = XDocument.Load(AboutFile.FullName).Root;

            if (metaData == null) { throw new AppException($"No meta data found for mod '{Id}'"); }

            Installed = Directory.CreationTime;
            Updated = Directory.LastWriteTime;

            Label = (metaData.Element("name") ?? throw new AppException($"No name found for mod '{Id}'")).Value;
            Author = metaData.Element("author")?.Value;
            if (string.IsNullOrWhiteSpace(Author)) { Author = "(Unknown)"; }

            Url = metaData.Element("url")?.Value;
            Description = RemoveTags(metaData.Element("description")?.Value);

            if (Label != "Core")
            {
                var targetedVersion = metaData.Element("targetVersion")?.Value.Trim();
                SupportedVersions = targetedVersion == null ? GetSupportedVersions(metaData.Element("supportedVersions")) : new[] { targetedVersion };
                VersionBadges = GetVersionBadges(SupportedVersions);
            }
            else
            {
                SupportedVersions = new[] { Static.G.RimWorld.Version };
                VersionBadges = new VersionBadge[] { };
            }

            IsValidVersion = SupportedVersions.Any(version => Static.G.RimWorld.VersionMatches(version));

            PreviewImageFile = Directory.GetFile("About\\Preview.png").ExistsOrNull();

            if (Directory.IsInside(Static.G.RimWorld.LocalModDirectory))
            {
                IsOfficial = true;
                SourceBadge = Author == "Ludeon Studios" ? SourceBadge.Official : SourceBadge.Local;
            }
            else if (Static.G.RimWorld.IsSteam && Directory.IsInside(Static.G.RimWorld.SteamApp.WorkshopDirectory.ExistsOrNull() ?? throw new AppException("Steam Workshop folder not found")))
            {
                SourceBadge = SourceBadge.Steam;
                IsFromWorkshop = true;
            }

            Brush = FMM.ModColors.ContainsKey(Label) && FMM.ModColors[Label].HasValue ? new SolidColorBrush(FMM.ModColors[Label].Value) : null;

            Badges = GetBadges();
        }

        private static string[] GetSupportedVersions(XContainer x) => x?.Elements("li").Select(li => li.Value.Trim()).OrderByDescending(li => li).ToArray() ?? new[] { "?" };
        private static VersionBadge[] GetVersionBadges(IEnumerable<string> versions) => versions.Select(VersionBadge.Create).OrderByDescending(badge => badge.Label).ToArray();

        private Badge[] GetBadges()
        {
            var list = new List<Badge> { SourceBadge };
            list.AddRange(VersionBadges);
            if (HasAssemblies) { list.Add(ContentBadge.Assemblies); }
            if (HasDefs) { list.Add(ContentBadge.Defs); }
            if (HasPatches) { list.Add(ContentBadge.Patches); }
            if (HasTextures) { list.Add(ContentBadge.Textures); }
            if (HasSounds) { list.Add(ContentBadge.Sounds); }

            return list.ToArray();
        }

        private DirectoryInfo GetContentDirectory(string path) => Directory.GetSubdirectory(path).ExistsOrNull() ?? Directory.GetSubdirectory(path).GetSubdirectory(Static.G.RimWorld.Version).ExistsOrNull();
        private static string RemoveTags(string description) => TagRegex.Replace(description, "").Replace("\\n", "\n");
    }
}
