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

		public void Feedback(HapticEffect effect, HapticMode mode = HapticMode.ApplicationDefault)
		{
			var touchSoundEnabled = Android.Provider.Settings.System.GetInt(Android.App.Application.Context.ContentResolver, Android.Provider.Settings.System.SoundEffectsEnabled)!=0;
			var touchVibrateEnabled = Android.Provider.Settings.System.GetInt(Android.App.Application.Context.ContentResolver, Android.Provider.Settings.System.HapticFeedbackEnabled)!=0;


			if (_vibrator != null && ((mode == HapticMode.SystemDefault && touchVibrateEnabled) || mode == HapticMode.Forced))
				_vibrator.Vibrate(300);


			if (_audio != null && ((mode == HapticMode.SystemDefault && touchSoundEnabled) || mode == HapticMode.Forced))
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
