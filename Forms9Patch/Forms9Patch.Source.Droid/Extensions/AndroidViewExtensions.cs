using Android.Views;

namespace Forms9Patch.Droid
{
    static class AndroidViewExtensions
    {
        public static T GetClosestAncestor<T>(this View view) where T : class
        {
            while (view?.Parent != null)
            {
                var parent = view.Parent as T;
                if (parent != null)
                    return parent;
                view = view.Parent as View;
            }
            return null;
        }

        public static T GetFurthestAncestor<T>(this View view) where T : class
        {
            T root = view?.Parent as T;
            while (view?.Parent != null)
            {
                var parent = view.Parent as T;
                if (parent != null)
                    root = parent;
                view = view.Parent as View;
            }
            return root;
        }

        public static T GetChild<T>(this ViewGroup viewGroup) where T : class
        {
            T child = null;
            int numVisuals = viewGroup.ChildCount;
            for (int i = 0; i < numVisuals; i++)
            {
                //var v = VisualTreeHelper.GetChild(viewGroup, i);
                var v = viewGroup.GetChildAt(i);
                child = v as T;
                if (child != null)
                    return child;
                if (v is ViewGroup childViewGroup)
                    child = childViewGroup.GetChild<T>();
                if (child != null)
                    return child;
            }
            return child;
        }

    }
}