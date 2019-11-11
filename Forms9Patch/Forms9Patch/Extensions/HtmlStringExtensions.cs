using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Html string extensions.
    /// </summary>
    public static class HtmlStringExtensions
    {
        static HtmlStringExtensions()
        {
            Settings.ConfirmInitialization();
        }

        static IHtmlToPngPdfService _htmlService;

        [Obsolete("Use ToPngAsync instead", true)]
        public static void ToPng(this string html, string fileName, Action<string> onComplete)
        { }

        /// <summary>
        /// Tos the png.
        /// </summary>
        /// <param name="html">Html.</param>
        /// <param name="fileName">File name.</param>
        /// <param name="onComplete">On complete.</param>
        public static async Task<HtmlToPngResult> ToPngAsync(this string html, string fileName)
        {
            _htmlService = _htmlService ?? DependencyService.Get<IHtmlToPngPdfService>();
            if (_htmlService == null)
                throw new NotSupportedException("Cannot get HtmlService: must not be supported on this platform.");
            HtmlToPngResult result = null;
            using (var indicator = ActivityIndicatorPopup.Create())
            {
                result = await _htmlService.ToPngAsync(indicator, html, fileName);
            }
            await Task.Delay(50);
            return result;
        }

    }
}
