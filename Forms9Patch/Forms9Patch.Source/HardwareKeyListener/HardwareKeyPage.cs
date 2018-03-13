using System;
using Xamarin.Forms;

namespace Forms9Patch
{
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
        public static VisualElement DefaultFocusedElement
        {
            get => _defaultElement;
            set
            {
                if (value != _defaultElement)
                {
                    if (value is View || value is HardwareKeyPage)
                        _defaultElement = value;
                    else
                        throw new Exception("DefaultElement can only be a Xamarin.Forms.View or Forms9Patch.HardwareKeyPage");
                }
            }
        }

        public static event EventHandler FocusedElementChanged;
        #endregion

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _VisibleInstances++;
            FocusMonitor.Start(this);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _VisibleInstances--;
            FocusMonitor.Stop(this);
        }
    }
}