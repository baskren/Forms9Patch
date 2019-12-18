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
            Directory.Delete(path);
        }

        WKWebView webView;

        public async Task<ToPngResult> ToPngAsync(ActivityIndicatorPopup popup, string html, string fileName)
        {
            var taskCompletionSource = new TaskCompletionSource<ToPngResult>();
            ToPng(taskCompletionSource, html, fileName);
            return await taskCompletionSource.Task;
        }

        public async Task<ToPngResult> ToPngAsync(ActivityIndicatorPopup popup, WebView webView, string fileName)
        {
            var taskCompletionSource = new TaskCompletionSource<ToPngResult>();
            ToPng(taskCompletionSource, webView, fileName);
            return await taskCompletionSource.Task;
        }

        /// <summary>
        /// Produces PNG from HTML
        /// </summary>
        /// <param name="taskCompletionSource"></param>
        /// <param name="html"></param>
        /// <param name="fileName"></param>
        public void ToPng(TaskCompletionSource<ToPngResult> taskCompletionSource, string html, string fileName)
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
                webView = new WKWebView(new CGRect(0, 0, 8.0 * 72, 10.5 * 72), configuration)
                {
                    NavigationDelegate = new WKUiCallback(fileName, taskCompletionSource),
                    UserInteractionEnabled = false,
                    BackgroundColor = UIColor.White
                };
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
        public void ToPng(TaskCompletionSource<ToPngResult> taskCompletionSource, WebView xfWebView, string fileName)
        {
            if (Platform.CreateRenderer(xfWebView) is WkWebViewRenderer renderer)
            {
                //var size = new Size(8.5, 11);
                //string jScript = @"var meta = document.createElement('meta'); meta.setAttribute('name', 'viewport'); meta.setAttribute('content', 'width=device-width'); document.getElementsByTagName('head')[0].appendChild(meta);";
                //WKUserScript wkUScript = new WKUserScript((NSString)jScript, WKUserScriptInjectionTime.AtDocumentEnd, true);
                //WKUserContentController wkUController = new WKUserContentController();
                //wkUController.AddUserScript(wkUScript);
                //renderer.Configuration.UserContentController = wkUController;
                renderer.BackgroundColor = UIColor.White;
                renderer.UserInteractionEnabled = false;
                renderer.NavigationDelegate = new WKUiCallback(fileName, taskCompletionSource);
            }
        }
    }


    class WKUiCallback : WKNavigationDelegate
    {
        public bool Completed { get; private set; }

        public bool Failed { get; private set; }

        int loadCount;
        readonly string _filename;
        readonly TaskCompletionSource<ToPngResult> _taskCompletionSource;

        public WKUiCallback(string fileName, TaskCompletionSource<ToPngResult> taskCompletionSource)
        {
            _filename = fileName;
            _taskCompletionSource = taskCompletionSource;
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
                    Device.BeginInvokeOnMainThread(async () =>
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
                                var path = Path.Combine(ToPngService.FolderPath(), _filename + ".png");
                                File.WriteAllBytes(path, data.ToArray());
                                _taskCompletionSource.SetResult(new ToPngResult(false, path));
                                return;
                            }

                            Failed = true;
                            _taskCompletionSource.SetResult(new ToPngResult(true, "No data returned."));
                        }
                        catch (Exception e)
                        {
                            Failed = true;
                            _taskCompletionSource.SetResult(new ToPngResult(true, "Exception: " + e.Message + (e.InnerException != null
                                ? "Inner exception: " + e.InnerException.Message
                                : null)));
                        }
                        finally
                        {
                            webView.Dispose();
                        }
                    });
                    return false;
                }
                return true;
            });

        }
    }
}
