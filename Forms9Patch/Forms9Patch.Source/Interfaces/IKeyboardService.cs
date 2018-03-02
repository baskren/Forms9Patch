using System;
namespace Forms9Patch
{
	/// <summary>
	/// Keyboard service.
	/// </summary>
	public interface IKeyboardService
	{
		/// <summary>
		/// Forces the device's on screen keyboard to be hidden.
		/// </summary>
		void Hide();

        bool IsHardwareKeyboardActive { get; }
	}
}
