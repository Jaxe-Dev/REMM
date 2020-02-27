using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using REMM.Common;
using REMM.RW;

namespace REMM.WPF.Controls
{
    public class ListViewPlus : ListView
    {
        public static readonly DependencyProperty CanReorderProperty = DependencyProperty.Register(nameof(CanReorder), typeof(bool), typeof(ListViewPlus), new PropertyMetadata(default(bool)));
        public bool CanReorder { get => (bool) GetValue(CanReorderProperty); private set => SetValue(CanReorderProperty, value); }

        private Point _dragStartPoint;

        public ListViewPlus()
        {
            PreviewMouseMove += ListBox_PreviewMouseMove;

            var listViewStyle = (Style) TryFindResource(typeof(ListView));
            if (listViewStyle != null) { Style = new Style(typeof(ListView), listViewStyle); }

            var containerStyle = new Style(typeof(ListViewItem));
            containerStyle.Setters.Add(new Setter(AllowDropProperty, true));
            containerStyle.Setters.Add(new EventSetter(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(ListBoxItem_PreviewMouseLeftButtonDown)));
            containerStyle.Setters.Add(new EventSetter(DropEvent, new DragEventHandler(ListBoxItem_Drop)));

            ItemContainerStyle = containerStyle;
            ItemTemplate = new DataTemplate(typeof(Mod)) { VisualTree = new FrameworkElementFactory(typeof(ModListItem)) };
        }

        private void ListBox_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var list = ItemsSourceList();
            if (list == null) { return; }

            var point = e.GetPosition(null);
            var diff = _dragStartPoint - point;

            if ((e.LeftButton != MouseButtonState.Pressed) || (!(Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance) && !(Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))) { return; }
            var parent = ((DependencyObject) e.OriginalSource).FindVisualParent<ListBoxItem>();
            if (parent != null) { DragDrop.DoDragDrop(parent, parent.DataContext, DragDropEffects.Move); }
        }

        private void ListBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) => _dragStartPoint = e.GetPosition(null);

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            CanReorder = ItemsSourceList() != null;
        }

        private void ListBoxItem_Drop(object sender, DragEventArgs e)
        {
            if (!(sender is ListBoxItem item) || (item.DataContext == null)) { return; }
            var source = e.Data.GetData(item.DataContext.GetType());
            var target = item.DataContext;

            var list = ItemsSourceList();
            if ((source == null) || (target == null) || (list == null)) { return; }

            e.Effects = DragDropEffects.Move;

            list.Move(source, target);
        }

        private IList ItemsSourceList() => ItemsSource as IList;
    }
}
