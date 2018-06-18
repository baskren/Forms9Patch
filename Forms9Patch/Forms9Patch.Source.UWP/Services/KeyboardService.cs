using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Input;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.UWP.KeyboardService))]
namespace Forms9Patch.UWP
{
    class KeyboardService : IKeyboardService, IDisposable
    {
        public bool IsHardwareKeyboardActive
        {
            get
            {
                KeyboardCapabilities keyboardCapabilities = new Windows.Devices.Input.KeyboardCapabilities();
                return keyboardCapabilities.KeyboardPresent != 0;
            }
        }

        Windows.Graphics.Display.DisplayInformation _displayInformation;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.iOS.KeyboardService"/> class.
        /// </summary>
        public KeyboardService()
        {
            InputPane.GetForCurrentView().Hiding += KeyboardService_Hiding;
            InputPane.GetForCurrentView().Showing += KeyboardService_Showing;
        }

        private void OnOrienationChanged(DisplayInformation sender, object args)
        {
            Height = InputPane.GetForCurrentView().OccludedRect.Height;
        }

        private void KeyboardService_Showing(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            Forms9Patch.KeyboardService.OnVisiblityChange(KeyboardVisibilityChange.Shown);
            Height = InputPane.GetForCurrentView().OccludedRect.Height;
            _displayInformation = Windows.Graphics.Display.DisplayInformation.GetForCurrentView();
            _displayInformation.OrientationChanged += OnOrienationChanged;
        }

        private void KeyboardService_Hiding(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            Forms9Patch.KeyboardService.OnVisiblityChange(KeyboardVisibilityChange.Hidden);
            Height = InputPane.GetForCurrentView().OccludedRect.Height;
            if (_displayInformation!=null)
                _displayInformation.OrientationChanged -= OnOrienationChanged;
        }

        /// <summary>
        /// Hide this instance.
        /// </summary>
        public void Hide()
        {
            InputPane.GetForCurrentView().TryHide();
        }

        public string LanguageRegion
        {
            get
            {
                return Windows.Globalization.Language.CurrentInputMethodLanguageTag;
            }
        }

        double _height;
        public double Height
        {
            get => _height;
            set
            {
                if (_height!=value)
                {
                    _height = value;
                    Forms9Patch.KeyboardService.OnHeightChanged(_height);
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_displayInformation!=null)
                    {
                        _displayInformation.OrientationChanged -= OnOrienationChanged;
                        _displayInformation = null;
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~KeyboardService() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
