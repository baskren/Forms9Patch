using System.Linq;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Android.Print;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(Forms9Patch.Droid.PrintService))]
namespace Forms9Patch.Droid
{
    public class PrintService : IPrintService
    {
        Forms9Patch.ActivityIndicatorPopup _activityIndicatorPopup;
        IVisualElementRenderer _existingRenderer;

        public void Print(WebView viewToPrint, string jobName)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
            {
                IVisualElementRenderer existingRenderer = Platform.GetRenderer(viewToPrint);
                if ((existingRenderer ?? Platform.CreateRendererWithContext(viewToPrint, Settings.Context)) is IVisualElementRenderer renderer)
                {
                    Android.Webkit.WebView droidWebView = renderer.View as Android.Webkit.WebView;
                    if (droidWebView == null && renderer.View is WebViewRenderer xfWebViewRenderer)
                        droidWebView = xfWebViewRenderer.Control;
                    if (droidWebView != null)
                    {
                        droidWebView.Settings.JavaScriptEnabled = true;
                        droidWebView.Settings.DomStorageEnabled = true;
                        droidWebView.SetLayerType(Android.Views.LayerType.Software, null);

                        // Only valid for API 19+
                        if (string.IsNullOrWhiteSpace(jobName))
                            jobName = Forms9Patch.ApplicationInfoService.Name;
                        var printMgr = (PrintManager)Settings.Context.GetSystemService(Context.PrintService);
                        printMgr.Print(jobName, droidWebView.CreatePrintDocumentAdapter(jobName), null);
                    }

                    if (existingRenderer == null)
                        renderer.Dispose();
                }
            }
        }

        public void Print(string html, string jobName)
        {
            _activityIndicatorPopup = ActivityIndicatorPopup.Create();
            var taskCompletionSource = new TaskCompletionSource<ToFileResult>();
            InnerPrint(taskCompletionSource, html, jobName);
        }

        void InnerPrint(TaskCompletionSource<ToFileResult> taskCompletionSource, string html, string jobName)
        {
            var size = new Size(8.5, 11);
            var webView = new Android.Webkit.WebView(Android.App.Application.Context);
            webView.Settings.JavaScriptEnabled = true;
            webView.Settings.DomStorageEnabled = true;
#pragma warning disable CS0618 // Type or member is obsolete
            webView.DrawingCacheEnabled = true;
#pragma warning restore CS0618 // Type or member is obsolete
            webView.SetLayerType(LayerType.Software, null);

            webView.Layout(0, 0, (int)((size.Width - 0.5) * 72), (int)((size.Height - 0.5) * 72));

            webView.SetWebViewClient(new WebViewCallBack(taskCompletionSource, jobName, OnPageFinished));
            webView.LoadData(html, "text/html; charset=utf-8", "UTF-8");
        }

        async Task OnPageFinished(Android.Webkit.WebView webView, string jobName, TaskCompletionSource<ToFileResult> taskCompletionSource)
        {
            if (string.IsNullOrWhiteSpace(jobName))
                jobName = Forms9Patch.ApplicationInfoService.Name;
            var printMgr = (PrintManager)Settings.Context.GetSystemService(Context.PrintService);
            await Task.Delay(1000); // allow a bit more time for the layout to complete;
            printMgr.Print(jobName, webView.CreatePrintDocumentAdapter(jobName), null);
            _activityIndicatorPopup.Dispose();
            _activityIndicatorPopup = null;
            taskCompletionSource.SetResult(new ToFileResult(false, jobName));
            webView.Dispose();
        }


        public bool CanPrint()
        {
            return Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat;
        }

    }

}
