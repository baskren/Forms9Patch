using System;
using System.Linq;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(Forms9Patch.iOS.WebViewExtensionsService))]
namespace Forms9Patch.iOS
{

    /// <summary>
    /// Web view extensions service.
    /// </summary>
    public class WebViewExtensionsService : UIPrintInteractionControllerDelegate, IWebViewExtensionService
    {
        //UIView AppleViewToPrint;
        //WebView ViewToPrint;

        /// <summary>
        /// Print the specified viewToPrint and jobName.
        /// </summary>
        /// <param name="viewToPrint">View to print.</param>
        /// <param name="jobName">Job name.</param>
        public void Print(WebView viewToPrint, string jobName)
        {
            var printInfo = UIPrintInfo.PrintInfo;

            printInfo.JobName = jobName;
            printInfo.Duplex = UIPrintInfoDuplex.None;
            printInfo.OutputType = UIPrintInfoOutputType.General;

            var printController = UIPrintInteractionController.SharedPrintController;
            printController.ShowsPageRange = true;
            printController.ShowsPaperSelectionForLoadedPapers = true;
            printController.PrintInfo = printInfo;
            printController.Delegate = this;

            if (viewToPrint.Source is HtmlWebViewSource htmlSource)
                printController.PrintFormatter = new UIMarkupTextPrintFormatter(htmlSource.Html);
            else if (viewToPrint.Source is UrlWebViewSource urlSource
                && urlSource.Url is string url
                && !string.IsNullOrWhiteSpace(url)
                && Foundation.NSUrl.FromString(url) is Foundation.NSUrl candidateUrl
                && candidateUrl.Scheme != null)
            {
                var printData = Foundation.NSData.FromUrl(candidateUrl);
                printController.PrintingItem = printData;
            }
            else
                printController.PrintFormatter = Platform.CreateRenderer(viewToPrint).NativeView.ViewPrintFormatter;

            printController.Present(true, (printInteractionController, completed, error) =>
            {
                System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + ": PRESENTED completed[" + completed + "] error[" + error + "]");
            });
        }

        /// <summary>
        /// Cans the print.
        /// </summary>
        /// <returns><c>true</c>, if print was caned, <c>false</c> otherwise.</returns>
        public bool CanPrint()
        {
            return UIPrintInteractionController.PrintingAvailable;
        }

        public void Print(string html, string jobName)
        {
            var webView = new Xamarin.Forms.WebView
            {
                Source = new HtmlWebViewSource
                {
                    Html = html
                }
            };
            Print(webView, jobName);
        }
    }
}
