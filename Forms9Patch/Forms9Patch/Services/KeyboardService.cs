using System;
using Xamarin.Forms;

namespace Forms9Patch
{
	/// <summary>
	/// Keyboard service.
	/// </summary>
	public static class KeyboardService
	{
		static IKeyboardService _service;

		/// <summary>
		/// Forces the device's on screen keyboard to be hidden
		/// </summary>
		public static void Hide()
		{
			_service = _service ?? DependencyService.Get<IKeyboardService>();
			_service?.Hide();
		}
	}
}
