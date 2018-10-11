using System;
using UIKit;
using Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

//[assembly: ExportRenderer(typeof(Forms9Patch.PopupBase), typeof(Forms9Patch.iOS.PopupBaseRenderer))]
namespace Forms9Patch.iOS
{
    public class PopupBaseRenderer : Rg.Plugins.Popup.IOS.Renderers.PopupPageRenderer
    {
        public override bool CanBecomeFirstResponder => true;

        [Export("OnKeyPress:")]
        void OnKeyPress(UIKeyCommand cmd) => HardwareKeyPageRenderer.ProcessKeyPress(cmd);

        public override UIKeyCommand[] KeyCommands => HardwareKeyPageRenderer.GetKeyCommands();

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            System.Diagnostics.Debug.WriteLine("Connecting to Element: " + Element);
        }

    }
}