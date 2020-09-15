using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Size = Windows.Foundation.Size;
using Xamarin.Forms.Platform.UWP;
using WinPopup = global::Windows.UI.Xaml.Controls.Primitives.Popup;
using Forms9Patch.Elements.Popups.Core;

[assembly: ExportRenderer(typeof(PopupPage), typeof(Forms9Patch.UWP.PopupPageRenderer))]
namespace Forms9Patch.UWP
{
    [Preserve(AllMembers = true)]
    public class PopupPageRenderer : PageRenderer
    {
        private Windows.Foundation.Rect _keyboardBounds;

        internal WinPopup Container { get; private set; }

        private PopupPage CurrentElement => (PopupPage)Element;

        [Preserve]
        public PopupPageRenderer()  {  }

        private void OnKeyboardHiding(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            _keyboardBounds = Windows.Foundation.Rect.Empty;
            UpdateElementSize();
        }

        private void OnKeyboardShowing(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            _keyboardBounds = sender.OccludedRect;
            UpdateElementSize();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            UpdateElementSize();

            return base.ArrangeOverride(finalSize);
        }

        internal void Prepare(WinPopup container)
        {
            Container = container;

            if (Window.Current is Window window)
                window.SizeChanged += OnSizeChanged;

            if (DisplayInformation.GetForCurrentView() is DisplayInformation displayInformation)
                displayInformation.OrientationChanged += OnOrientationChanged;

            if (InputPane.GetForCurrentView() is InputPane inputPane)
            {
                inputPane.Showing += OnKeyboardShowing;
                inputPane.Hiding += OnKeyboardHiding;
            }

            if (ContainerElement != null)
                ContainerElement.PointerPressed += OnBackgroundClick;
        }

        internal void Destroy()
        {
            Container = null;

            if (Window.Current is Window window)
                window.SizeChanged -= OnSizeChanged;

            if (DisplayInformation.GetForCurrentView() is DisplayInformation displayInformation)
                displayInformation.OrientationChanged -= OnOrientationChanged;

            if (InputPane.GetForCurrentView() is InputPane inputPane)
            {
                inputPane.Showing -= OnKeyboardShowing;
                inputPane.Hiding -= OnKeyboardHiding;
            }

            if (ContainerElement!=null)
                ContainerElement.PointerPressed -= OnBackgroundClick;
        }

        private void OnOrientationChanged(DisplayInformation sender, object args)
            => UpdateElementSize();
        

        private void OnSizeChanged(object sender, WindowSizeChangedEventArgs e)
            => UpdateElementSize();
        

        private void OnBackgroundClick(object sender, PointerRoutedEventArgs e)
        {
            if (e.OriginalSource == this)
                CurrentElement?.SendBackgroundClick();
        }

        private async void UpdateElementSize()
        {
            if (Window.Current?.Bounds is Windows.Foundation.Rect windowBound
                && ApplicationView.GetForCurrentView()?.VisibleBounds is Windows.Foundation.Rect visibleBounds)
            {
                await Task.Delay(50);

                var keyboardHeight = _keyboardBounds != Windows.Foundation.Rect.Empty ? _keyboardBounds.Height : 0;

                var top = visibleBounds.Top - windowBound.Top;
                var bottom = windowBound.Bottom - visibleBounds.Bottom;
                var left = visibleBounds.Left - windowBound.Left;
                var right = windowBound.Right - visibleBounds.Right;

                top = Math.Max(0, top);
                bottom = Math.Max(0, bottom);
                left = Math.Max(0, left);
                right = Math.Max(0, right);

                var systemPadding = new Xamarin.Forms.Thickness(left, top, right, bottom);

                CurrentElement?.SetValue(PopupPage.SystemPaddingProperty, systemPadding);
                CurrentElement?.SetValue(PopupPage.KeyboardOffsetProperty, keyboardHeight);
                CurrentElement?.Layout(new Rectangle(windowBound.X, windowBound.Y, windowBound.Width, windowBound.Height));
                CurrentElement?.ForceLayout();
            }
        }
    }
}
