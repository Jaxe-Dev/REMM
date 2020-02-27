using System.Windows.Media;
using REMM.Common;

namespace REMM.RW
{
    public class Badge
    {
        public const double DefaultWidth = 60d;

        public string Label { get; }
        public string LabelCap { get; }
        public string LabelAllCaps { get; }
        public string Description { get; }
        public double Width { get; }
        public SolidColorBrush Background { get; }

        public Badge(string label, Color color, double width = DefaultWidth, string description = null)
        {
            Label = label;
            LabelCap = label.CapitalizeFirst();
            LabelAllCaps = label.ToUpper();
            Background = new SolidColorBrush(color);
            Width = width;
            Description = description;
        }
    }
}
