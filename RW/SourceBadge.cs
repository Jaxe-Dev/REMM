using System.Windows.Media;

namespace REMM.RW
{
    public class SourceBadge : Badge
    {
        public static readonly SourceBadge Official = new SourceBadge("official", Color.FromScRgb(1f, 0.2f, 0f, 0f), "Official game data");
        public static readonly SourceBadge Local = new SourceBadge("local", Color.FromScRgb(1f, 0.075f, 0.025f, 0f), "Found in the mods folder");
        public static readonly SourceBadge Steam = new SourceBadge("workshop", Color.FromScRgb(1f, 0f, 0f, 0.1f), "Subscribed on Steam Workshop");

        private SourceBadge(string label, Color color, string description) : base(label, color, 50d, description) { }
    }
}
