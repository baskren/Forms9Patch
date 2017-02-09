using System;
namespace Forms9Patch
{
	/// <summary>
	/// The different haptic modes
	/// </summary>
	public enum HapticMode
	{
		/// <summary>
		/// No haptic response
		/// </summary>
		Off,
		/// <summary>
		/// The system default haptic, if detectable (which it is not on iOS)
		/// </summary>
		SystemDefault,
		/// <summary>
		/// The default haptic response (as set by Forms9Patch.Settings.Haptics)
		/// </summary>
		ApplicationDefault,
		/// <summary>
		/// GIVE ME HAPTICS!
		/// </summary>
		Forced
	}
}
