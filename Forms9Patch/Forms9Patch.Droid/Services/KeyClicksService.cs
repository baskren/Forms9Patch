using Android.Content;
using Android.OS;
using Android.Media;
using Android.Content.PM;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.Droid.KeyClicksService))]
namespace Forms9Patch.Droid
{
	public class KeyClicksService : IKeyClicksService
	{
		readonly static Vibrator _vibrator = (Vibrator)Android.App.Application.Context.GetSystemService(Context.VibratorService);
		readonly static AudioManager _audio = (AudioManager)Android.App.Application.Context.GetSystemService(Context.AudioService);

		public void Feedback(HapticEffect effect, KeyClicks mode = KeyClicks.Default)
		{
			var soundEnabled = (mode & KeyClicks.On) > 0;
			if (mode == KeyClicks.Default)
			{
				soundEnabled = (Forms9Patch.Settings.KeyClicks & KeyClicks.On) > 0;
				if (Forms9Patch.Settings.KeyClicks == KeyClicks.Default)
				{
					soundEnabled = Android.Provider.Settings.System.GetInt(Android.App.Application.Context.ContentResolver, Android.Provider.Settings.System.SoundEffectsEnabled) != 0;
					//vibeEnabled = Android.Provider.Settings.System.GetInt(Android.App.Application.Context.ContentResolver, Android.Provider.Settings.System.HapticFeedbackEnabled) != 0;
				}
			}


			var permission = Android.App.Application.Context.CheckCallingOrSelfPermission("android.permission.VIBRATE");

			if (_vibrator != null && permission == Permission.Granted && Android.Provider.Settings.System.GetInt(Android.App.Application.Context.ContentResolver, Android.Provider.Settings.System.HapticFeedbackEnabled) != 0)
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
