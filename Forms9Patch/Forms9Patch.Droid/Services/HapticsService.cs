using System;
using Android.Content;
using Android.OS;
using Forms9Patch.Interfaces;
using Android.Media;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.Droid.HapticsService))]

namespace Forms9Patch.Droid
{
    public class HapticsService : IHapticsService
    {
        readonly static Vibrator _vibrator = (Vibrator)Android.App.Application.Context.GetSystemService(Context.VibratorService);

        static bool _appEnabledTested;
        static bool _appEnabled;
        static bool AppEnabled
        {
            get
            {
                if (!_appEnabledTested)
                {
                    _appEnabled = Android.App.Application.Context.CheckCallingOrSelfPermission("android.permission.VIBRATE") == Android.Content.PM.Permission.Granted;
                    _appEnabledTested = true;
                }
                return _appEnabled;
            }
        }

        Android.Media.AudioAttributes _attributes;
        Android.Media.AudioAttributes Attributes
        {
            get
            {
                if (_attributes == null)
                {
                    using (var builder = new Android.Media.AudioAttributes.Builder())
                    {
                        builder.SetContentType(AudioContentType.Sonification);
                        _attributes = builder.Build();
                    }
                }
                return _attributes;
            }
        }

        public void Feedback(HapticEffect effect, EffectMode mode = EffectMode.Default)
        {
            if (effect == HapticEffect.None)
                return;
            var hapticEnabled = mode == EffectMode.On;
            if (mode == EffectMode.Default)
                hapticEnabled = (Forms9Patch.Settings.HapticEffectMode > EffectMode.Off)
                    && Android.Provider.Settings.System.GetInt(Android.App.Application.Context.ContentResolver, Android.Provider.Settings.System.HapticFeedbackEnabled) != 0;
            if (hapticEnabled && AppEnabled)
            {
                if (effect == HapticEffect.Selection)
                    Settings.Activity.Window.DecorView.PerformHapticFeedback(Android.Views.FeedbackConstants.KeyboardTap);
                else if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                {
                    VibrationEffect droidEffect = null;
                    switch (effect)
                    {
                        case HapticEffect.LightImpact:
                            droidEffect = VibrationEffect.CreateOneShot(200, 128);
                            break;
                        case HapticEffect.MediumImpact:
                            droidEffect = VibrationEffect.CreateOneShot(200, 196);
                            break;
                        case HapticEffect.HeavyImpact:
                            droidEffect = VibrationEffect.CreateOneShot(200, 255);
                            break;
                        case HapticEffect.ErrorNotification:
                            droidEffect = VibrationEffect.CreateWaveform(new long[] { 0, 200, 100, 200, 100, 200 }, new int[] { 0, 196, 0, 196, 0, 255 }, -1);
                            break;
                        case HapticEffect.WarningNotification:
                            droidEffect = VibrationEffect.CreateWaveform(new long[] { 0, 200, 100, 200 }, new int[] { 0, 196, 0, 255 }, -1);
                            break;
                        case HapticEffect.SuccessNotification:
                            droidEffect = VibrationEffect.CreateWaveform(new long[] { 0, 200, 100, 200 }, new int[] { 0, 255, 0, 196 }, -1);
                            break;
                    }
                    if (droidEffect != null)
                        _vibrator.Vibrate(droidEffect);
                }
                else
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    long[] pattern = null;
                    switch (effect)
                    {
                        case HapticEffect.LightImpact:
                            _vibrator.Vibrate(200, Attributes);
                            break;
                        case HapticEffect.MediumImpact:
                            _vibrator.Vibrate(200, Attributes);
                            break;
                        case HapticEffect.HeavyImpact:
                            _vibrator.Vibrate(200, Attributes);
                            break;
                        case HapticEffect.ErrorNotification:
                            pattern = new long[] { 0, 200, 100, 200, 100, 200 };
                            break;
                        case HapticEffect.WarningNotification:
                            pattern = new long[] { 0, 200, 100, 200 };
                            break;
                        case HapticEffect.SuccessNotification:
                            pattern = new long[] { 0, 200, 100, 200 };
                            break;
                    }
                    if (pattern != null)
                    {
                        _vibrator.Vibrate(pattern, -1, Attributes);
                    }
#pragma warning restore CS0618 // Type or member is obsolete
                }
            }
        }
    }
}
