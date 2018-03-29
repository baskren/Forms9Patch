using System;
using Xamarin.Forms;
using System.Linq;
using System.Reflection;

namespace Forms9Patch
{
    /// <summary>
    /// Focus monitor: Helps you keep up with what VisualElement currently has focus
    /// </summary>
    public static class FocusMonitor
    {
        static PropertyInfo _currentlyFocusedPropertyInfo;
        static FocusMonitor()
        {

            var type = typeof(Xamarin.Forms.VisualElement);
            //_currentlyFocusedPropertyInfo = type.GetProperty("CurrentlyFocused", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            _currentlyFocusedPropertyInfo = type.GetRuntimeProperty("CurrentlyFocused");
            if (_currentlyFocusedPropertyInfo != null)
            {
                //var eventInfo = type.GetEvent("FocusChanged");
                var eventInfo = type.GetRuntimeEvent("FocusChanged");
                //var methodInfo = typeof(FocusMonitor).GetMethod("OnVisualElementFocusChanged", BindingFlags.Static | BindingFlags.NonPublic);
                var methodInfo = typeof(FocusMonitor).GetRuntimeMethod("OnVisualElementFocusChanged", null);   // ("OnVisualElementFocusChanged", BindingFlags.Static | BindingFlags.NonPublic);
                if (methodInfo != null)
                {
                    //Delegate handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, null, methodInfo);
                    var handler = methodInfo.CreateDelegate(typeof(EventHandler<VisualElement>));
                    eventInfo.AddEventHandler(null, handler);
                }
                else
                    _currentlyFocusedPropertyInfo = null;
            }
        }

        internal static void OnVisualElementFocusChanged(object fromElement, VisualElement toElement)
        {
            FocusedElementChanged?.Invoke(fromElement, toElement);
        }

        #region Properties
        static bool _enabled;
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.FocusMonitor"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public static bool Enabled
        {
            get
            {
                if (_currentlyFocusedPropertyInfo != null)
                    return true;
                return _enabled;
            }
            set
            {
                if (_currentlyFocusedPropertyInfo == null)
                    _enabled = value;
            }
        }

        static VisualElement _element;
        /// <summary>
        /// Gets or sets the focused element.
        /// </summary>
        /// <value>The focused element.</value>
        public static VisualElement FocusedElement
        {
            get
            {
                if (_currentlyFocusedPropertyInfo != null)
                {
                    var result = (VisualElement)_currentlyFocusedPropertyInfo.GetValue(null);
                    return result;
                }
                return _element;
            }
            set
            {
                if (FocusedElement == value)
                    return;


                if (_currentlyFocusedPropertyInfo != null)
                    _currentlyFocusedPropertyInfo.SetValue(null, value);
                else
                {
                    var wasElement = _element;

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
                        FocusedElementChanged?.Invoke(wasElement, _element);
                }
            }
        }
        #endregion

        /// <summary>
        /// Occurs when focused element changed.
        /// </summary>
        public static event EventHandler<VisualElement> FocusedElementChanged;



        #region Focus Monitoring
        /// <summary>
        /// Starts monitoring a VisualElement and all of its decendants
        /// </summary>
        /// <returns>The start.</returns>
        /// <param name="element">Element.</param>
        public static void Start(VisualElement element)
        {
            if (!_enabled || element == null || element.IsFocusMonitoring())
                return;

            element.IsFocusMonitoring(true);

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

        /// <summary>
        /// Stops monitoring a VisualElement and all of its decendants
        /// </summary>
        /// <returns>The stop.</returns>
        /// <param name="element">Element.</param>
        public static void Stop(VisualElement element)
        {
            if (element == null)
                return;

            element.IsFocusMonitoring(false);

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

    internal static class FocusMonitoringExtensions
    {
        public static readonly BindableProperty IsFocusMonitoringProperty = BindableProperty.Create("IsFocus", typeof(bool), typeof(FocusMonitoringExtensions), default(bool));

        public static bool IsFocusMonitoring(this VisualElement visualElement)
        {
            return (bool)visualElement.GetValue(IsFocusMonitoringProperty);
        }

        public static void IsFocusMonitoring(this VisualElement visualElement, bool value)
        {
            visualElement.SetValue(IsFocusMonitoringProperty, value);
        }
    }
}