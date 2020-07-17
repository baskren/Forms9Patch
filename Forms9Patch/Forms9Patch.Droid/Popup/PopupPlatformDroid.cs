using System;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Widget;
using Xamarin.Forms;
using XApplication = Xamarin.Forms.Application;
using Forms9Patch.Droid;
using Forms9Patch.Elements.Popups.Core;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(PopupPlatformDroid))]
namespace Forms9Patch.Droid
{
    [Preserve(AllMembers = true)]
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    internal class PopupPlatformDroid : IPopupPlatform
    {
        private IPopupNavigation PopupNavigationInstance => PopupNavigation.Instance;

        private FrameLayout DecoreView => (FrameLayout)((Activity)Settings.Context).Window.DecorView;

        public event EventHandler OnInitialized
        {
            add => Settings.OnInitialized += value;
            remove => Settings.OnInitialized -= value;
        }

        public bool IsInitialized => Settings.IsInitialized;

        public bool IsSystemAnimationEnabled => GetIsSystemAnimationEnabled();

        public Task AddAsync(PopupPage page)
        {
            var decoreView = DecoreView;

            page.Parent = XApplication.Current?.MainPage;

            var renderer = page.GetOrCreateRenderer();

            decoreView.AddView(renderer.View);

            return PostAsync(renderer.View);
        }

        public Task RemoveAsync(PopupPage page)
        {
            if (page.GetOrCreateRenderer() is IVisualElementRenderer renderer)
            {
                var element = renderer.Element;

                if (renderer.View is Android.Views.View view)
                    DecoreView?.RemoveView(view);
                renderer.Dispose();

                if (element != null)
                    element.Parent = null;

                return PostAsync(DecoreView);
            }

            return Task.CompletedTask;
        }

        #region System Animation

        private bool GetIsSystemAnimationEnabled()
        {
            float animationScale;
            var context = Settings.Context;

            if (context == null)
                return false;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBeanMr1)
            {
                animationScale = Android.Provider.Settings.Global.GetFloat(
                    context.ContentResolver,
                    Android.Provider.Settings.Global.AnimatorDurationScale,
                    1);
            }
            else
            {
                animationScale = Android.Provider.Settings.System.GetFloat(
                    context.ContentResolver,
#pragma warning disable CS0618 // Type or member is obsolete
                    Android.Provider.Settings.System.AnimatorDurationScale,
#pragma warning restore CS0618 // Type or member is obsolete
                    1);
            }

            return animationScale > 0;
        }

        #endregion

        #region Helpers

        Task PostAsync(Android.Views.View nativeView)
        {
            if (nativeView == null)
                return Task.FromResult(true);

            var tcs = new TaskCompletionSource<bool>();

            nativeView.Post(() =>
            {
                tcs.SetResult(true);
            });

            return tcs.Task;
        }

        #endregion
    }
}