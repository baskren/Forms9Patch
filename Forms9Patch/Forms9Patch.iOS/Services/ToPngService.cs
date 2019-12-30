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
        public async Task<ToFileResult> ToPngAsync(string html, string fileName)
        {
            var taskCompletionSource = new TaskCompletionSource<ToFileResult>();
            ToPng(taskCompletionSource, html, fileName);
            return await taskCompletionSource.Task;
        }

        /// <summary>
        /// Produces PNG from a WebView
        /// </summary>
        /// <param name="popup"></param>
        /// <param name="webView"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<ToFileResult> ToPngAsync(WebView webView, string fileName)
        {
            var taskCompletionSource = new TaskCompletionSource<ToFileResult>();
            ToPng(taskCompletionSource, webView, fileName);
            return await taskCompletionSource.Task;
        }

        /// <summary>
        /// Produces PNG from HTML
        /// </summary>
        /// <param name="taskCompletionSource"></param>
        /// <param name="html"></param>
        /// <param name="fileName"></param>
        public void ToPng(TaskCompletionSource<ToFileResult> taskCompletionSource, string html, string fileName)
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
        /// Produces PNG from a Xamarin.Forms.WebView
        /// </summary>
        /// <param name="taskCompletionSource"></param>
        /// <param name="xfWebView"></param>
        /// <param name="fileName"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0067:Dispose objects before losing scope", Justification = "Disposal happens in WKUiCallback")]
        public void ToPng(TaskCompletionSource<ToFileResult> taskCompletionSource, WebView xfWebView, string fileName)
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

                var snapshotConfig = new WKSnapshotConfiguration
                {
                    Rect = new CGRect(0, 0, width, height)
                };

                var image = await webView.TakeSnapshotAsync(snapshotConfig);

                if (image.AsPNG() is NSData data)
                {
                    var path = Path.Combine(ToPngService.FolderPath(), filename + ".png");
                    File.WriteAllBytes(path, data.ToArray());
                    taskCompletionSource.SetResult(new ToFileResult(false, path));
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


    class WKNavigationCompleteCallback : WKNavigationDelegate
    {
        public bool Completed { get; private set; }

        int loadCount;
        readonly string _filename;
        readonly TaskCompletionSource<ToFileResult> _taskCompletionSource;
        readonly Func<WKWebView, string, TaskCompletionSource<ToFileResult>, Task> _action;

        public WKNavigationCompleteCallback(string fileName, TaskCompletionSource<ToFileResult> taskCompletionSource, Func<WKWebView, string, TaskCompletionSource<ToFileResult>, Task> action)
        {
            _filename = fileName;
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
                        _action?.Invoke(webView, _filename, _taskCompletionSource);
                    });
                    return false;
                }
                return true;
            });

        }
    }
}
