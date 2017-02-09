using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Forms9Patch
{
	/// <summary>
	/// Keyboard service.
	/// </summary>
	public static class KeyboardService
	{
		static IKeyboardService _service;

		static KeyboardService()
		{
			_service = _service ?? DependencyService.Get<IKeyboardService>();
		}

		/// <summary>
		/// Forces the device's on screen keyboard to be hidden
		/// </summary>
		public static void Hide()
		{
			_service = _service ?? DependencyService.Get<IKeyboardService>();
			_service?.Hide();
		}

		public static void OnVisiblityChange(KeyboardVisibilityChange state)
		{
			switch (state)
			{
				case KeyboardVisibilityChange.Shown:
					Shown?.Invoke(null, EventArgs.Empty);
					break;
				case KeyboardVisibilityChange.Hidden:
					Hidden?.Invoke(null, EventArgs.Empty);
					break;
			}
		}

		public static event EventHandler Hidden;
		public static event EventHandler Shown;
	}

	public enum KeyboardVisibilityChange
	{
		Shown,
		Hidden
	}
}
