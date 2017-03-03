using System;
namespace Forms9Patch
{
	/// <summary>
	/// The different haptic modes
	/// </summary>
	[Flags]
	public enum HapticMode
	{
		/// <summary>
		/// No haptic response
		/// </summary>
		Off=0,
		/// <summary>
		/// The system default haptic, if detectable (which it is not on iOS)
		/// </summary>
		Default=1,
		/// <summary>
		/// GIVE ME TOUCH SOUNDS!
		/// </summary>
		Sound=2,
		/// <summary>
		/// GIVE ME TOUCH VIBRATES!
		/// </summary>
		Vibrate=4,
		/// <summary>
		/// GIVE ME SOUNDS AND VIBRATIONS!
		/// </summary>
		SoundAndVibrate=6
	}
}
