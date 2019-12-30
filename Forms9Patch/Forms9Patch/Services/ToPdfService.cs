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
        /// <param name="html"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static async Task<ToFileResult> ToPdfAsync(this string html, string fileName)
        {
            _platformToPdfService = _platformToPdfService ?? DependencyService.Get<IToPdfService>();
            if (_platformToPdfService == null)
                throw new NotSupportedException("Cannot get HtmlService: must not be supported on this platform.");
            ToFileResult result = null;
            using (var indicator = ActivityIndicatorPopup.Create())
            {
                result = await _platformToPdfService.ToPdfAsync(html, fileName);
            }
            await Task.Delay(50);
            return result;
        }

        /// <summary>
        /// Creates a PNG from the contents of a Xamarin.Forms.WebView
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static async Task<ToFileResult> ToPdfAsync(this Xamarin.Forms.WebView webView, string fileName)
        {
            _platformToPdfService = _platformToPdfService ?? DependencyService.Get<IToPdfService>();
            if (_platformToPdfService == null)
                throw new NotSupportedException("Cannot get HtmlService: must not be supported on this platform.");
            ToFileResult result = null;
            using (var indicator = ActivityIndicatorPopup.Create())
            {
                result = await _platformToPdfService.ToPdfAsync(webView, fileName);
            }
            await Task.Delay(50);
            return result;
        }
    }
}
