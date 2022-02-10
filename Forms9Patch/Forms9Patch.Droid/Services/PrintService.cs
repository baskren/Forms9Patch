using System;
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
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class PrintService : IPrintService
    {
        Forms9Patch.ActivityIndicatorPopup _activityIndicatorPopup;

        public Task PrintAsync(WebView viewToPrint, string jobName, FailAction failAction = FailAction.ShowAlert)
        {
            try
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
            catch (Exception e)
            {
                if (failAction == FailAction.ShowAlert)
                {
                    Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        using (var toast = Forms9Patch.Alert.Create("Print Failure", e.Message))
                        {
                            await toast.WaitForPoppedAsync();
                        }
                    });
                }
                else if (failAction == FailAction.ThrowException)
                    throw e;
            }
            return Task.CompletedTask;
        }

        public Task PrintAsync(string html, string jobName, FailAction failAction = FailAction.ShowAlert)
        {
            _activityIndicatorPopup = ActivityIndicatorPopup.Create();
            var taskCompletionSource = new TaskCompletionSource<ToFileResult>();
            InnerPrint(taskCompletionSource, html, jobName, failAction);
            return taskCompletionSource.Task;
        }

        void InnerPrint(TaskCompletionSource<ToFileResult> taskCompletionSource, string html, string jobName, FailAction failAction)
        {
            try
            {
                var size = new Size(8.5, 11);
                var webView = new Android.Webkit.WebView(Android.App.Application.Context);
                webView.Settings.JavaScriptEnabled = true;
                webView.Settings.DomStorageEnabled = true;
#pragma warning disable CS0618 // Type or member is obsolete
                webView.DrawingCacheEnabled = true;
#pragma warning restore CS0618 // Type or member is obsolete
                webView.SetLayerType(LayerType.Software, null);

                //webView.Layout(0, 0, (int)((size.Width - 0.5) * 72), (int)((size.Height - 0.5) * 72));
                webView.Layout(36, 36, (int)((PageSize.Default.Width - 0.5) * 72), (int)((PageSize.Default.Height - 0.5) * 72));

                webView.SetWebViewClient(new WebViewCallBack(taskCompletionSource, jobName, PageSize.Default, null, OnPageFinished));
                webView.LoadData(html, "text/html; charset=utf-8", "UTF-8");
            }
            catch (Exception e)
            {
                if (failAction == FailAction.ShowAlert)
                {
                    Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        using (var toast = Forms9Patch.Alert.Create("Print Failure", e.Message))
                        {
                            await toast.WaitForPoppedAsync();
                        }
                    });
                }
                else if (failAction == FailAction.ThrowException)
                    throw e;
            }
        }

        async Task OnPageFinished(Android.Webkit.WebView webView, string jobName, PageSize pageSize, PageMargin margin, TaskCompletionSource<ToFileResult> taskCompletionSource)
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
