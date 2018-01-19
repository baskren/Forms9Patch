using System;
namespace Forms9Patch
{
	/// <summary>
	/// Haptic service.
	/// </summary>
	public interface IKeyClicksService
	{
		/// <summary>
		/// Feedback the specified effect and mode.
		/// </summary>
		/// <param name="effect">Effect.</param>
		/// <param name="mode">Mode.</param>
		void Feedback(HapticEffect effect, KeyClicks mode=KeyClicks.Default);

	}
}
