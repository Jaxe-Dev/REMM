using System.Collections.Generic;
using System.Windows;
using REMM.RW;

namespace REMM.WPF.Controls
{
    public partial class ModListView
    {
        public static readonly DependencyProperty ModListProperty = DependencyProperty.Register(nameof(ModList), typeof(IEnumerable<Mod>), typeof(ModListView), new PropertyMetadata(default(IEnumerable<Mod>)));
        public static readonly DependencyProperty SelectedModProperty = DependencyProperty.Register(nameof(SelectedMod), typeof(Mod), typeof(ModListView), new PropertyMetadata(default(Mod)));
        public static readonly DependencyProperty DragDropContextProperty = DependencyProperty.Register(nameof(DragDropContext), typeof(string), typeof(ModListView), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty CanDragDropProperty = DependencyProperty.Register(nameof(CanDragDrop), typeof(bool), typeof(ModListView), new PropertyMetadata(false));

        public IEnumerable<Mod> ModList { get => (IEnumerable<Mod>) GetValue(ModListProperty); set => SetValue(ModListProperty, value); }
        public Mod SelectedMod { get => (Mod) GetValue(SelectedModProperty); set => SetValue(SelectedModProperty, value); }
        public string DragDropContext { get => (string) GetValue(DragDropContextProperty); set => SetValue(DragDropContextProperty, value); }
        public bool CanDragDrop { get => (bool) GetValue(CanDragDropProperty); set => SetValue(CanDragDropProperty, value); }

        public ModListView() => InitializeComponent();
    }
}
