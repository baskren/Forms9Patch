using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using Forms9Patch.iOS;
using Forms9Patch.Elements.Popups.Core;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Size = Xamarin.Forms.Size;

[assembly: ExportRenderer(typeof(PopupPage), typeof(PopupPageRenderer))]
namespace Forms9Patch.iOS
{
    [Preserve(AllMembers = true)]
    public class PopupPageRenderer : PageRenderer
    {
        private readonly UIGestureRecognizer _tapGestureRecognizer;
        private NSObject _willChangeFrameNotificationObserver;
        private NSObject _willHideNotificationObserver;
        private CGRect _keyboardBounds;

        private PopupPage CurrentElement => (PopupPage)Element;

        #region Main Methods

        public PopupPageRenderer()
        {
            _tapGestureRecognizer = new UITapGestureRecognizer(OnTap)
            {
                CancelsTouchesInView = false
            };
        }

        bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                View?.RemoveGestureRecognizer(_tapGestureRecognizer);
                _tapGestureRecognizer?.Dispose();

                if (_willChangeFrameNotificationObserver != null)
                    NSNotificationCenter.DefaultCenter.RemoveObserver(_willChangeFrameNotificationObserver);

                if (_willHideNotificationObserver != null)
                    NSNotificationCenter.DefaultCenter.RemoveObserver(_willHideNotificationObserver);

                _willChangeFrameNotificationObserver?.Dispose();
                _willHideNotificationObserver?.Dispose();

                _willChangeFrameNotificationObserver = null;
                _willHideNotificationObserver = null;

            }
            base.Dispose(disposing);
        }

        #endregion

        #region Gestures Methods

        private void OnTap(UITapGestureRecognizer e)
        {
            var view = e.View;
            var location = e.LocationInView(view);
            var subview = view.HitTest(location, null);
            if (subview == view)
                CurrentElement.SendBackgroundClick();
        }

        #endregion

        #region Life Cycle Methods

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
            ModalTransitionStyle = UIModalTransitionStyle.CoverVertical;

            View?.AddGestureRecognizer(_tapGestureRecognizer);
        }

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();

            View?.RemoveGestureRecognizer(_tapGestureRecognizer);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            UnregisterAllObservers();

            _willChangeFrameNotificationObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, KeyBoardUpNotification);
            _willHideNotificationObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, KeyBoardDownNotification);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            UnregisterAllObservers();
        }

        #endregion

        #region Layout Methods

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            var currentElement = CurrentElement;

            if (View?.Superview?.Frame == null || currentElement == null)
                return;

            var superviewFrame = View.Superview.Frame;
            var applactionFrame = UIScreen.MainScreen.ApplicationFrame;

            Thickness systemPadding;

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                var safeAreaInsets = UIApplication.SharedApplication.KeyWindow.SafeAreaInsets;

                systemPadding = new Thickness(
                    safeAreaInsets.Left,
                    safeAreaInsets.Top,
                    safeAreaInsets.Right,
                    safeAreaInsets.Bottom);
            }
            else
            {
                systemPadding = new Thickness
                {
                    Left = applactionFrame.Left,
                    Top = applactionFrame.Top,
                    Right = applactionFrame.Right - applactionFrame.Width - applactionFrame.Left,
                    Bottom = applactionFrame.Bottom - applactionFrame.Height - applactionFrame.Top
                };
            }

            currentElement.SetValue(PopupPage.SystemPaddingProperty, systemPadding);
            currentElement.SetValue(PopupPage.KeyboardOffsetProperty, _keyboardBounds.Height);

            if (Element != null)
                SetElementSize(new Size(superviewFrame.Width, superviewFrame.Height));

            currentElement.ForceLayout();
        }

        #endregion

        #region Notifications Methods

        private void UnregisterAllObservers()
        {
            if (_willChangeFrameNotificationObserver != null)
                NSNotificationCenter.DefaultCenter.RemoveObserver(_willChangeFrameNotificationObserver);

            if (_willHideNotificationObserver != null)
                NSNotificationCenter.DefaultCenter.RemoveObserver(_willHideNotificationObserver);

            _willChangeFrameNotificationObserver = null;
            _willHideNotificationObserver = null;
        }

        private void KeyBoardUpNotification(NSNotification notifi)
        {
            _keyboardBounds = UIKeyboard.BoundsFromNotification(notifi);

            ViewDidLayoutSubviews();
        }

#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
        private async void KeyBoardDownNotification(NSNotification notifi)
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
        {
            var canAnimated = notifi.UserInfo.TryGetValue(UIKeyboard.AnimationDurationUserInfoKey, out NSObject duration);

            _keyboardBounds = CGRect.Empty;

            if (canAnimated)
            {
                //It is needed that buttons are working when keyboard is opened. See #11
                await Task.Delay(70);

                if (!_disposed)
                    await UIView.AnimateAsync((double)(NSNumber)duration, OnKeyboardAnimated);
            }
            else
            {
                ViewDidLayoutSubviews();
            }
        }

        #endregion

        #region Animation Methods

        private void OnKeyboardAnimated()
        {
            if (_disposed)
                return;

            ViewDidLayoutSubviews();
        }

        #endregion


    }
}
