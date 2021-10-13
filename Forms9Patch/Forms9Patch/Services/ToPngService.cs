using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Html string extensions.
    /// </summary>
    public static class ToPngService
    {
        static ToPngService()
        {
            Settings.ConfirmInitialization();
        }

        static IToPngService _platformToPngService;


        /// <summary>
        /// Converts HTML text to PNG
        /// </summary>
        /// <param name="html">HTML string to be converted to PNG</param>
        /// <param name="fileName">Name (not path), excluding suffix, of PNG file</param>
        /// <param name="width">Width of resulting PNG (in pixels).</param>
        /// <returns></returns>
        public static async Task<ToFileResult> ToPngAsync(this string html, string fileName, int width = -1)
        {
            _platformToPngService = _platformToPngService ?? DependencyService.Get<IToPngService>();
            if (_platformToPngService == null)
                throw new NotSupportedException("Cannot get HtmlService: must not be supported on this platform.");
            ToFileResult result = null;
            using (var indicator = ActivityIndicatorPopup.Create())
            {
                if (width <= 0)
                    width = (int)Math.Ceiling((PageSize.Default.Width - 0.5) * 4);
                result = await _platformToPngService.ToPngAsync(html, fileName, width);
                await indicator.CancelAsync();
            }
            await Task.Delay(50);
            return result;
        }

        /// <summary>
        /// Creates a PNG from the contents of a Xamarin.Forms.WebView
        /// </summary>
        /// <param name="webView">Xamarin.Forms.WebView</param>
        /// <param name="fileName">Name (not path), excluding suffix, of PNG file</param>
        /// <param name="width">Width of resulting PNG (in pixels).</param>
        /// <returns></returns>
        public static async Task<ToFileResult> ToPngAsync(this Xamarin.Forms.WebView webView, string fileName, int width = -1)
        {
            _platformToPngService = _platformToPngService ?? DependencyService.Get<IToPngService>();
            if (_platformToPngService == null)
                throw new NotSupportedException("Cannot get HtmlService: must not be supported on this platform.");
            ToFileResult result = null;
            using (var indicator = ActivityIndicatorPopup.Create())
            {
                if (width <= 0)
                    width = (int)Math.Ceiling((PageSize.Default.Width - (12 * 25.4)) * 4);
                result = await _platformToPngService.ToPngAsync(webView, fileName, width);
                await indicator.CancelAsync();
            }
            await Task.Delay(50);
            return result;
        }
    }
}
