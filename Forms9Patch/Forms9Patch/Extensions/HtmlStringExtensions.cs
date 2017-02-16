using System;
using Xamarin.Forms;

namespace Forms9Patch
{
	/// <summary>
	/// Html string extensions.
	/// </summary>
	public static class HtmlStringExtensions
	{
		static IHtmlToPngPdfService _htmlService;

		/// <summary>
		/// Tos the png.
		/// </summary>
		/// <param name="html">Html.</param>
		/// <param name="fileName">File name.</param>
		/// <param name="onComplete">On complete.</param>
		public static void ToPng(this string html, string fileName, Action<string> onComplete)
		{
			_htmlService = _htmlService ?? DependencyService.Get<IHtmlToPngPdfService>();
			if (_htmlService == null)
				throw new NotSupportedException("Cannot get HtmlService: must not be supported on this platform.");
			_htmlService.ToPng(html, fileName, onComplete);
		}

	}
}
