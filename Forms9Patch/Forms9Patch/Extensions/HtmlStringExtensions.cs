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
		/// converts the string (assumed to be HTML) to a PNG file and passess the path of the file as the argument to the onComplete action.
		/// </summary>
		/// <param name="html">Html.</param>
		/// <param name="pageSize">Page size.</param>
		/// <param name="fileName">File name.</param>
		/// <param name="onComplete">On complete.</param>
		public static void ToPng(this string html, Size pageSize,  string fileName, Action<string> onComplete)
		{
			_htmlService = _htmlService ?? DependencyService.Get<IHtmlToPngPdfService>();
			if (_htmlService == null)
				throw new NotSupportedException("Cannot get HtmlService: must not be supported on this platform.");
			_htmlService.ToPng(html, pageSize, fileName, onComplete);
		}

	}
}
