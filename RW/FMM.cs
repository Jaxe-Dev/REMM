using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Xml.Linq;
using REMM.Common;

namespace REMM.RW
{
    public static class FMM
    {
        public static readonly Dictionary<string, Color?> ModColors;

        public static bool UseColors { get; } = false;

        static FMM() => ModColors = GetModColors();

        private static Dictionary<string, Color?> GetModColors()
        {
            var file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData\\LocalLow\\Ludeon Studios\\RimWorld by Ludeon Studios\\Config\\Mod_1507748539_ModManager.xml");
            if (!File.Exists(file)) { return null; }

            var settings = XDocument.Load(file).Root;

            var mods = settings?.Element("ModSettings")?.Element("ButtonAttributes");
            if ((mods == null) || !mods.HasElements) { return null; }

            return mods.Elements().ToDictionary(mod => mod.Element("Name")?.Value ?? throw new AppException("Mod ID not listed"), mod => ColorFromRgbaString(mod.Element("Color")?.Value));
        }

        private static Color? ColorFromRgbaString(string value)
        {
            if (!UseColors) { return null; }

            var match = new Regex(@"RGBA\(([0-9]+\.[0-9]+), ([0-9]+\.[0-9]+), ([0-9]+\.[0-9]+), ([0-9]+\.[0-9]+)\)").Match(value);
            if (!match.Success) { return null; }

            byte GetColorByte(Capture capture) => (byte) Math.Round(double.Parse(capture.Value, CultureInfo.InvariantCulture) * 255);

            var red = GetColorByte(match.Groups[1]);
            var green = GetColorByte(match.Groups[2]);
            var blue = GetColorByte(match.Groups[3]);
            var alpha = GetColorByte(match.Groups[4]);

            return Color.FromArgb(alpha, red, green, blue);
        }
    }
}
