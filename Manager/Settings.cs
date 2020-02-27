using System;
using System.IO;
using System.Xml.Linq;
using REMM.Common;

namespace REMM.Manager
{
    public class Settings : ViewModel
    {
        private const string RWDirectoryKey = "RimWorldDirectory";
        private const string RWDataDirectoryKey = "RimWorldDataDirectory";
        private const string UseFMMColorsKey = "UseFMMColors";

        public FileInfo File { get; }

        private DirectoryInfo _gameDirectory;
        public DirectoryInfo GameDirectory { get => _gameDirectory; set => SetField(ref _gameDirectory, value); }

        private DirectoryInfo _dataDirectory;
        public DirectoryInfo DataDirectory
        {
            get => _dataDirectory;
            set
            {
                if (SetFields(ref _dataDirectory, value, new[] { nameof(ConfigDirectory) })) { ConfigDirectory = DataDirectory.GetSubdirectory("Config"); }
            }
        }

        public DirectoryInfo ConfigDirectory { get; private set; }

        private bool _useFMMColors;
        public bool UseFMMColors { get => _useFMMColors; set => SetField(ref _useFMMColors, value); }

        public Settings(FileInfo file)
        {
            File = file;
            if (!File.Exists)
            {
                DataDirectory = GetDefaultDataDirectory();
                Save();
                return;
            }

            var x = XDocument.Load(File.FullName).Root;
            if (x == null) { throw new AppException("Invalid config file"); }

            var gameDirectory = x.Element(RWDirectoryKey)?.Value;
            var dataDirectory = x.Element(RWDataDirectoryKey)?.Value;

            GameDirectory = string.IsNullOrWhiteSpace(gameDirectory) ? null : new DirectoryInfo(gameDirectory);
            DataDirectory = string.IsNullOrWhiteSpace(dataDirectory) ? null : new DirectoryInfo(dataDirectory);

            UseFMMColors = x.Element(UseFMMColorsKey)?.Value.Equals(bool.TrueString, StringComparison.OrdinalIgnoreCase) ?? false;
        }

        public void Save()
        {
            var x = new XElement("Config");
            if (!Static.G.RimWorld?.IsSteam ?? false) { x.Add(new XElement(RWDirectoryKey, GameDirectory.FullName)); }
            x.Add(new XElement(RWDataDirectoryKey, DataDirectory));
            if (UseFMMColors) { x.Add(new XElement(UseFMMColorsKey, UseFMMColors)); }

            new XDocument(x).Save(File.FullName);
        }

        private static DirectoryInfo GetDefaultDataDirectory() => new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "Low", "Ludeon Studios\\RimWorld by Ludeon Studios"));
    }
}
