using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using UIKit;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(Forms9Patch.iOS.ToPdfService))]
namespace Forms9Patch.iOS
{
    public class ToPdfService : UIPrintInteractionControllerDelegate, IToPdfService
    {
        const string LocalStorageFolderName = "Forms9Patch.ToPdfService";

        public static string FolderPath()
        {
            P42.Utils.DirectoryExtensions.AssureExists(P42.Utils.Environment.TemporaryStoragePath);
            var root = Path.Combine(P42.Utils.Environment.TemporaryStoragePath, LocalStorageFolderName);
            P42.Utils.DirectoryExtensions.AssureExists(root);
            return root;
        }

        static ToPdfService()
        {
            var path = FolderPath();
            Directory.Delete(path, true);
        }


        public bool IsAvailable => UIPrintInteractionController.PrintingAvailable;

        /// <summary>
        /// Produces PDF from some HTML
        /// </summary>
        /// <param name="popup"></param>
        /// <param name="html"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<ToFileResult> ToPdfAsync(string html, string fileName)
        {
            var taskCompletionSource = new TaskCompletionSource<ToFileResult>();
            ToPdf(taskCompletionSource, html, fileName);
            return await taskCompletionSource.Task;
        }

        public async Task<ToFileResult> ToPdfAsync(WebView webView, string fileName)
        {
            var taskCompletionSource = new TaskCompletionSource<ToFileResult>();
            ToPdf(taskCompletionSource, webView, fileName);
            return await taskCompletionSource.Task;
        }

        public void ToPdf(TaskCompletionSource<ToFileResult> taskCompletionSource, string html, string fileName)
        {
            if (NSProcessInfo.ProcessInfo.IsOperatingSystemAtLeastVersion(new NSOperatingSystemVersion(11, 0, 0)))
            {
                string jScript = @"var meta = document.createElement('meta'); meta.setAttribute('name', 'viewport'); meta.setAttribute('content', 'width=device-width'); document.getElementsByTagName('head')[0].appendChild(meta);";
                WKUserScript wkUScript = new WKUserScript((NSString)jScript, WKUserScriptInjectionTime.AtDocumentEnd, true);
                WKUserContentController wkUController = new WKUserContentController();
                wkUController.AddUserScript(wkUScript);
                var configuration = new WKWebViewConfiguration
                {
                    UserContentController = wkUController
                };
                //webView = new WKWebView(new CGRect(0, 0, (size.Width - 0.5) * 72, (size.Height - 0.5) * 72), configuration)
                var webView = new WKWebView(new CGRect(0, 0, 8.0 * 72, 10.5 * 72), configuration)
                {

                    UserInteractionEnabled = false,
                    BackgroundColor = UIColor.White
                };
                webView.NavigationDelegate = new WKNavigationCompleteCallback(fileName, taskCompletionSource, NavigationComplete);
                webView.LoadHtmlString(html, null);
            }
        }

        /// <summary>
        /// Produces PDF from a Xamarin.Forms.WebView
        /// </summary>
        /// <param name="taskCompletionSource"></param>
        /// <param name="xfWebView"></param>
        /// <param name="fileName"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0067:Dispose objects before losing scope", Justification = "Disposal happens in WKUiCallback")]
        public void ToPdf(TaskCompletionSource<ToFileResult> taskCompletionSource, WebView xfWebView, string fileName)
        {
            if (Platform.CreateRenderer(xfWebView) is WkWebViewRenderer renderer)
            {
                renderer.BackgroundColor = UIColor.White;
                renderer.UserInteractionEnabled = false;
                renderer.NavigationDelegate = new WKNavigationCompleteCallback(fileName, taskCompletionSource, NavigationComplete);
            }
        }

        async Task NavigationComplete(WKWebView webView, string filename, TaskCompletionSource<ToFileResult> taskCompletionSource)
        {
            try
            {
                var widthString = await webView.EvaluateJavaScriptAsync("document.documentElement.offsetWidth");
                var width = double.Parse(widthString.ToString());

                var heightString = await webView.EvaluateJavaScriptAsync("document.documentElement.offsetHeight");
                var height = double.Parse(heightString.ToString());

                webView.ClipsToBounds = false;
                webView.ScrollView.ClipsToBounds = false;

                if (webView.CreatePdfFile(webView.ViewPrintFormatter) is NSMutableData data)
                {
                    var path = System.IO.Path.Combine(ToPngService.FolderPath(), filename + ".pdf");
                    System.IO.File.WriteAllBytes(path, data.ToArray());
                    taskCompletionSource.SetResult(new ToFileResult(false, path));
                    data.Dispose();
                    return;
                }
                taskCompletionSource.SetResult(new ToFileResult(true, "No data returned."));
            }
            catch (Exception e)
            {
                taskCompletionSource.SetResult(new ToFileResult(true, "Exception: " + e.Message + (e.InnerException != null
                    ? "Inner exception: " + e.InnerException.Message
                    : null)));
            }
            finally
            {
                webView.Dispose();
            }

        }

    }

    class PdfRenderer : UIPrintPageRenderer
    {
        public NSMutableData PrintToPdf()
        {
            var pdfData = new NSMutableData();
            UIGraphics.BeginPDFContext(pdfData, PaperRect, null);
            PrepareForDrawingPages(new NSRange(0, NumberOfPages));
            var rect = UIGraphics.PDFContextBounds;
            for (int i = 0; i < NumberOfPages; i++)
            {
                UIGraphics.BeginPDFPage();
                DrawPage(i, rect);
            }
            UIGraphics.EndPDFContent();
            return pdfData;
        }
    }

    static class WKWebViewExtensions
    {
        public static NSMutableData CreatePdfFile(this WebKit.WKWebView webView, UIViewPrintFormatter printFormatter)
        {
            var renderer = new PdfRenderer();
            renderer.AddPrintFormatter(printFormatter, 0);
            // Letter = 8.5" * 72 x 11" * 72
            // Inset = .5"/2 * 72 x 1"/2 * 72
            var page = new CGRect(0, 0, 8.5 * 72, 11 * 72);
            var pdfPageFrame = page.Inset(dx: (nfloat).25 * 72, dy: (nfloat).5 * 72);
            renderer.SetValueForKey(NSValue.FromCGRect(page), new NSString("paperRect"));
            renderer.SetValueForKey(NSValue.FromCGRect(pdfPageFrame), new NSString("printableRect"));
            return renderer.PrintToPdf();
        }

    }


}
