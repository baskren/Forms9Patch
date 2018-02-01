using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Forms9Patch.UWP
{
    static class FrameworkElementExtensions
    {
        public static T GetParent<T>(this FrameworkElement uwpElement) where T:class
        {
            while (uwpElement?.Parent != null)
            {
                var parent = uwpElement.Parent as T;
                if (parent != null)
                    return parent;
                uwpElement = uwpElement.Parent as FrameworkElement;
            }
            return null;
        }

        public static T GetProginator<T>(this FrameworkElement uwpElement) where T: class
        {
            T root = null;
            while (uwpElement?.Parent != null)
            {
                var parent = uwpElement.Parent as T;
                if (parent != null)
                    root = parent;
                uwpElement = uwpElement.Parent as FrameworkElement;
            }
            return root;
        }

        public static T GetChild<T>(this DependencyObject parent) where T : Windows.UI.Xaml.DependencyObject
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                var v = VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null && v!=null)
                    child = GetChild<T>(v);
                if (child != null)
                    break;
            }
            return child;
        }

        /*
        public static List<FrameworkElement> GetChildren(this FrameworkElement parent)
        {
            var result = new List<FrameworkElement>();
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i) as FrameworkElement;
                result.Add(child);
            }
            return result;
        }
        */

        public static List<Windows.UI.Xaml.Controls.ListViewItem> GetItemViews(this Windows.UI.Xaml.Controls.ListView listView)
        {
            return listView.GetChildren<Windows.UI.Xaml.Controls.ListViewItem>();
        }

        public static List<T> GetChildren<T>(this DependencyObject parent) where T : Windows.UI.Xaml.DependencyObject
        {
            var results = new List<T>();
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                var v = VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null && v != null)
                {
                    var children = GetChildren<T>(v);
                    results.AddRange(children);
                }
                if (child != null)
                    results.Add(child);
            }
            return results;
        }
    }
}
