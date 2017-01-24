using System;
using PCLStorage;
using Xamarin.Forms;

namespace Forms9Patch
{
	public static class HtmlStringExtensions
	{
		static IHtmlToPdfService _htmlService;

		public static void ToPdf(this string html, Size pageSize, IFolder folder, string fileName, Action<IFile> onComplete)
		{
			_htmlService = _htmlService ?? DependencyService.Get<IHtmlToPdfService>();
			if (_htmlService == null)
				throw new NotSupportedException("Cannot get HtmlService: must not be supported on this platform.");
			_htmlService.ToPdf(html, pageSize, folder, fileName, onComplete);
		}
	}
}
