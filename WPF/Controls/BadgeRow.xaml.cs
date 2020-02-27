using System.Windows;
using REMM.RW;

namespace REMM.WPF.Controls
{
    public partial class BadgeRow
    {
        public static readonly DependencyProperty ModProperty = DependencyProperty.Register(nameof(RW.Mod), typeof(Mod), typeof(BadgeRow), new PropertyMetadata(default(Mod)));
        public Mod Mod { get => (Mod) GetValue(ModProperty); set => SetValue(ModProperty, value); }

        public BadgeRow() => InitializeComponent();
    }
}
