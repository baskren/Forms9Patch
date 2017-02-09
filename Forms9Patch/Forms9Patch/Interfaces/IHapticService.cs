using System;
namespace Forms9Patch
{
	/// <summary>
	/// Haptic service.
	/// </summary>
	public interface IHapticService
	{
		/// <summary>
		/// Feedback the specified effect and mode.
		/// </summary>
		/// <param name="effect">Effect.</param>
		/// <param name="mode">Mode.</param>
		void Feedback(HapticEffect effect, HapticMode mode=HapticMode.ApplicationDefault);

	}
}
