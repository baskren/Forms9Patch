using System;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: Dependency(typeof(Forms9Patch.UWP.WebViewExtensionsService))]
namespace Forms9Patch.UWP
{
    /// <summary>
    /// Web view extensions service.
    /// </summary>
    public class WebViewExtensionsService : IWebViewExtensionService
    {

        /// <summary>
        /// Cans the print.
        /// </summary>
        /// <returns><c>true</c>, if print was caned, <c>false</c> otherwise.</returns>
        public bool CanPrint()
        {
            return false;
        }

        public void Print(Xamarin.Forms.WebView webView, string jobName)
        {
            throw new NotImplementedException();
        }
    }
}