using System;
using Xamarin.Forms;

namespace Forms9Patch
{
	/// <summary>
	/// Haptics service.
	/// </summary>
	public static class HapticsService 
	{
		static IHapticService _service;

		/// <summary>
		/// Feedback the specified effect and mode.
		/// </summary>
		/// <param name="effect">Effect.</param>
		/// <param name="mode">Mode.</param>
		public static void Feedback(HapticEffect effect, HapticMode mode=HapticMode.ApplicationDefault)
		{
			if (mode == HapticMode.Off || (mode == HapticMode.ApplicationDefault && !Settings.Haptics))
				return;
			_service = _service ?? DependencyService.Get<IHapticService>();
			_service?.Feedback(effect,mode);
		}

	}
}
