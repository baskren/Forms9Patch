using System;
using System.IO;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using UIKit;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(Forms9Patch.iOS.ToPngService))]
namespace Forms9Patch.iOS
{
    /// <summary>
    /// HTML to PDF service.
    /// </summary>
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class ToPngService : IToPngService
    {
        const string LocalStorageFolderName = "Forms9Patch.ToPngService";

        public static string FolderPath()
        {
            P42.Utils.DirectoryExtensions.AssureExists(P42.Utils.Environment.TemporaryStoragePath);
            var root = Path.Combine(P42.Utils.Environment.TemporaryStoragePath, LocalStorageFolderName);
            P42.Utils.DirectoryExtensions.AssureExists(root);
            return root;
        }

        static ToPngService()
        {
            var path = FolderPath();
            Directory.Delete(path, true);
        }

        /// <summary>
        /// Produces PNG from some HTML
        /// </summary>
        /// <param name="popup"></param>
        /// <param name="html"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<ToFileResult> ToPngAsync(string html, string fileName, int width)
        {
            if (NSProcessInfo.ProcessInfo.IsOperatingSystemAtLeastVersion(new NSOperatingSystemVersion(11, 0, 0)))
            {
                var taskCompletionSource = new TaskCompletionSource<ToFileResult>();
                ToPng(taskCompletionSource, html, fileName, width);
                return await taskCompletionSource.Task;
            }
            else
                return await Task.FromResult<ToFileResult>(new ToFileResult(true, "PNG output not available prior to iOS 11"));
        }

        /// <summary>
        /// Produces PNG from a WebView
        /// </summary>
        /// <param name="popup"></param>
        /// <param name="webView"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<ToFileResult> ToPngAsync(WebView webView, string fileName, int width)
        {
            if (NSProcessInfo.ProcessInfo.IsOperatingSystemAtLeastVersion(new NSOperatingSystemVersion(11, 0, 0)))
            {
                var taskCompletionSource = new TaskCompletionSource<ToFileResult>();
                ToPng(taskCompletionSource, webView, fileName, width);
                return await taskCompletionSource.Task;
            }
            else
                return await Task.FromResult<ToFileResult>(new ToFileResult(true, "PNG output not available prior to iOS 11"));
        }

        /// <summary>
        /// Produces PNG from HTML
        /// </summary>
        /// <param name="taskCompletionSource"></param>
        /// <param name="html"></param>
        /// <param name="fileName"></param>
        public void ToPng(TaskCompletionSource<ToFileResult> taskCompletionSource, string html, string fileName, int width)
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
                var webView = new WKWebView(new CGRect(0, 0, width, width), configuration)
                {

                    UserInteractionEnabled = false,
                    BackgroundColor = UIColor.White
                };
                webView.NavigationDelegate = new WKNavigationCompleteCallback(fileName, new WebViewToPngSize(width), null, taskCompletionSource, NavigationComplete);
                webView.LoadHtmlString(html, null);
            }
            else
                taskCompletionSource.SetResult(new ToFileResult(true, "PNG output not available prior to iOS 11"));
        }


        /// <summary>
        /// Produces PNG from a Xamarin.Forms.WebView
        /// </summary>
        /// <param name="taskCompletionSource"></param>
        /// <param name="xfWebView"></param>
        /// <param name="fileName"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0067:Dispose objects before losing scope", Justification = "Disposal happens in WKUiCallback")]
        public void ToPng(TaskCompletionSource<ToFileResult> taskCompletionSource, WebView xfWebView, string fileName, int width)
        {
            if (NSProcessInfo.ProcessInfo.IsOperatingSystemAtLeastVersion(new NSOperatingSystemVersion(11, 0, 0)))
            {
                if (Platform.CreateRenderer(xfWebView) is WkWebViewRenderer renderer)
                {
                    renderer.BackgroundColor = UIColor.White;
                    renderer.UserInteractionEnabled = false;
                    renderer.NavigationDelegate = new WKNavigationCompleteCallback(fileName, new WebViewToPngSize(width, renderer.Bounds.ToRectangle()), null, taskCompletionSource, NavigationComplete);
                }
            }
            else
                taskCompletionSource.SetResult(new ToFileResult(true, "PNG output not available prior to iOS 11"));
        }

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

                var bounds = webView.Bounds;
                webView.Bounds = new CGRect(0, 0, (nfloat)width, (nfloat)height);

                var scale = pageSize.Width / width;


                var snapshotConfig = new WKSnapshotConfiguration
                {
                    SnapshotWidth = pageSize.Width / Display.Scale
                };

                var image = await webView.TakeSnapshotAsync(snapshotConfig);

                if (image.AsPNG() is NSData data)
                {
                    var path = Path.Combine(ToPngService.FolderPath(), filename + ".png");
                    File.WriteAllBytes(path, data.ToArray());
                    taskCompletionSource.SetResult(new ToFileResult(false, path));
                    return;
                }
                webView.Bounds = bounds;
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


    class WKNavigationCompleteCallback : WKNavigationDelegate
    {
        public bool Completed { get; private set; }

        int loadCount;
        readonly string _filename;
        readonly PageSize _pageSize;
        readonly PageMargin _margin;
        readonly TaskCompletionSource<ToFileResult> _taskCompletionSource;
        readonly Func<WKWebView, string, PageSize, PageMargin, TaskCompletionSource<ToFileResult>, Task> _action;

        public WKNavigationCompleteCallback(string fileName, PageSize pageSize, PageMargin margin, TaskCompletionSource<ToFileResult> taskCompletionSource, Func<WKWebView, string, PageSize, PageMargin, TaskCompletionSource<ToFileResult>, Task> action)
        {
            _filename = fileName;
            _pageSize = pageSize;
            _margin = margin;
            _taskCompletionSource = taskCompletionSource;
            _action = action;
        }

        public override void DidStartProvisionalNavigation(WKWebView webView, WKNavigation navigation)
        {
            loadCount++;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Potential Code Quality Issues", "RECS0165:Asynchronous methods should return a Task instead of void", Justification = "Needed for BeginInvokeOnMainThread")]
        public override void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
        {
            loadCount--;
            Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
            {
                if (loadCount <= 0)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        _action?.Invoke(webView, _filename, _pageSize, _margin, _taskCompletionSource);
                    });
                    return false;
                }
                return true;
            });

        }
    }
}
