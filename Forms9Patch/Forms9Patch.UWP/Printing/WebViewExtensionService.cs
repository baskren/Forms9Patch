using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Graphics.Printing;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
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
            return PrintManager.IsSupported();
        }

        public void Print(Xamarin.Forms.WebView webView, string jobName)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (Platform.GetRenderer(webView) is Xamarin.Forms.Platform.UWP.WebViewRenderer renderer && renderer.Control is Windows.UI.Xaml.Controls.WebView nativeWebView)
                {
                    if (string.IsNullOrWhiteSpace(jobName))
                        jobName = Forms9Patch.ApplicationInfoService.Name;
                    nativeWebView.Name = jobName ?? "Forms9Patch.WebViewPrint";

                    // Initalize common helper class and register for printing
                    var printHelper = new WebViewPrintHelper(nativeWebView);
                    printHelper.RegisterForPrinting();
                    await printHelper.Init();
                    bool showprint = await PrintManager.ShowPrintUIAsync(jobName);

                    //printHelper.UnregisterForPrinting();

                }
            });
        }

    }
}