using System;
using Xamarin.Forms;

namespace Forms9Patch
{
    public static class FocusMonitor
    {
        #region Properties
        static bool _enabled;
        public static bool Enabled
        {
            get => _enabled;
            set => _enabled = value;
        }

        static VisualElement _element;
        public static VisualElement FocusedElement
        {
            get { return _element; }
            set
            {
                bool changed = false;
                if (_element != null)
                {
                    _element = null;
                    changed = true;
                }
                if (value != null)
                {
                    var focused = value.Focus();
                    if (focused)
                    {
                        _element = value;
                        changed = true;
                    }
                }
                if (changed)
                    FocusedElementChanged?.Invoke(_element, EventArgs.Empty);
            }
        }
        #endregion

        public static event EventHandler FocusedElementChanged;



        #region Focus Monitoring
        public static void Start(VisualElement element)
        {
            if (!_enabled || element == null)
                return;

            if (element.IsFocused)
                _element = element;

            if (element is NavigationPage navPage)
            {
                // we don't attach to NavigationPages
                if (navPage.InternalChildren.Count > 0)
                {
                    var lastPage = navPage.InternalChildren[navPage.InternalChildren.Count - 1] as Page;
                    Start(lastPage);
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
                        Start(childElement);
            }
        }

        public static void Stop(VisualElement element)
        {
            if (element == null)
                return;
            if (element == FocusedElement)
                _element = null;
            if (element == HardwareKeyPage.FocusedElement)
                HardwareKeyPage._element = null;
            if (element == HardwareKeyPage.DefaultFocusedElement)
                HardwareKeyPage.DefaultFocusedElement = null;

            System.Diagnostics.Debug.WriteLine("DetachFrom[" + element + "]");
            element.Focused -= OnVisualElementFocused;
            element.Unfocused -= OnVisualElementUnfocused;
            element.ChildAdded -= OnVisualElementChildAdded;
            element.ChildRemoved -= OnVisualElementChildRemoved;
            foreach (var child in element.LogicalChildren)
                if (child is VisualElement childElement)
                    Stop(childElement);
        }

        static void OnVisualElementUnfocused(object sender, FocusEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("OnVisualElementUnfocused[" + e.VisualElement + "]");
            if (FocusedElement == e.VisualElement)
                FocusedElement = null;
        }

        static void OnVisualElementFocused(object sender, FocusEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("OnVisualElementFocused[" + e.VisualElement + "]");
            FocusedElement = e.VisualElement;
        }


        static void OnVisualElementChildRemoved(object sender, ElementEventArgs e)
        {
            if (sender is VisualElement visualElement)
                Stop(visualElement);
        }

        static void OnVisualElementChildAdded(object sender, ElementEventArgs e)
        {
            if (sender is VisualElement visualElement)
                Start(visualElement);
        }
        #endregion


    }
}