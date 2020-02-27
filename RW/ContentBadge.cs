using System.Windows.Media;

namespace REMM.RW
{
    public class ContentBadge : Badge
    {
        public static readonly ContentBadge Assemblies = new ContentBadge("dlls", Color.FromScRgb(1f, 0f, 0.1f, 0.1f), "Contains assemblies");
        public static readonly ContentBadge Defs = new ContentBadge("defs", Colors.SaddleBrown, "Contains new defs");
        public static readonly ContentBadge Patches = new ContentBadge("patches", Colors.Purple, "Contains xml patches");
        public static readonly ContentBadge Textures = new ContentBadge("textures", Colors.Olive, "Contains new graphics");
        public static readonly ContentBadge Sounds = new ContentBadge("sounds", Colors.DarkSlateBlue, "Contains new audio");

        private ContentBadge(string label, Color color, string description) : base(label, color, 50d, description) { }
    }
}
