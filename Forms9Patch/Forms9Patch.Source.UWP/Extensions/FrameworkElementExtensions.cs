using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

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
    }
}
