using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Views.InputMethods;
using Xamarin.Forms;
using Java.Util;
using Android.Views;
using Xamarin.Forms.PlatformConfiguration;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.Droid.KeyboardService))]
namespace Forms9Patch.Droid
{
    public class KeyboardService : IKeyboardService
    {
        public bool IsHardwareKeyboardActive
        {
            get
            {
                var activity = Settings.Context as Activity;
                return activity.Resources.Configuration.HardKeyboardHidden == Android.Content.Res.HardKeyboardHidden.No;
            }
        }

        public void Hide()
        {
            if (Android.App.Application.Context.GetSystemService(Context.InputMethodService) is InputMethodManager im
                && Settings.Context is Activity activity)
            {
                var token = activity.CurrentFocus?.WindowToken;
                im.HideSoftInputFromWindow(token, HideSoftInputFlags.NotAlways);
            }
        }

        public KeyboardService()
        {
            Android.Views.View root = Forms9Patch.Droid.Settings.Activity.FindViewById(Android.Resource.Id.Content);


            var rootLayoutListener = new RootLayoutListener(root);
            rootLayoutListener.HeightChanged += OnHeightChanged;
            root.ViewTreeObserver.AddOnGlobalLayoutListener(rootLayoutListener);
        }


        double _lastHeight;
        private void OnHeightChanged(object sender, double e)
        {
            Height = e;
            if (Height > 0 && _lastHeight <= 0)
                Forms9Patch.KeyboardService.OnVisiblityChange(KeyboardVisibilityChange.Shown);
            else if (_lastHeight > 0 && Height <= 0)
                Forms9Patch.KeyboardService.OnVisiblityChange(KeyboardVisibilityChange.Hidden);
            _lastHeight = Height;
        }

        public string LanguageRegion
        {
            get
            {
                var imm = (InputMethodManager)Forms9Patch.Droid.Settings.Context.GetSystemService(Context.InputMethodService);
                var ims = imm.CurrentInputMethodSubtype;
                string result;
                //if (Android.OS.Build.VERSION.SDK_INT > 24)
                if (Android.OS.Build.VERSION.SdkInt > Android.OS.BuildVersionCodes.M)
                    result = ims?.LanguageTag.Replace('_', '-');
                else
#pragma warning disable CS0618 // Type or member is obsolete
                    result = ims?.Locale.Replace('_', '-');
#pragma warning restore CS0618 // Type or member is obsolete

                if (string.IsNullOrWhiteSpace(result))
                {
                    var language = Locale.Default.Language;
                    var country = Locale.Default.Country;

                    if (string.IsNullOrWhiteSpace(language))
                        return country;
                    if (string.IsNullOrWhiteSpace(country))
                        return language;
                    return language + "-" + country;
                }
                return result;
            }
        }


        double _height;
        public double Height
        {
            get => _height;
            set
            {
                if (System.Math.Abs(_height - value) > 0.1)
                {
                    _height = value;
                    Forms9Patch.KeyboardService.OnHeightChanged(_height);
                }
                _height = value;
            }
        }
    }

    class RootLayoutListener : Java.Lang.Object, ViewTreeObserver.IOnGlobalLayoutListener
    {
        //int[] _discrepancy = { 0 };

        readonly Android.Graphics.Rect _startRect;
        readonly Android.Views.View _rootView;

        public event EventHandler<double> HeightChanged;


        public RootLayoutListener(Android.Views.View view)
        {
            _rootView = view;
            _startRect = new Android.Graphics.Rect();
            _rootView.GetWindowVisibleDisplayFrame(_startRect);
            //var expectedHeight = Forms9Patch.Display.Height;
            //var expectedWidth = Forms9Patch.Display.Width;
            //System.Diagnostics.Debug.WriteLine("_startRect=[" + _startRect.Width() + "," + _startRect.Height() + "]");
            //System.Diagnostics.Debug.WriteLine(" expected=[" + expectedWidth + "," + expectedHeight + "]");
        }

        public void OnGlobalLayout()
        {
            Android.Graphics.Rect currentRect = new Android.Graphics.Rect();
            _rootView.GetWindowVisibleDisplayFrame(currentRect);

            var height = _startRect.Height() - currentRect.Height();

            HeightChanged?.Invoke(this, height / Display.Scale);
        }

        bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                _startRect?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
