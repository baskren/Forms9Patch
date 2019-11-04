using System;
using System.IO;
using CoreGraphics;
using Foundation;
using UIKit;
using WebKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(Forms9Patch.iOS.HtmlToPngService))]
namespace Forms9Patch.iOS
{
    /// <summary>
    /// HTML to PDF service.
    /// </summary>
    public class HtmlToPngService : IHtmlToPngPdfService
    {
        WKWebView webView;

        /// <summary>
        /// Converts HTML to PNG
        /// </summary>
        /// <param name="html">HTML.</param>
        /// <param name="fileName">File name.</param>
        /// <param name="onComplete">On complete.</param>
        public void ToPng(string html, string fileName, Action<string> onComplete)
        {
            if (NSProcessInfo.ProcessInfo.IsOperatingSystemAtLeastVersion(new NSOperatingSystemVersion(11, 0, 0)))
            {
                var size = new Size(8.5, 11);

                string jScript = @"var meta = document.createElement('meta'); meta.setAttribute('name', 'viewport'); meta.setAttribute('content', 'width=device-width'); document.getElementsByTagName('head')[0].appendChild(meta);";
                WKUserScript wkUScript = new WKUserScript((NSString)jScript, WKUserScriptInjectionTime.AtDocumentEnd, true);
                WKUserContentController wkUController = new WKUserContentController();
                wkUController.AddUserScript(wkUScript);
                var configuration = new WKWebViewConfiguration
                {
                    UserContentController = wkUController
                };
                webView = new WKWebView(new CGRect(0, 0, (size.Width - 0.5) * 72, (size.Height - 0.5) * 72), configuration)
                {
                    NavigationDelegate = new WKUiCallback(size, fileName, onComplete),
                    UserInteractionEnabled = false,
                    BackgroundColor = UIColor.White
                };
                webView.LoadHtmlString(html, null);
            }
        }
    }


    class WKUiCallback : WKNavigationDelegate
    {
        public bool Completed { get; private set; }

        public bool Failed { get; private set; }

        int loadCount;
        Size _size;
        string _filename;
        Action<string> _onComplete;

        public WKUiCallback(Size size, string fileName, Action<string> onComplete)
        {
            _size = size;
            _filename = fileName;
            _onComplete = onComplete;
        }

        public override void DidStartProvisionalNavigation(WKWebView webView, WKNavigation navigation)
        {
            loadCount++;
        }

        public override void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
        {
            loadCount--;
            Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
            {
                if (loadCount <= 0)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var heightString = await webView.EvaluateJavaScriptAsync("document.body.scrollHeight");
                        var height = double.Parse(heightString.ToString());
                        var vertMargin = (nfloat)(0.25 * 72);
                        var horzMargin = vertMargin;
                        var pageMargins = new UIEdgeInsets(vertMargin, horzMargin, vertMargin, horzMargin);

                        webView.ViewPrintFormatter.ContentInsets = pageMargins;
                        webView.ClipsToBounds = false;
                        webView.ScrollView.ClipsToBounds = false;

                        var image = await webView.TakeSnapshotAsync(new WKSnapshotConfiguration
                        {
                            //Rect = new CGRect(0, 0, (_size.Width - 0.5) * 72, height / (Display.Scale / 2.0))
                            Rect = new CGRect(0, 0, (_size.Width - 0.5) * 72, (height / 2) + vertMargin)
                        });

                        if (image.AsPNG() is NSData data)
                        {
                            var path = Path.Combine(P42.Utils.Environment.TemporaryStoragePath, _filename + ".png");
                            File.WriteAllBytes(path, data.ToArray());
                            _onComplete?.Invoke(path);
                            webView.Dispose();
                            return;
                        }

                        Failed = true;
                        _onComplete?.Invoke(null);
                        webView.Dispose();
                    });
                    return false;
                }
                return true;
            });
        }

    }
}
