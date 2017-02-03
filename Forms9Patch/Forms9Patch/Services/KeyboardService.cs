using System;
using Xamarin.Forms;

namespace Forms9Patch
{
	public static class KeyboardService
	{
		static IKeyboardService _service;

		public static void Hide()
		{
			_service = _service ?? DependencyService.Get<IKeyboardService>();
			_service?.Hide();
		}
	}
}
