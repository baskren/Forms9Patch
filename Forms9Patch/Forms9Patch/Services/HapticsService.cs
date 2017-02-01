using System;
using Xamarin.Forms;

namespace Forms9Patch
{
	public static class HapticsService 
	{
		static IHapticService _service;

		public static void Feedback(HapticEffect effect, HapticMode mode=HapticMode.ApplicationDefault)
		{
			if (mode == HapticMode.Off || (mode == HapticMode.ApplicationDefault && !Settings.Haptics))
				return;
			_service = _service ?? DependencyService.Get<IHapticService>();
			_service?.Feedback(effect,mode);
		}

	}
}
