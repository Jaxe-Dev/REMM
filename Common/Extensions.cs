using System.Collections;
using System.Windows;
using System.Windows.Media;

namespace REMM.Common
{
    public static class Extensions
    {
        public static string CapitalizeFirst(this string self)
        {
            if (string.IsNullOrWhiteSpace(self)) { return self; }
            if (self.Length == 1) { return self.ToUpper(); }
            return char.ToUpper(self[0]) + self.Substring(1);
        }

        public static TDependencyObject FindVisualParent<TDependencyObject>(this DependencyObject self) where TDependencyObject : DependencyObject
        {
            while (true)
            {
                var parentObject = VisualTreeHelper.GetParent(self);
                if (parentObject == null) { return null; }

                if (parentObject is TDependencyObject parent) { return parent; }

                self = parentObject;
            }
        }

        public static void Move(this IList items, object source, object target)
        {
            var sourceIndex = items.IndexOf(source);
            var targetIndex = items.IndexOf(target);

            if (sourceIndex < targetIndex)
            {
                items.Insert(targetIndex + 1, source);
                items.RemoveAt(sourceIndex);
            }
            else
            {
                var removeIndex = sourceIndex + 1;
                if ((items.Count + 1) <= removeIndex) { return; }
                items.Insert(targetIndex, source);
                items.RemoveAt(removeIndex);
            }
        }
    }
}
