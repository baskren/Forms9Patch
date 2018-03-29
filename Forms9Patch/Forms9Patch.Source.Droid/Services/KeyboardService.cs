using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Views.InputMethods;
using Xamarin.Forms;
using Java.Util;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.Droid.KeyboardService))]
namespace Forms9Patch.Droid
{
    public class KeyboardService : IKeyboardService
    {
        InputMethodManager im;
        bool _lastAcceptingText;

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
            im = Android.App.Application.Context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            var activity = Settings.Context as Activity;
            if (im != null && activity != null)
            {
                var token = activity.CurrentFocus == null ? null : activity.CurrentFocus.WindowToken;
                im.HideSoftInputFromWindow(token, HideSoftInputFlags.NotAlways);
            }
        }

        public KeyboardService()
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(25), () =>
            {
                im = Android.App.Application.Context.GetSystemService(Context.InputMethodService) as InputMethodManager;
                if (im.IsAcceptingText != _lastAcceptingText)
                {
                    Forms9Patch.KeyboardService.OnVisiblityChange(im.IsAcceptingText ? KeyboardVisibilityChange.Shown : KeyboardVisibilityChange.Hidden);
                    _lastAcceptingText = im.IsAcceptingText;
                }
                return true;
            });
        }

        public string LanguageRegion
        {
            get
            {
                var imm = (InputMethodManager)Forms9Patch.Droid.Settings.Context.GetSystemService(Context.InputMethodService);
                var ims = imm.CurrentInputMethodSubtype;
                var result = ims?.LanguageTag.Replace('_', '-');
                //var result = ims?.Locale.Replace('_', '-');

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
    }
}
