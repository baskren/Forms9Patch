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
        public static T GetClosestAncestor<T>(this FrameworkElement uwpElement) where T : class
        {
            while (uwpElement?.Parent != null)
            {
                if (uwpElement.Parent is T parent)
                    return parent;
                uwpElement = uwpElement.Parent as FrameworkElement;
            }
            return null;
        }

        public static T GetFurthestAncestor<T>(this FrameworkElement uwpElement) where T : class
        {
            var root = uwpElement?.Parent as T;
            while (uwpElement?.Parent != null)
            {
                if (uwpElement.Parent is T parent)
                    root = parent;
                uwpElement = uwpElement.Parent as FrameworkElement;
            }
            return root;
        }

        public static T GetChild<T>(this DependencyObject parent) where T : Windows.UI.Xaml.DependencyObject
        {
            var child = default(T);
            var numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                var v = VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null && v != null)
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
            var child = default(T);
            var numVisuals = VisualTreeHelper.GetChildrenCount(parent);
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

        public static string GenerateHeirachry(this FrameworkElement element, string leader = "", bool last = true)
        {
            var result = leader + (leader.Length > 0 && last ? " └─" : " ├─") + element + "\n";

            leader = leader + (last ? "   " : " │ ");

            var children = element.GetChildren<FrameworkElement>();
            if (children != null)
                for (int i = 0; i < children.Count; i++)
                {
                    var lastChild = i == children.Count - 1;
                    result += children[i].GenerateHeirachry(leader, lastChild);
                }

            return result;
        }
    }
}
