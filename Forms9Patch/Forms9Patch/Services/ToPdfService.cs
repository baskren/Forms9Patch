using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Html string extensions.
    /// </summary>
    public static class ToPdfService
    {
        static ToPdfService()
        {
            Settings.ConfirmInitialization();
        }

        static IToPdfService _platformToPdfService;

        /// <summary>
        /// Returns true if PDF generation is available on this device
        /// </summary>
        public static bool IsAvailable
        {
            get
            {
                _platformToPdfService = _platformToPdfService ?? DependencyService.Get<IToPdfService>();
                if (_platformToPdfService == null)
                    return false;
                return _platformToPdfService.IsAvailable;
            }
        }

        /// <summary>
        /// Converts HTML text to PNG
        /// </summary>
        /// <param name="html">HTML string to be converted to PDF</param>
        /// <param name="fileName">Name (not path), excluding suffix, of PDF file</param>
        /// <param name="pageSize">PDF page size, in points. (default based upon user's region)</param>
        /// <param name="margin">PDF page's margin, in points. (default is zero)</param>
        /// <returns></returns>
        public static async Task<ToFileResult> ToPdfAsync(this string html, string fileName, PageSize pageSize = default, PageMargin margin = default)
        {

            _platformToPdfService = _platformToPdfService ?? DependencyService.Get<IToPdfService>();
            if (_platformToPdfService == null)
                throw new NotSupportedException("Cannot get HtmlService: must not be supported on this platform.");
            ToFileResult result = null;
            using (var indicator = ActivityIndicatorPopup.Create())
            {
                if (pageSize is null || pageSize.Width <= 0 || pageSize.Height <= 0)
                    pageSize = PageSize.Default;

                margin = margin ?? new PageMargin();
                if (pageSize.Width - margin.HorizontalThickness < 1 || pageSize.Height - margin.VerticalThickness < 1)
                    return new ToFileResult(true, "Page printable area (page size - margins) has zero width or height.");

                result = await _platformToPdfService.ToPdfAsync(html, fileName, pageSize, margin);
                await indicator.CancelAsync();
            }
            await Task.Delay(50);
            return result;
        }

        /// <summary>
        /// Creates a PNG from the contents of a Xamarin.Forms.WebView
        /// </summary>
        /// <param name="webView">Xamarin.Forms.WebView</param>
        /// <param name="fileName">Name (not path), excluding suffix, of PDF file</param>
        /// <param name="pageSize">PDF page size, in points. (default based upon user's region)</param>
        /// <param name="margin">PDF page's margin, in points. (default is zero)</param>
        /// <returns>Forms9Patch.ToFileResult</returns>
        public static async Task<ToFileResult> ToPdfAsync(this Xamarin.Forms.WebView webView, string fileName, PageSize pageSize = default, PageMargin margin = default)
        {
            _platformToPdfService = _platformToPdfService ?? DependencyService.Get<IToPdfService>();
            if (_platformToPdfService == null)
                throw new NotSupportedException("Cannot get HtmlService: must not be supported on this platform.");
            ToFileResult result = null;
            using (var indicator = ActivityIndicatorPopup.Create())
            {
                if (pageSize is null || pageSize.Width <= 0 || pageSize.Height <= 0)
                    pageSize = PageSize.Default;

                margin = margin ?? new PageMargin();
                if (pageSize.Width - margin.HorizontalThickness < 1 || pageSize.Height - margin.VerticalThickness < 1)
                    return new ToFileResult(true, "Page printable area (page size - margins) has zero width or height.");

                result = await _platformToPdfService.ToPdfAsync(webView, fileName, pageSize, margin);
                await indicator.CancelAsync();
            }
            await Task.Delay(50);
            return result;
        }
    }
}
