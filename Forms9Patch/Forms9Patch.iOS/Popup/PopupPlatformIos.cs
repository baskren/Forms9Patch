using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XFPlatform = Xamarin.Forms.Platform.iOS.Platform;
using Forms9Patch.Elements.Popups.Core;

[assembly: Dependency(typeof(Forms9Patch.iOS.PopupPlatformIos))]
namespace Forms9Patch.iOS
{
    [Preserve(AllMembers = true)]
    internal class PopupPlatformIos : IPopupPlatform
    {
        readonly List<UIWindow> _windows = new List<UIWindow>();

        static bool IsiOS9OrNewer => UIDevice.CurrentDevice.CheckSystemVersion(9, 0);
        static bool IsiOS13OrNewer => UIDevice.CurrentDevice.CheckSystemVersion(13, 0);

        public event EventHandler OnInitialized
        {
            add => Settings.OnInitialized += value;
            remove => Settings.OnInitialized -= value;
        }

        public bool IsInitialized => Settings.IsInitialized;

        public bool IsSystemAnimationEnabled => true;

        public async Task AddAsync(PopupPage page)
        {
            page.Parent = Application.Current.MainPage;

            page.DescendantRemoved += HandleChildRemoved;

            if (UIApplication.SharedApplication.KeyWindow.WindowLevel == UIWindowLevel.Normal)
                UIApplication.SharedApplication.KeyWindow.WindowLevel = -1;

            var renderer = page.GetOrCreateRenderer();

            var window = new PopupWindow();

            if (IsiOS13OrNewer)
                _windows.Add(window);

            window.BackgroundColor = Color.Transparent.ToUIColor();
            window.RootViewController = new PopupPlatformRenderer(renderer);
            window.RootViewController.View.BackgroundColor = Color.Transparent.ToUIColor();
            window.WindowLevel = UIWindowLevel.Normal;
            window.MakeKeyAndVisible();

            if (!IsiOS9OrNewer)
            {
                window.Frame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
            }

            await window.RootViewController.PresentViewControllerAsync(renderer.ViewController, false);
        }

        public async Task RemoveAsync(PopupPage page)
        {
            //await Task.Delay(50);

            page.DescendantRemoved -= HandleChildRemoved;

            if (XFPlatform.GetRenderer(page) is IVisualElementRenderer renderer && renderer.ViewController is UIViewController viewController)
            {
                if (!viewController.IsBeingDismissed && viewController.View?.Window is UIWindow window)
                {
                    await window.RootViewController.DismissViewControllerAsync(false);
                    DisposeModelAndChildrenRenderers(page);
                    window.RootViewController.Dispose();
                    window.RootViewController = null;
                    page.Parent = null;
                    window.Hidden = true;

                    if (IsiOS13OrNewer && _windows.Contains(window))
                        _windows.Remove(window);

                    window.Dispose();
                    window = null;

                    if (UIApplication.SharedApplication.KeyWindow.WindowLevel == -1)
                        UIApplication.SharedApplication.KeyWindow.WindowLevel = UIWindowLevel.Normal;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("!!!! CRASH AVERTED !!!!");
                    DisposeModelAndChildrenRenderers(page);
                    page.Parent = null;
                }
            }
            page.Parent = null;
        }

        private void DisposeModelAndChildrenRenderers(VisualElement view)
        {
            IVisualElementRenderer renderer;
            foreach (VisualElement child in view.Descendants())
            {
                renderer = XFPlatform.GetRenderer(child);
                XFPlatform.SetRenderer(child, null);

                if (renderer != null)
                {
                    renderer.NativeView.RemoveFromSuperview();
                    renderer.Dispose();
                }
            }

            renderer = XFPlatform.GetRenderer(view);
            if (renderer != null)
            {
                renderer.NativeView.RemoveFromSuperview();
                renderer.Dispose();
            }
            XFPlatform.SetRenderer(view, null);
        }

        private void HandleChildRemoved(object sender, ElementEventArgs e)
        {
            var view = e.Element;
            DisposeModelAndChildrenRenderers((VisualElement)view);
        }
    }
}
