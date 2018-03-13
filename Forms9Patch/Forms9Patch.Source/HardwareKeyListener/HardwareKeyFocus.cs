using System;
using Xamarin.Forms;

namespace Forms9Patch
{
    public static class HardwareKeyFocus
    {
        static VisualElement _element;
        public static VisualElement Element
        {
            get { return _element; }
            set
            {
                value?.Focus();
                if (_element != value)
                {
                    System.Diagnostics.Debug.WriteLine("==============================================");
                    System.Diagnostics.Debug.WriteLine("HardwareKeyFocusElement Was=[" + _element + "]");
                    _element = value;
                    ElementChanged?.Invoke(value, EventArgs.Empty);
                    System.Diagnostics.Debug.WriteLine("HardwareKeyFocusElement  Is=[" + _element + "]");
                    System.Diagnostics.Debug.WriteLine("==============================================");
                    System.Diagnostics.Debug.WriteLine("");
                }
            }
        }

        public static VisualElement DefaultElement { get; set; }

        public static event EventHandler ElementChanged;


        #region Focus Monitoring
        internal static void AttachTo(VisualElement element)
        {
            if (element == null)
                return;
            if (element is NavigationPage navPage)
            {
                // we don't attach to NavigationPages
                if (navPage.InternalChildren.Count > 0)
                {
                    var lastPage = navPage.InternalChildren[navPage.InternalChildren.Count - 1] as Page;
                    AttachTo(lastPage);
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("AttachTo[" + element + "]");
                element.Focused += OnVisualElementFocused;
                element.Unfocused += OnVisualElementUnfocused;
                element.ChildAdded += OnVisualElementChildAdded;
                element.ChildRemoved += OnVisualElementChildRemoved;
                foreach (var child in element.LogicalChildren)
                    if (child is VisualElement childElement)
                        AttachTo(childElement);
            }
        }

        internal static void DetachFrom(VisualElement element)
        {
            if (element == null)
                return;
            if (element == Element)
                Element = null;
            if (element == DefaultElement)
                DefaultElement = null;

            System.Diagnostics.Debug.WriteLine("DetachFrom[" + element + "]");
            element.Focused -= OnVisualElementFocused;
            element.Unfocused -= OnVisualElementUnfocused;
            element.ChildAdded -= OnVisualElementChildAdded;
            element.ChildRemoved -= OnVisualElementChildRemoved;
            foreach (var child in element.LogicalChildren)
                if (child is VisualElement childElement)
                    DetachFrom(childElement);
        }

        static void OnVisualElementUnfocused(object sender, FocusEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("OnVisualElementUnfocused[" + e.VisualElement + "]");
            if (Element == e.VisualElement)
                Element = null;
        }

        static void OnVisualElementFocused(object sender, FocusEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("OnVisualElementFocused[" + e.VisualElement + "]");
            Element = e.VisualElement;
        }


        static void OnVisualElementChildRemoved(object sender, ElementEventArgs e)
        {
            if (sender is VisualElement visualElement)
                DetachFrom(visualElement);
        }

        static void OnVisualElementChildAdded(object sender, ElementEventArgs e)
        {
            if (sender is VisualElement visualElement)
                AttachTo(visualElement);
        }
        #endregion



    }
}