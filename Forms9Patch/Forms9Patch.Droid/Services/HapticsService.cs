using Android.Content;
using Android.OS;
using Android.Media;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.Droid.HapticsService))]
namespace Forms9Patch.Droid
{
	public class HapticsService : IHapticService
	{
		readonly static Vibrator _vibrator = (Vibrator)Android.App.Application.Context.GetSystemService(Context.VibratorService);
		readonly static AudioManager _audio = (AudioManager)Android.App.Application.Context.GetSystemService(Context.AudioService);

		public void Feedback(HapticEffect effect, HapticMode mode = HapticMode.Default)
		{
			var soundEnabled = (mode & HapticMode.Sound) > 0;
			var vibeEnabled = (mode & HapticMode.Vibrate) > 0;
			if (mode == HapticMode.Default)
			{
				soundEnabled = (Forms9Patch.Settings.HapticMode & HapticMode.Sound) > 0;
				vibeEnabled = (Forms9Patch.Settings.HapticMode & HapticMode.Vibrate) > 0;
				if (Forms9Patch.Settings.HapticMode == HapticMode.Default)
				{
					soundEnabled = Android.Provider.Settings.System.GetInt(Android.App.Application.Context.ContentResolver, Android.Provider.Settings.System.SoundEffectsEnabled) != 0;
					vibeEnabled = Android.Provider.Settings.System.GetInt(Android.App.Application.Context.ContentResolver, Android.Provider.Settings.System.HapticFeedbackEnabled) != 0;
				}
			}



			if (_vibrator != null && vibeEnabled)
				_vibrator.Vibrate(300);


			if (_audio != null && soundEnabled)
			{
				SoundEffect sound = SoundEffect.KeyClick;
				switch (effect)
				{
					case HapticEffect.None:
						return;
					case HapticEffect.KeyClick:
						break;
					case HapticEffect.Return:
						sound = SoundEffect.Return;
						break;
					case HapticEffect.Delete:
						sound = SoundEffect.Delete;
						break;
				}
				_audio.PlaySoundEffect(sound);
			}
		}

	}
}
