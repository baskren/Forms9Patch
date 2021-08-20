using System;
using System.Collections.Generic;
using System.Threading;
using Xamarin.Forms;
using System.ComponentModel;

namespace Forms9Patch
{
    /// <summary>
    /// Required to enable Hardware Key Listening capability
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    public class HardwareKeyPage : ContentPage
    {
        #region static implementation
        static int _instances;
        static int _VisibleInstances
        {
            get => _instances;
            set
            {
                if (value != _instances)
                {
                    _instances = value;
                    if (_instances <= 0)
                        FocusMonitor.Enabled = false;
                    else
                        FocusMonitor.Enabled = true;
                }
            }
        }

        static HardwareKeyPage()
        {
            Settings.ConfirmInitialization();
            FocusMonitor.FocusedElementChanged += OnFocusMonitorFocusedElementChanged;
        }

        //static Timer _falseFocusTimer=null;
        static DateTime _lastFocusChangedDateTime = DateTime.MinValue.AddYears(1);
        static void OnFocusMonitorFocusedElementChanged(object wasElement, VisualElement currentElement)
        {
            //System.Diagnostics.Debug.WriteLine("Forms9Patch.HardwareKeyPage.OnFocusElementChanged (" + wasElement + ", " + currentElement + ") ");

            if (currentElement == _expectedFocusedElement)
            {
                //System.Diagnostics.Debug.WriteLine("\t (currentElement == _expectedFocusedElement) [" + currentElement + "]");
                // the FocusedElement setter has successfully updated the focus (or the app just regained focus).
                FocusedElementChanged?.Invoke(_element, EventArgs.Empty);
            }
            else if (currentElement is Xamarin.Forms.InputView && DateTime.Now - _lastFocusChangedDateTime > TimeSpan.FromMilliseconds(100))
            {
                //System.Diagnostics.Debug.WriteLine("\t (currentElement is Xamarin.Forms.InputView) [" + currentElement + "]");
                // the user has changed the focus  (or the app just regained focus).
                _element = currentElement;
                _expectedFocusedElement = currentElement;
                FocusedElementChanged?.Invoke(_element, EventArgs.Empty);
            }
            //else
            //    System.Diagnostics.Debug.WriteLine("\t n\\a");
            _lastFocusChangedDateTime = DateTime.Now;
            //System.Diagnostics.Debug.WriteLine("\n");
        }

        #region UWP Workarounds
        /*
        static object _nativeFocus;
        static object _lastNativeElement;
        static internal Func<object> GetNativeFocused;
        static internal Action RemoveNativeFocus;
        static void OnFalseFocusTimeOut(object state)
        {
            FocusedElement = null;
        }
        */
        static internal Action RemoveNativeFocus;
        #endregion


        internal static VisualElement _element;
        static VisualElement _expectedFocusedElement;
        /// <summary>
        /// Gets or sets the currently focused element - the element who'sHardwareKeyListeners are currently active
        /// </summary>
        /// <value>The focused element.</value>
        public static VisualElement FocusedElement
        {
            get => _element;
            set
            {
                if (_element != value)
                {
                    //System.Diagnostics.Debug.WriteLine("Forms9Patch.HardwareKeyPage.FocusedElement = [" + value + "]  was: [" + _element + "]");

                    _element = value;

                    if (value is Xamarin.Forms.InputView)
                        _expectedFocusedElement = value;
                    else
                    {
                        _expectedFocusedElement = null;
                        HardwareKeyPage.RemoveNativeFocus?.Invoke();
                    }

                    _lastFocusChangedDateTime = DateTime.Now;
                    if (FocusMonitor.FocusedElement == _expectedFocusedElement)
                    {
                        // we are not going to trigger a change in FocusMonitor.FocusedElement (or Xamarin.Forms.VisualElement.CurrentlyFocused) but we are changing HardwareKeyPage.FocusedElement.  
                        // So we need to signal a HardwareKeyPage.FocusedElementChanged event.
                        FocusedElementChanged?.Invoke(_element, EventArgs.Empty);
                        //System.Diagnostics.Debug.WriteLine("\t  (FocusMonitor.FocusedElement == _expectedFocusedElement) [" + _expectedFocusedElement + "]");
                        //System.Diagnostics.Debug.WriteLine("\n");
                    }
                    else
                    {
                        FocusMonitor.FocusedElement = _expectedFocusedElement;
                    }
                }
            }
        }

        static VisualElement _defaultElement;
        /// <summary>
        /// Gets or sets the default focused element - the element who's HardwareKeyListeners are active if no element has focus 
        /// </summary>
        /// <value>The default focused element.</value>
        public static VisualElement DefaultFocusedElement
        {
            get => _defaultElement;
            set
            {
                if (value != _defaultElement)
                {
                    if (value is View || value is Forms9Patch.HardwareKeyPage || value is Forms9Patch.PopupBase)
                        _defaultElement = value;
                    else if (value != null)
                        throw new Exception("DefaultElement can only be a Xamarin.Forms.View or Forms9Patch.HardwareKeyPage. [" + value + "]");
                    else
                        _defaultElement = null;
                }
            }
        }

        /// <summary>
        /// Event called when the HardwareKay.FocusedElement has changed
        /// </summary>
        public static event EventHandler FocusedElementChanged;
        #endregion

        #region Fields
        VisualElement _elementFocusedBeforeDisappearing;
        #endregion

        /// <summary>
        /// Called when the HardwareKeyPage appears
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_elementFocusedBeforeDisappearing != null)
                DefaultFocusedElement = _elementFocusedBeforeDisappearing;
            _VisibleInstances++;
            FocusMonitor.Start(this);
        }

        // Called by RootPage to address failure of Android to call OnAppearing when popping a page
        internal void OnReappearing()
        {
            if (Device.RuntimePlatform == Device.Android)
                OnAppearing();
        }

        /// <summary>
        /// Called when the HardwareKeyPage disappers
        /// </summary>
        protected override void OnDisappearing()
        {
            _elementFocusedBeforeDisappearing = DefaultFocusedElement;
            base.OnDisappearing();
            _VisibleInstances--;
            FocusMonitor.Stop(this);
        }
    }
}