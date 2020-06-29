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
        public Task<ToFileResult> ToPdfAsync(string html, string fileName, PageSize pageSize, PageMargin margin)
        {
            var taskCompletionSource = new TaskCompletionSource<ToFileResult>();
            ToPdf(taskCompletionSource, html, fileName, pageSize, margin);
            return taskCompletionSource.Task;
        }

        public async Task<ToFileResult> ToPdfAsync(WebView webView, string fileName, PageSize pageSize, PageMargin margin)
        {
            var taskCompletionSource = new TaskCompletionSource<ToFileResult>();
            ToPdf(taskCompletionSource, webView, fileName, pageSize, margin);
            return await taskCompletionSource.Task;
        }

        public void ToPdf(TaskCompletionSource<ToFileResult> taskCompletionSource, string html, string fileName, PageSize pageSize, PageMargin margin)
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
                var webView = new WKWebView(new CGRect(0, 0, pageSize.Width, pageSize.Height), configuration)
                {
                    UserInteractionEnabled = false,
                    BackgroundColor = UIColor.White
                };
                webView.NavigationDelegate = new WKNavigationCompleteCallback(fileName, pageSize, margin,taskCompletionSource, NavigationComplete);
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
        public void ToPdf(TaskCompletionSource<ToFileResult> taskCompletionSource, WebView xfWebView, string fileName, PageSize pageSize, PageMargin margin)
        {
            if (Platform.CreateRenderer(xfWebView) is WkWebViewRenderer renderer)
            {
                renderer.BackgroundColor = UIColor.White;
                renderer.UserInteractionEnabled = false;
                renderer.NavigationDelegate = new WKNavigationCompleteCallback(fileName, pageSize, margin, taskCompletionSource, NavigationComplete);
            }
        }
        /*
        public void ToPdf(TaskCompletionSource<ToFileResult> taskCompletionSource, WebView xfWebView, string fileName)
        {
            if (xfWebView.Effects.Any(e=>e is Forms9Patch.WebViewPrintEffect))
            {

            }
        }
        */

        async Task NavigationComplete(WKWebView webView, string filename, PageSize pageSize, PageMargin margin, TaskCompletionSource<ToFileResult> taskCompletionSource)
        {
            try
            {
                var widthString = await webView.EvaluateJavaScriptAsync("document.documentElement.offsetWidth");
                var width = double.Parse(widthString.ToString());

                var heightString = await webView.EvaluateJavaScriptAsync("document.documentElement.offsetHeight");
                var height = double.Parse(heightString.ToString());

                if (width < 1 || height < 1)
                {
                    taskCompletionSource.SetResult(new ToFileResult(true, "WebView has zero width or height"));
                    return;
                }

                webView.ClipsToBounds = false;
                webView.ScrollView.ClipsToBounds = false;

                if (webView.CreatePdfFile(webView.ViewPrintFormatter, pageSize, margin) is NSMutableData data)
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
            // Second parameter (CGRect bounds) of BeginPDFContext controls the size of the page onto which the content is rendered ... but not the content size.
            // So the content (if bigger) will be clipped (both vertically and horizontally).
            // Also, pagenation is determinted by the content - independent of the below CGRect bounds.
            UIGraphics.BeginPDFContext(pdfData, PaperRect, null);
            //UIGraphics.BeginPDFContext(pdfData, new CGRect(0, 0, 200, 300), null); 
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
        public static NSMutableData CreatePdfFile(this WKWebView webView, UIViewPrintFormatter printFormatter, PageSize pageSize, PageMargin margin)
        {
            var bounds = webView.Bounds;

            //webView.Bounds = new CoreGraphics.CGRect(bounds.X, bounds.Y, bounds.Width, webView.ScrollView.ContentSize.Height);
            webView.Bounds = new CoreGraphics.CGRect(0, 0, (nfloat)pageSize.Width, (nfloat)pageSize.Height);
            margin = margin ?? new PageMargin();
            var pdfPageFrame = new CoreGraphics.CGRect((nfloat)margin.Left, (nfloat)margin.Top, webView.Bounds.Width - margin.HorizontalThickness, webView.Bounds.Height - margin.VerticalThickness);
            //var pdfPageFrame = new CoreGraphics.CGRect(0, 0, 72 * 8, 72 * 10.5);
            var renderer = new PdfRenderer();
            renderer.AddPrintFormatter(printFormatter, 0);
            //renderer.SetValueForKey(NSValue.FromCGRect(UIScreen.MainScreen.Bounds), new NSString("paperRect"));
            renderer.SetValueForKey(NSValue.FromCGRect(webView.Bounds), new NSString("paperRect"));
            renderer.SetValueForKey(NSValue.FromCGRect(pdfPageFrame), new NSString("printableRect"));
            webView.Bounds = bounds;
            return renderer.PrintToPdf();
        }

    }


}
