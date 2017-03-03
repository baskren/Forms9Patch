using System;
using Xamarin.Forms;

namespace Forms9Patch
{
	/// <summary>
	/// Haptics service.
	/// </summary>
	public static class KeyClicksService 
	{
		static IKeyClicksService _service;

		/// <summary>
		/// Feedback the specified effect and mode.
		/// </summary>
		/// <param name="effect">Effect.</param>
		/// <param name="mode">Mode.</param>
		public static void Feedback(HapticEffect effect, KeyClicks mode=KeyClicks.Default)
		{
			if (mode == KeyClicks.Off)
				return;
			_service = _service ?? DependencyService.Get<IKeyClicksService>();
			_service?.Feedback(effect,mode);
		}

	}
}
