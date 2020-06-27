using Xamarin.Forms;
using System;
using System.Threading.Tasks;

namespace Forms9Patch
{
    /// <summary>
    /// Extensions to XF WebView
    /// </summary>
    public static class PrintService
    {
        static PrintService()
        {
            Settings.ConfirmInitialization();
        }

        static IPrintService _service;

        internal static WebViewSource ActualSource(this WebView webView)
            => (WebViewSource)webView.GetValue(Forms9Patch.WebViewPrintEffect.ActualSourceProperty);

        [Obsolete("Please use PrintAsync", true)]
        public static void Print(this WebView webview, string jobName)
        {
            throw new Exception("Forms9Patch.PrintService.Print is obsolete.  Please use PrintAsync");
        }

        /// <summary>
        /// Print the specified webview and jobName.
        /// </summary>
        /// <param name="webview">Webview.</param>
        /// <param name="jobName">Job name.</param>
        public static Task PrintAsync(this WebView webview, string jobName)
        {
            _service = _service ?? DependencyService.Get<IPrintService>();
            if (_service == null)
                throw new NotSupportedException("Cannot get IWebViewService: must not be supported on this platform.");
            return _service.PrintAsync(webview, jobName ?? ApplicationInfoService.Name);
        }

        [Obsolete("Please use PrintAsync", true)]
        public static void Print(this string html, string jobName)
        {
            throw new Exception("Forms9Patch.PrintService.Print is obsolete.  Please use PrintAsync");
        }

        /// <summary>
        /// Print HTML string
        /// </summary>
        /// <param name="html"></param>
        /// <param name="jobName"></param>
        public static Task PrintAsync(this string html, string jobName)
        {
            _service = _service ?? DependencyService.Get<IPrintService>();
            if (_service == null)
                throw new NotSupportedException("Cannot get IWebViewService: must not be supported on this platform.");
            return _service.PrintAsync(html, jobName ?? ApplicationInfoService.Name);
        }


        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Forms9Patch.WebViewExtensions"/> can print.
        /// </summary>
        /// <value><c>true</c> if can print; otherwise, <c>false</c>.</value>
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
