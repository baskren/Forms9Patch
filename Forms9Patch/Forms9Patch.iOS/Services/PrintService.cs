using System;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(Forms9Patch.iOS.PrintService))]
namespace Forms9Patch.iOS
{

    /// <summary>
    /// Web view extensions service.
    /// </summary>
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class PrintService : UIPrintInteractionControllerDelegate, IPrintService
    {
        //UIView AppleViewToPrint;
        //WebView ViewToPrint;

        /// <summary>
        /// Print the specified viewToPrint and jobName.
        /// </summary>
        /// <param name="viewToPrint">View to print.</param>
        /// <param name="jobName">Job name.</param>
        public async Task PrintAsync(WebView viewToPrint, string jobName, FailAction failAction)
        {
            var effectApplied = viewToPrint.Effects.Any(e => e is Forms9Patch.WebViewPrintEffect);
            var actualSource = viewToPrint.ActualSource() as WebViewSource;
            if (effectApplied && actualSource!=null)
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

                string html = null;

                if (actualSource is HtmlWebViewSource htmlSource)
                    html = htmlSource.Html;
                else if (actualSource is EmbeddedHtmlViewSource embeddedHtmlViewSource)
                    html = embeddedHtmlViewSource.Html;
                /*
                else if (actualSource is UrlWebViewSource urlSource
                    && urlSource.Url is string url
                    && !string.IsNullOrWhiteSpace(url)
                    && url.StartsWith($"file://", StringComparison.OrdinalIgnoreCase))
                {
                    var path = url.Substring(7).TrimEnd('/');
                        html = System.IO.File.ReadAllText(path);
                }
                */

                if (!string.IsNullOrWhiteSpace(html))
                    printController.PrintFormatter = new UIMarkupTextPrintFormatter(html);
                else if (actualSource is UrlWebViewSource urlSource
                    && urlSource.Url is string url
                    && !string.IsNullOrWhiteSpace(url)
                    && Foundation.NSUrl.FromString(url) is Foundation.NSUrl candidateUrl
                    && candidateUrl.Scheme != null
                    )
                {
                    //printController.PrintingItem = printData;
                    //printController.PrintingItem = new NSUrl(url);
                    if (await Forms9Patch.ToPdfService.ToPdfAsync(viewToPrint, "Forms9Patch.PdfPrintFile") is ToFileResult result)
                    {
                        if (result.IsError)
                        {
                            using (var alert = Forms9Patch.Alert.Create("Print File Generation Error", result.Result))
                            {
                                await alert.WaitForPoppedAsync();
                            }
                            return;
                        }
                        else
                        {
                            var fileUrl = NSUrl.CreateFileUrl(result.Result, null);
                            printController.PrintingItem = fileUrl;
                        }
                    }
                }
                else
                    printController.PrintFormatter = Platform.CreateRenderer(viewToPrint).NativeView.ViewPrintFormatter;

                printController.Present(true, (printInteractionController, completed, error) =>
                {
                    System.Diagnostics.Debug.WriteLine(GetType() + "." + P42.Utils.ReflectionExtensions.CallerMemberName() + ": PRESENTED completed[" + completed + "] error[" + error + "]");
                });
            }
            else
                throw new Exception("Cannot print WebView without first calling Forms9Patch.WebViewPrintEffect.AttachTo(webView) BEFORE setting webview.Source.  ");

        }

        /// <summary>
        /// Cans the print.
        /// </summary>
        /// <returns><c>true</c>, if print was caned, <c>false</c> otherwise.</returns>
        public bool CanPrint()
        {
            return UIPrintInteractionController.PrintingAvailable;
        }

        public Task PrintAsync(string html, string jobName, FailAction failAction)
        {
            var webView = new Xamarin.Forms.WebView();
            WebViewPrintEffect.ApplyTo(webView);
            webView.Source = new HtmlWebViewSource{ Html = html };
            return PrintAsync(webView, jobName, failAction);
        }
    }
}
