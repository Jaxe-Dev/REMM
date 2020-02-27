using System.Windows.Media;

namespace REMM.RW
{
    public class VersionBadge : Badge
    {
        private static readonly Color Matching = Color.FromScRgb(1f, 0f, 0.05f, 0f);
        private static readonly Color NonMatching = Color.FromScRgb(1f, 0.1f, 0.05f, 0.05f);

        private VersionBadge(string label, string full) : base(label, Static.G.RimWorld.VersionMatches(label) ? Matching : NonMatching, 30d, full) { }

        public static VersionBadge Create(string label) => new VersionBadge(RimWorld.TrimVersion(label), label);
    }
}
