using System.Windows;
using REMM.RW;

namespace REMM.WPF.Controls
{
    public partial class BadgeIcon
    {
        public static readonly DependencyProperty BadgeProperty = DependencyProperty.Register(nameof(RW.Badge), typeof(Badge), typeof(BadgeIcon), new PropertyMetadata(default(Badge)));
        public static readonly DependencyProperty IsShownProperty = DependencyProperty.Register(nameof(IsShown), typeof(bool), typeof(BadgeIcon), new PropertyMetadata(true));
        public Badge Badge { get => (Badge) GetValue(BadgeProperty); set => SetValue(BadgeProperty, value); }
        public bool IsShown { get => (bool) GetValue(IsShownProperty); set => SetValue(IsShownProperty, value); }

        public BadgeIcon() => InitializeComponent();
    }
}
