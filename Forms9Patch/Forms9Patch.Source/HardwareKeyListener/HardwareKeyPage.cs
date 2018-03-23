using System;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Required to enable Hardware Key Listening capability
    /// </summary>
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
            FocusMonitor.FocusedElementChanged += OnFocusElementChanged;
        }

        static void OnFocusElementChanged(object sender, EventArgs e)
        {
            FocusedElement = sender as VisualElement;
        }

        internal static VisualElement _element;
        /// <summary>
        /// Gets or sets the currently focused element - the element who'sHardwareKeyListeners are currently active
        /// </summary>
        /// <value>The focused element.</value>
        public static VisualElement FocusedElement
        {
            get { return _element; }
            set
            {
                if (_element != value)
                {
                    System.Diagnostics.Debug.WriteLine("==============================================");
                    System.Diagnostics.Debug.WriteLine("HardwareKeyPage.FocusElement Was=[" + _element + "]");
                    _element = value;
                    FocusMonitor.FocusedElement = _element;
                    FocusedElementChanged?.Invoke(_element, EventArgs.Empty);
                    System.Diagnostics.Debug.WriteLine("HardwareKeyPage.FocusElement  Is=[" + _element + "]");
                    System.Diagnostics.Debug.WriteLine("==============================================");
                    System.Diagnostics.Debug.WriteLine("");
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
                    if (value is View || value is Forms9Patch.HardwareKeyPage)
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

        /// <summary>
        /// Called when the HardwareKeyPage appears
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
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
            base.OnDisappearing();
            _VisibleInstances--;
            FocusMonitor.Stop(this);
        }
    }
}