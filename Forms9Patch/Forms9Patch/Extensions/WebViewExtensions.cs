using Xamarin.Forms;
using System;

namespace Forms9Patch
{
	public static class WebViewExtensions
	{
		static IPrintService _service;

		public static void Print(this WebView webview, string jobName)
		{
			_service = _service ?? DependencyService.Get<IPrintService>();
			if (_service == null)
				throw new NotSupportedException("Cannot get PrintService: must not be supported on this platform.");
			_service.Print(webview, jobName);
		}

		public static bool CanPrint
		{
			get
			{
				_service = _service ?? DependencyService.Get<IPrintService>();
				if (_service == null)
					return false;
				return _service.CanPrint();
			}
		}
	}
}
