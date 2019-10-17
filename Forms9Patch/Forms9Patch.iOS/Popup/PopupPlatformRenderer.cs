using UIKit;
using Xamarin.Forms.Platform.iOS;
using Foundation;
using System;

namespace Forms9Patch.iOS
{
    [Preserve(AllMembers = true)]
    [Register("Forms9PatchPopupPlatformRenderer")]
    internal class PopupPlatformRenderer : UIViewController
    {
        public IVisualElementRenderer Renderer { get; private set; }

        public PopupPlatformRenderer(IVisualElementRenderer renderer)
        {
            P42.Utils.Debug.AddToCensus(this);
            Renderer = renderer;
        }

        public PopupPlatformRenderer(IntPtr handle) : base(handle)
        {
            P42.Utils.Debug.AddToCensus(this);
        }

        bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                if (Renderer is IDisposable disposable)
                    disposable.Dispose();
                Renderer = null;
                P42.Utils.Debug.RemoveFromCensus(this);
            }
            base.Dispose(disposing);
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            if ((ChildViewControllers != null) && (ChildViewControllers.Length > 0))
                return ChildViewControllers[0].GetSupportedInterfaceOrientations();
            return base.GetSupportedInterfaceOrientations();
        }

        public override UIInterfaceOrientation PreferredInterfaceOrientationForPresentation()
        {
            if ((ChildViewControllers != null) && (ChildViewControllers.Length > 0))
                return ChildViewControllers[0].PreferredInterfaceOrientationForPresentation();
            return base.PreferredInterfaceOrientationForPresentation();
        }

        public override UIViewController ChildViewControllerForStatusBarHidden()
            => Renderer.ViewController;

        public override bool ShouldAutorotate()
        {
            if ((ChildViewControllers != null) && (ChildViewControllers.Length > 0))
                return ChildViewControllers[0].ShouldAutorotate();
            return base.ShouldAutorotate();
        }

        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
        {
            if ((ChildViewControllers != null) && (ChildViewControllers.Length > 0))
                return ChildViewControllers[0].ShouldAutorotateToInterfaceOrientation(toInterfaceOrientation);
            return base.ShouldAutorotateToInterfaceOrientation(toInterfaceOrientation);
        }

        public override bool ShouldAutomaticallyForwardRotationMethods => true;

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            PresentedViewController?.ViewDidLayoutSubviews();
        }
    }
}
