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

		/// <summary>
		/// Initializes the <see cref="T:Forms9Patch.KeyboardService"/> class.
		/// </summary>
		static IKeyboardService Service
		{
            get
            {
                Settings.ConfirmInitialization();
                _service = _service ?? DependencyService.Get<IKeyboardService>();
                if (_service == null)
                {
                    System.Diagnostics.Debug.WriteLine("KeyboardService is not available");
                    //throw new ServiceNotAvailableException("KeyboardService is not available");
                }
                return _service;
            }
		}

		/// <summary>
		/// Forces the device's on screen keyboard to be hidden
		/// </summary>
		public static void Hide() => Service.Hide();

		/// <summary>
		/// Ons the visiblity change.
		/// </summary>
		/// <param name="state">State.</param>
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

		/// <summary>
		/// Occurs when hidden.
		/// </summary>
		public static event EventHandler Hidden;
		/// <summary>
		/// Occurs when shown.
		/// </summary>
		public static event EventHandler Shown;

        public static bool IsHardwareKeyboardActive => Service.IsHardwareKeyboardActive;
	}

	/// <summary>
	/// Keyboard visibility change.
	/// </summary>
	public enum KeyboardVisibilityChange
	{
		/// <summary>
		/// The keyboard will show/has shown.
		/// </summary>
		Shown,
		/// <summary>
		/// The keyboard will hide/has been hidden.
		/// </summary>
		Hidden
	}
}
