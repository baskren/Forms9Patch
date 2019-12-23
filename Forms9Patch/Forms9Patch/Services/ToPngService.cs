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
        /// OBSOLETE
        /// </summary>
        /// <param name="html"></param>
        /// <param name="fileName"></param>
        /// <param name="onComplete"></param>
        [Obsolete("Use ToPngAsync instead", true)]
        public static void ToPng(this string html, string fileName, Action<string> onComplete)
        { }

        /// <summary>
        /// Converts HTML text to PNG
        /// </summary>
        /// <param name="html"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static async Task<ToPngResult> ToPngAsync(this string html, string fileName)
        {
            _platformToPngService = _platformToPngService ?? DependencyService.Get<IToPngService>();
            if (_platformToPngService == null)
                throw new NotSupportedException("Cannot get HtmlService: must not be supported on this platform.");
            ToPngResult result = null;
            using (var indicator = ActivityIndicatorPopup.Create())
            {
                result = await _platformToPngService.ToPngAsync(indicator, html, fileName);
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
        public static async Task<ToPngResult> ToPngAsync(this Xamarin.Forms.WebView webView, string fileName)
        {
            _platformToPngService = _platformToPngService ?? DependencyService.Get<IToPngService>();
            if (_platformToPngService == null)
                throw new NotSupportedException("Cannot get HtmlService: must not be supported on this platform.");
            ToPngResult result = null;
            using (var indicator = ActivityIndicatorPopup.Create())
            {
                result = await _platformToPngService.ToPngAsync(indicator, webView, fileName);
            }
            await Task.Delay(50);
            return result;
        }
    }
}
