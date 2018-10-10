using System;
using Xamarin.Forms;
using System.Linq;

namespace Forms9Patch
{
    internal static class PageExtensions
    {
        public static Page FindCurrentPage(Page parent)
        {
            if (parent == null)
                return parent;
            if (parent is MasterDetailPage)
                return parent;
            if (parent is TabbedPage)
                return parent;
            if (parent is CarouselPage)
                return parent;
            var modalStack = parent.NavigationProxy?.ModalStack;
            if (modalStack != null && modalStack.Count > 0)
            {
                for (int i = modalStack.Count - 1; i >= 0; i--)
                    if (modalStack[i] is Page popupPage)
                        return popupPage == parent ? popupPage : FindCurrentPage(popupPage);
            }
            var navigationStack = parent.NavigationProxy?.NavigationStack;
            if (navigationStack != null && navigationStack.Count > 0)
            {
                for (int i = navigationStack.Count - 1; i >= 0; i--)
                    if (navigationStack[i] is Page popupPage)
                        return popupPage == parent ? popupPage : FindCurrentPage(popupPage);
            }
            if (parent.LogicalChildren.Count < 1)
                return parent;
            if (parent.LogicalChildren[0] is Page page)
                return FindCurrentPage(page);
            return parent;
        }


    }
}