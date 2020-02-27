using System.Windows;
using REMM.Common;
using REMM.RW;

namespace REMM.WPF.Controls
{
    public partial class ModListItem
    {
        public ModListItem() => InitializeComponent();

        private void MenuItemGoToFolder_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(DataContext is Mod mod)) { return; }
            mod.Directory.OpenInExplorer();
        }
    }
}
