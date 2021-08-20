using System;
using Xamarin.Forms;
using System.Linq;
using System.Reflection;
using P42.Utils;
using System.ComponentModel;

namespace Forms9Patch
{
    /// <summary>
    /// Focus monitor: Helps you keep up with what VisualElement currently has focus
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    public static class FocusMonitor
    {
#pragma warning disable IDE0044 // Add readonly modifier
        static PropertyInfo _currentlyFocusedPropertyInfo;
#pragma warning restore IDE0044 // Add readonly modifier
        static FocusMonitor()
        {

            var visualElementType = typeof(Xamarin.Forms.VisualElement);
            _currentlyFocusedPropertyInfo = visualElementType.GetPropertyInfo("CurrentlyFocused");
            if (_currentlyFocusedPropertyInfo != null)
            {
                var eventInfo = visualElementType.GetRuntimeEvent("FocusChanged");
                var focusMonitorType = typeof(FocusMonitor);
                var methodInfo = focusMonitorType.GetMethodInfo(nameof(OnVisualElementFocusChanged));
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

        static VisualElement _lastFromElement;
        static DateTime _lastFocusChangedDateTime = DateTime.MinValue.AddYears(1);
        internal static void OnVisualElementFocusChanged(object fromElement, VisualElement toElement)
        {
            //System.Diagnostics.Debug.WriteLine("Forms9Patch.FocusMonitor.OnVisualElementFocusChanged(" + fromElement + "," + toElement + ")");
            /*
            if (DateTime.Now - _lastFocusChangedDateTime < TimeSpan.FromMilliseconds(50) &&  _lastFromElement is Xamarin.Forms.ScrollView && fromElement==null && toElement is Xamarin.Forms.InputView && HardwareKeyPage.RemoveNativeFocus!=null)
            {
                System.Diagnostics.Debug.WriteLine("\t (_lastFromElement is Xamarin.Forms.ScrollView && fromElement==null && toElement is Xamarin.Forms.InputView && HardwareKeyPage.RemoveNativeFocus!=null)");
                // special UWP situation: Xamarin.Forms thinks UWP is giving control back to an InputEntry element BUT that's not what's really happened!
                _lastFromElement = fromElement as VisualElement;
                _lastFocusChangedDateTime = DateTime.Now;
                return;
            }
            */

            _lastFromElement = fromElement as VisualElement;
            _lastFocusChangedDateTime = DateTime.Now;
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
                //System.Diagnostics.Debug.WriteLine("Forms9Patch.FocusMonitor.FocusedElement = [" + value + "]   was: [" + FocusedElement + "]");
                if (FocusedElement == value)
                    return;

                /*
                if (_currentlyFocusedPropertyInfo != null)
                    _currentlyFocusedPropertyInfo.SetValue(null, value);
                else
                {
                */
                //System.Diagnostics.Debug.WriteLine("\t A");
                var wasElement = _element;

                var changed = false;
                //System.Diagnostics.Debug.WriteLine("\t B _element=[" + _element + "]");
                if (_element != null)
                {
                    //System.Diagnostics.Debug.WriteLine("\t C");
                    _element = null;
                    changed = true;
                }
                //System.Diagnostics.Debug.WriteLine("\t D value=[" + value + "]");
                if (value != null)
                {
                    //System.Diagnostics.Debug.WriteLine("\t E value!=null");
                    var focused = value.Focus();
                    if (focused)
                    {
                        //System.Diagnostics.Debug.WriteLine("\t F.B value.Focus()==true");
                        _element = value;
                        changed = true;
                    }
                    //else
                    //System.Diagnostics.Debug.WriteLine("\t F.A value.Focus()==false");
                }
                else if (wasElement != null)
                {
                    //System.Diagnostics.Debug.WriteLine("\t G wasElement!=null   where wasElement=[" + wasElement + "]");
                    wasElement.Unfocus();
                    //if (wasElement.IsFocused)
                    //    System.Diagnostics.Debug.WriteLine("\t  G.1  couldn't unfocus wasElement");
                }
                else
                {
                    //System.Diagnostics.Debug.WriteLine("\t H value==null && wasElement==null");
                    FocusedElement?.Unfocus();
                    if (FocusedElement != null)
                    {
                        //System.Diagnostics.Debug.WriteLine("\t I couldn't Unfocus FocusedElement, attempted HardwareKeyPage.RemoveNativeFocus");
                        HardwareKeyPage.RemoveNativeFocus?.Invoke();
                    }
                }

                //System.Diagnostics.Debug.WriteLine("\t J");
                if (changed && _currentlyFocusedPropertyInfo == null)
                {
                    //System.Diagnostics.Debug.WriteLine("\t K");
                    FocusedElementChanged?.Invoke(wasElement, _element);
                }
                //}
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
            //System.Diagnostics.Debug.WriteLine("FocusMonitor.Start(" + element + ") ENTER");

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
                element.Focused += OnVisualElementFocused;
                element.Unfocused += OnVisualElementUnfocused;
                element.ChildAdded += OnVisualElementChildAdded;
                element.ChildRemoved += OnVisualElementChildRemoved;
                foreach (var child in element.LogicalChildren)
                    if (child is VisualElement childElement)
                        Start(childElement);
            }
            //System.Diagnostics.Debug.WriteLine("FocusMonitor.Start(" + element + ") EXIT");
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

            //System.Diagnostics.Debug.WriteLine("DetachFrom[" + element + "]");
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
            //if (e.VisualElement is Xamarin.Forms.ScrollView)
            //    //FocusedElement = null;
            //    return;
            //else
            FocusedElement = e.VisualElement;
            //if (e.VisualElement is Xamarin.Forms.ScrollView && e.VisualElement?.Parent is VisualElement parent)
            //    parent.Focus();

        }


        static void OnVisualElementChildRemoved(object sender, ElementEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("OnVisualElementChildRemoved.OnVisualElementChildRemoved(" + sender + "," + e.Element + ") ENTER");

            //if (sender is VisualElement visualElement)
            if (e.Element is VisualElement visualElement)
                Stop(visualElement);
            //System.Diagnostics.Debug.WriteLine("OnVisualElementChildRemoved.OnVisualElementChildRemoved(" + sender + "," + e.Element + ") EXIT");
        }

        static void OnVisualElementChildAdded(object sender, ElementEventArgs e)
        {
            //if (sender is VisualElement visualElement)
            if (e.Element is VisualElement visualElement)
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